using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace FunderMaps.Data.Providers;

/// <summary>
///     Npgsql database provider.
/// </summary>
internal class NpgsqlDbProvider : DbProvider, IDisposable, IAsyncDisposable
{
    private readonly NpgsqlDataSourceBuilder _dataSourceBuilder;
    private readonly NpgsqlDataSource _dataSource;
    private readonly ILogger<NpgsqlDbProvider> _logger;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public NpgsqlDbProvider(IOptions<DbProviderOptions> options, ILogger<NpgsqlDbProvider> logger)
        : base(options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_options.ConnectionString);

        if (!string.IsNullOrEmpty(_options.ApplicationName))
        {
            dataSourceBuilder.ConnectionStringBuilder.ApplicationName = _options.ApplicationName;
        }

        dataSourceBuilder.MapEnum<AccessPolicy>();
        dataSourceBuilder.MapEnum<ApplicationRole>("application.role");
        dataSourceBuilder.MapEnum<AuditStatus>();
        dataSourceBuilder.MapEnum<ConstructionPile>();
        dataSourceBuilder.MapEnum<CrackType>();
        dataSourceBuilder.MapEnum<EnforcementTerm>();
        dataSourceBuilder.MapEnum<EnvironmentDamageCharacteristics>();
        dataSourceBuilder.MapEnum<Facade>();
        dataSourceBuilder.MapEnum<FoundationDamageCause>();
        dataSourceBuilder.MapEnum<FoundationDamageCharacteristics>();
        dataSourceBuilder.MapEnum<FoundationQuality>();
        dataSourceBuilder.MapEnum<FoundationRisk>("data.foundation_risk_indication");
        dataSourceBuilder.MapEnum<FoundationType>();
        dataSourceBuilder.MapEnum<IncidentQuestionType>();
        dataSourceBuilder.MapEnum<InquiryType>();
        dataSourceBuilder.MapEnum<OrganizationRole>();
        dataSourceBuilder.MapEnum<PileType>();
        dataSourceBuilder.MapEnum<Quality>();
        dataSourceBuilder.MapEnum<RecoveryDocumentType>();
        dataSourceBuilder.MapEnum<RecoveryStatus>();
        dataSourceBuilder.MapEnum<RecoveryType>();
        dataSourceBuilder.MapEnum<Reliability>();
        dataSourceBuilder.MapEnum<RotationType>();
        dataSourceBuilder.MapEnum<Substructure>();
        dataSourceBuilder.MapEnum<WoodEncroachement>();
        dataSourceBuilder.MapEnum<WoodQuality>();
        dataSourceBuilder.MapEnum<WoodType>();

        _dataSourceBuilder = dataSourceBuilder;

        _dataSource = dataSourceBuilder.Build();
    }

    /// <summary>
    ///     Get the connection as URI.
    /// </summary>
    public override string ConnectionUri
    {
        get
        {
            var username = _dataSourceBuilder.ConnectionStringBuilder.Username;
            var password = _dataSourceBuilder.ConnectionStringBuilder.Password;
            var host = _dataSourceBuilder.ConnectionStringBuilder.Host;
            var port = _dataSourceBuilder.ConnectionStringBuilder.Port;
            var database = _dataSourceBuilder.ConnectionStringBuilder.Database;

            return $"postgresql://{username}:{password}@{host}:{port}/{database}"; // ?connect_timeout=10&application_name=myapp        
        }
    }

    /// <summary>
    ///     <see cref="DbProvider.ConnectionScope"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override DbConnection ConnectionScope() => _dataSource.CreateConnection();

    /// <summary>
    ///     <see cref="DbProvider.CreateCommand(string, DbConnection)"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override DbCommand CreateCommand(string cmdText, DbConnection connection)
        => new NpgsqlCommand(cmdText, connection as NpgsqlConnection);

    /// <summary>
    ///     <see cref="DbProvider.HandleException(ExceptionDispatchInfo)"/>
    /// </summary>
    internal override void HandleException(ExceptionDispatchInfo edi)
    {
        if (edi.SourceException is PostgresException exception)
        {
            switch (exception.SqlState)
            {
                case PostgresErrorCodes.ForeignKeyViolation:
                    _logger.LogWarning(exception, "Foreign key violation");
                    throw new ReferenceNotFoundException(exception.Message, exception);

                case PostgresErrorCodes.AdminShutdown:
                    _logger.LogWarning(exception, "Database shutdown");
                    throw new ServiceUnavailableException(exception.Message, exception);

                case PostgresErrorCodes.NoDataFound:
                    throw new ReferenceNotFoundException(exception.Message, exception);

                case "UX101":
                    _logger.LogWarning(exception, "Unique constraint violation");
                    throw new InvalidIdentifierException(exception.Message, exception);

                default:
                    _logger.LogWarning(exception, "Unhandled database exception");
                    throw new DatabaseException(exception.Message, exception);
            }
        }

        base.HandleException(edi);
    }

    /// <summary>
    ///     Dispose unmanaged resources.
    /// </summary>
    public void Dispose() => _dataSource.Dispose();

    /// <summary>
    ///     Dispose unmanaged resources.
    /// </summary>
    public async ValueTask DisposeAsync() => await _dataSource.DisposeAsync();
}
