using FunderMaps.Core.Types;
using FunderMaps.Core.Types.MapLayer;
using FunderMaps.Core.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Data.Providers
{
    /// <summary>
    ///     Npgsql database provider.
    /// </summary>
    internal class NpgsqlDbProvider : DbProvider
    {
        private string _connectionString;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public NpgsqlDbProvider(IConfiguration configuration, IOptions<DbProviderOptions> options)
            : base(configuration, options)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString(_options.ConnectionStringName));

            connectionStringBuilder.Timeout = _options.ConnectionTimeout > 0 ? _options.ConnectionTimeout : connectionStringBuilder.Timeout;
            connectionStringBuilder.MinPoolSize = _options.MinPoolSize > 0 ? _options.MinPoolSize : connectionStringBuilder.MinPoolSize;
            connectionStringBuilder.MaxPoolSize = _options.MaxPoolSize > 0 ? _options.MaxPoolSize : connectionStringBuilder.MaxPoolSize;
            connectionStringBuilder.CommandTimeout = _options.CommandTimeout > 0 ? _options.CommandTimeout : connectionStringBuilder.CommandTimeout;

            if (!string.IsNullOrEmpty(_options.ApplicationName))
            {
                connectionStringBuilder.ApplicationName = _options.ApplicationName;
            }

            _connectionString = connectionStringBuilder.ConnectionString;
        }

        // FUTURE: Move somewhere. Too npgsql specific
        /// <summary>
        ///     Static initializer.
        /// </summary>
        static NpgsqlDbProvider()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ApplicationRole>("application.role");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<AuditStatus>("report.audit_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<BaseMeasurementLevel>("report.base_measurement_level");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<BuildingType>("geocoder.building_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<BuiltYearSource>("report.built_year_source");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<BundleStatus>("maplayer.bundle_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ConstructionPile>("report.construction_pile");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<CrackType>("report.crack_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EnforcementTerm>("report.enforcement_term");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EnvironmentDamageCharacteristics>("report.environment_damage_characteristics");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ExternalDataSource>("geocoder.data_source");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Facade>("report.facade");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationDamageCause>("report.foundation_damage_cause");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationDamageCharacteristics>("report.foundation_damage_characteristics");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationQuality>("report.foundation_quality");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationRisk>("data.foundation_risk_indication");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationType>("report.foundation_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<IncidentQuestionType>("report.incident_question_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<InquiryType>("report.inquiry_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OrganizationRole>("application.organization_role");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<PileType>("report.pile_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ProjectSampleStatus>("report.project_sample_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Quality>("report.quality");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<RecoveryDocumentType>("report.recovery_document_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<RecoveryStatus>("report.recovery_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<RecoveryType>("report.recovery_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<RotationType>("report.rotation_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Substructure>("report.substructure");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<WoodEncroachement>("report.wood_encroachement");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<WoodQuality>("report.wood_quality");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<WoodType>("report.wood_type");
        }

        /// <summary>
        ///     <see cref="DbProvider.ConnectionScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override DbConnection ConnectionScope()
            => new NpgsqlConnection(_connectionString);

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
                    case Npgsql.PostgresErrorCodes.ForeignKeyViolation:
                        throw new ReferenceNotFoundException(exception.Message, exception);
                }
            }

            base.HandleException(edi);
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
