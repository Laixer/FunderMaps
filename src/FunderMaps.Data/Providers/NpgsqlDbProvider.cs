using FunderMaps.Core.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data.Common;

namespace FunderMaps.Data.Providers
{
    /// <summary>
    /// Npgsql database provider.
    /// </summary>
    internal class NpgsqlDbProvider : DbProvider
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="options">Configuration options.</param>
        public NpgsqlDbProvider(IConfiguration configuration, IOptions<DbProviderOptions> options)
            : base(configuration, options)
        {
        }

        // TODO: Move somewhere
        /// <summary>
        /// Static initializer.
        /// </summary>
        static NpgsqlDbProvider()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ApplicationRole>("application.role");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<AuditStatus>("report.audit_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<BaseMeasurementLevel>("report.base_measurement_level");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ConstructionPile>("report.construction_pile");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<CrackType>("report.crack_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EnforcementTerm>("report.enforcement_term");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EnvironmentDamageCharacteristics>("report.environment_damage_characteristics");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Facade>("report.facade");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationDamageCause>("report.foundation_damage_cause");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationDamageCharacteristics>("report.foundation_damage_characteristics");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationQuality>("report.foundation_quality");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationType>("report.foundation_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<IncidentQuestionType>("report.incident_question_type");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<InquiryType>("report.inquiry_type");
            //NpgsqlConnection.GlobalTypeMapper.MapEnum<OrganizationRole>("report.inquiry_type");
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
        /// Create a new Npgsql connection instance.
        /// </summary>
        /// <returns><see cref="DbConnection"/> instance.</returns>
        public override DbConnection ConnectionScope() => new NpgsqlConnection(ConnectionString);

        /// <summary>
        /// Create Npgsql command on the database connection.
        /// </summary>
        /// <param name="cmdText">The text of the query.</param>
        /// <param name="connection">Database connection, see <see cref="DbConnection"/>.</param>
        /// <returns>See <see cref="DbCommand"/>.</returns>
        public override DbCommand CreateCommand(string cmdText, DbConnection connection)
            => new NpgsqlCommand(cmdText, connection as NpgsqlConnection);
    }
}
