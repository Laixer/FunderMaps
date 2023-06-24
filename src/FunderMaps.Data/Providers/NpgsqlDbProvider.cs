using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace FunderMaps.Data.Providers;

/// <summary>
///     Npgsql database provider.
/// </summary>
internal class NpgsqlDbProvider : DbProvider, IAsyncDisposable
{
    private readonly NpgsqlDataSource _dataSource;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public NpgsqlDbProvider(IConfiguration configuration, IOptions<DbProviderOptions> options)
        : base(options)
    {
        if (_options.ConnectionStringName is null)
        {
            throw new ArgumentNullException(nameof(_options.ConnectionStringName));
        }

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString(_options.ConnectionStringName));

        if (!string.IsNullOrEmpty(_options.ApplicationName))
        {
            dataSourceBuilder.ConnectionStringBuilder.ApplicationName = _options.ApplicationName;
        }

        // dataSourceBuilder.MapEnum<AccessPolicy>("application.access_policy");
        dataSourceBuilder.MapEnum<AccessPolicy>();
        dataSourceBuilder.MapEnum<ApplicationRole>("application.role");
        // dataSourceBuilder.MapEnum<AuditStatus>("report.audit_status");
        dataSourceBuilder.MapEnum<AuditStatus>();
        // dataSourceBuilder.MapEnum<BuildingType>("geocoder.building_type");
        dataSourceBuilder.MapEnum<BuildingType>();
        // dataSourceBuilder.MapEnum<BuiltYearSource>("report.built_year_source");
        dataSourceBuilder.MapEnum<BuiltYearSource>();
        // dataSourceBuilder.MapEnum<ConstructionPile>("report.construction_pile");
        dataSourceBuilder.MapEnum<ConstructionPile>();
        // dataSourceBuilder.MapEnum<CrackType>("report.crack_type");
        dataSourceBuilder.MapEnum<CrackType>();
        // dataSourceBuilder.MapEnum<EnforcementTerm>("report.enforcement_term");
        dataSourceBuilder.MapEnum<EnforcementTerm>();
        // dataSourceBuilder.MapEnum<EnvironmentDamageCharacteristics>("report.environment_damage_characteristics");
        dataSourceBuilder.MapEnum<EnvironmentDamageCharacteristics>();
        dataSourceBuilder.MapEnum<ExternalDataSource>("geocoder.data_source");
        // dataSourceBuilder.MapEnum<Facade>("report.facade");
        dataSourceBuilder.MapEnum<Facade>();
        // dataSourceBuilder.MapEnum<FoundationDamageCause>("report.foundation_damage_cause");
        dataSourceBuilder.MapEnum<FoundationDamageCause>();
        // dataSourceBuilder.MapEnum<FoundationDamageCharacteristics>("report.foundation_damage_characteristics");
        dataSourceBuilder.MapEnum<FoundationDamageCharacteristics>();
        // dataSourceBuilder.MapEnum<FoundationQuality>("report.foundation_quality");
        dataSourceBuilder.MapEnum<FoundationQuality>();
        dataSourceBuilder.MapEnum<FoundationRisk>("data.foundation_risk_indication");
        // dataSourceBuilder.MapEnum<FoundationType>("report.foundation_type");
        dataSourceBuilder.MapEnum<FoundationType>();
        // dataSourceBuilder.MapEnum<IncidentQuestionType>("report.incident_question_type");
        dataSourceBuilder.MapEnum<IncidentQuestionType>();
        // dataSourceBuilder.MapEnum<InquiryType>("report.inquiry_type");
        dataSourceBuilder.MapEnum<InquiryType>();
        // dataSourceBuilder.MapEnum<OrganizationRole>("application.organization_role");
        dataSourceBuilder.MapEnum<OrganizationRole>();
        // dataSourceBuilder.MapEnum<PileType>("report.pile_type");
        dataSourceBuilder.MapEnum<PileType>();
        // dataSourceBuilder.MapEnum<Quality>("report.quality");
        dataSourceBuilder.MapEnum<Quality>();
        // dataSourceBuilder.MapEnum<RecoveryDocumentType>("report.recovery_document_type");
        dataSourceBuilder.MapEnum<RecoveryDocumentType>();
        // dataSourceBuilder.MapEnum<RecoveryStatus>("report.recovery_status");
        dataSourceBuilder.MapEnum<RecoveryStatus>();
        // dataSourceBuilder.MapEnum<RecoveryType>("report.recovery_type");
        dataSourceBuilder.MapEnum<RecoveryType>();
        // dataSourceBuilder.MapEnum<RotationType>("report.rotation_type");
        dataSourceBuilder.MapEnum<RotationType>();
        // dataSourceBuilder.MapEnum<Substructure>("report.substructure");
        dataSourceBuilder.MapEnum<Substructure>();
        // dataSourceBuilder.MapEnum<WoodEncroachement>("report.wood_encroachement");
        dataSourceBuilder.MapEnum<WoodEncroachement>();
        // dataSourceBuilder.MapEnum<WoodQuality>("report.wood_quality");
        dataSourceBuilder.MapEnum<WoodQuality>();
        // dataSourceBuilder.MapEnum<WoodType>("report.wood_type");
        dataSourceBuilder.MapEnum<WoodType>();

        _dataSource = dataSourceBuilder.Build();
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
                    throw new ReferenceNotFoundException(exception.Message, exception);

                case PostgresErrorCodes.AdminShutdown:
                    throw new ServiceUnavailableException(exception.Message, exception);

                case PostgresErrorCodes.NoDataFound:
                    throw new ReferenceNotFoundException(exception.Message, exception);

                case "UX101":
                    throw new InvalidIdentifierException(exception.Message, exception);
            }
        }

        base.HandleException(edi);
    }

    /// <summary>
    ///     Dispose unmanaged resources.
    /// </summary>
    public async ValueTask DisposeAsync() => await _dataSource.DisposeAsync();
}
