using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;
using FunderMaps.Interfaces;
using FunderMaps.Providers;
using Dapper;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Sample repository.
    /// </summary>
    public class SampleRepository : EfRepository<FisDbContext, Sample2>, ISampleRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="dbProvider">Database provider.</param>
        public SampleRepository(FisDbContext dbContext, DbProvider dbProvider)
            : base(dbContext, dbProvider)
        {
        }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Sample2"/> on success, null on error.</returns>
        public override async Task<Sample2> GetByIdAsync(int id)
        {
            var sql = @"SELECT samp.id,
                               samp.report,
                               samp.foundation_type,
                               attr.owner AS attribution,
                               samp.monitoring_well,
                               samp.cpt,
                               samp.create_date, 
                               samp.update_date,
                               samp.note,
                               samp.wood_level,
                               samp.groundwater_level,
                               samp.groundlevel,
                               samp.foundation_recovery_adviced,
                               samp.foundation_damage_cause,
                               samp.built_year,
                               samp.foundation_quality,
                               samp.access_policy,
                               samp.enforcement_term,
                               samp.base_measurement_level,
                               addr.*
                        FROM   report.sample AS samp
                               INNER JOIN report.address AS addr ON samp.address = addr.id
                               INNER JOIN report.report AS reprt ON samp.report = reprt.id
                               INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                        WHERE  samp.delete_date IS NULL
                               AND samp.id = @Id
                        LIMIT  1";

            async Task<IEnumerable<Sample2>> map(IDbConnection cnn) =>
                await cnn.QueryAsync<Sample2, Address2, Sample2>(sql, (sampleEntity, addressEntity) =>
                {
                    sampleEntity.Address = addressEntity;
                    return sampleEntity;
                }, new { Id = id });

            var result = await RunSqlCommand(map);
            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
        }

        /// <summary>
        /// Return all samples.
        /// </summary>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<Sample2>> ListAllAsync(Navigation navigation)
        {
            var sql = @"SELECT samp.id,
                               samp.report,
                               samp.foundation_type,
                               attr.owner AS attribution,
                               samp.monitoring_well,
                               samp.cpt,
                               samp.create_date, 
                               samp.update_date,
                               samp.note,
                               samp.wood_level,
                               samp.groundwater_level,
                               samp.groundlevel,
                               samp.foundation_recovery_adviced,
                               samp.foundation_damage_cause,
                               samp.built_year,
                               samp.foundation_quality,
                               samp.access_policy,
                               samp.enforcement_term,
                               samp.base_measurement_level,
                               addr.*
                        FROM   report.sample AS samp
                               INNER JOIN report.address AS addr ON samp.address = addr.id
                               INNER JOIN report.report AS reprt ON samp.report = reprt.id
                               INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                        WHERE  samp.delete_date IS NULL
                        ORDER BY create_date DESC
                        OFFSET @Offset
                        LIMIT @Limit";

            async Task<IEnumerable<Sample2>> map(IDbConnection cnn) =>
                await cnn.QueryAsync<Sample2, Address2, Sample2>(sql, (sampleEntity, addressEntity) =>
                {
                    sampleEntity.Address = addressEntity;
                    return sampleEntity;
                }, navigation);

            var result = await RunSqlCommand(map);
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Return all samples and filter on access policy and organization.
        /// </summary>
        /// <param name="org_id">Organization identifier.</param>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<Sample2>> ListAllAsync(int org_id, Navigation navigation)
        {
            var sql = @"SELECT samp.id,
                               samp.report,
                               samp.foundation_type,
                               attr.owner AS attribution,
                               samp.monitoring_well,
                               samp.cpt,
                               samp.create_date, 
                               samp.update_date,
                               samp.note,
                               samp.wood_level,
                               samp.groundwater_level,
                               samp.groundlevel,
                               samp.foundation_recovery_adviced,
                               samp.foundation_damage_cause,
                               samp.built_year,
                               samp.foundation_quality,
                               samp.access_policy,
                               samp.enforcement_term,
                               samp.base_measurement_level,
                               addr.*
                        FROM   report.sample AS samp
                               INNER JOIN report.address AS addr ON samp.address = addr.id
                               INNER JOIN report.report AS reprt ON samp.report = reprt.id
                               INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                        WHERE  samp.delete_date IS NULL
                                AND (attr.owner = @Owner
                                        OR samp.access_policy = 'public')
                        ORDER BY create_date DESC
                        OFFSET @Offset
                        LIMIT @Limit";

            async Task<IEnumerable<Sample2>> map(IDbConnection cnn) =>
                await cnn.QueryAsync<Sample2, Address2, Sample2>(sql, (sampleEntity, addressEntity) =>
                {
                    sampleEntity.Address = addressEntity;
                    return sampleEntity;
                }, new { Owner = org_id, navigation.Offset, navigation.Limit });

            var result = await RunSqlCommand(map);
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Return all samples by report.
        /// </summary>
        /// <param name="report">Report identifier.</param>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<Sample2>> ListAllReportAsync(int report, Navigation navigation)
        {
            var sql = @"SELECT samp.id,
                               samp.report,
                               samp.foundation_type,
                               attr.owner AS attribution,
                               samp.monitoring_well,
                               samp.cpt,
                               samp.create_date, 
                               samp.update_date,
                               samp.note,
                               samp.wood_level,
                               samp.groundwater_level,
                               samp.groundlevel,
                               samp.foundation_recovery_adviced,
                               samp.foundation_damage_cause,
                               samp.built_year,
                               samp.foundation_quality,
                               samp.access_policy,
                               samp.enforcement_term,
                               samp.base_measurement_level,
                               addr.*
                        FROM   report.sample AS samp
                               INNER JOIN report.address AS addr ON samp.address = addr.id
                               INNER JOIN report.report AS reprt ON samp.report = reprt.id
                               INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                        WHERE  samp.delete_date IS NULL
                                AND reprt.id = @Report
                        ORDER BY create_date DESC
                        OFFSET @Offset
                        LIMIT @Limit";

            async Task<IEnumerable<Sample2>> map(IDbConnection cnn) =>
                await cnn.QueryAsync<Sample2, Address2, Sample2>(sql, (sampleEntity, addressEntity) =>
                {
                    sampleEntity.Address = addressEntity;
                    return sampleEntity;
                }, new { Report = report, navigation.Offset, navigation.Limit });

            var result = await RunSqlCommand(map);
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Return all samples by report and filter on access policy and organization.
        /// </summary>
        /// <param name="report">Report identifier.</param>
        /// <param name="org_id"></param>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<Sample2>> ListAllReportAsync(int report, int org_id, Navigation navigation)
        {
            var sql = @"SELECT samp.id,
                               samp.report,
                               samp.foundation_type,
                               attr.owner AS attribution,
                               samp.monitoring_well,
                               samp.cpt,
                               samp.create_date, 
                               samp.update_date,
                               samp.note,
                               samp.wood_level,
                               samp.groundwater_level,
                               samp.groundlevel,
                               samp.foundation_recovery_adviced,
                               samp.foundation_damage_cause,
                               samp.built_year,
                               samp.foundation_quality,
                               samp.access_policy,
                               samp.enforcement_term,
                               samp.base_measurement_level,
                               addr.*
                        FROM   report.sample AS samp
                               INNER JOIN report.address AS addr ON samp.address = addr.id
                               INNER JOIN report.report AS reprt ON samp.report = reprt.id
                               INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                        WHERE  samp.delete_date IS NULL
                                AND reprt.id = @Report
                                AND (attr.owner = @Owner
                                        OR samp.access_policy = 'public')
                        ORDER BY create_date DESC
                        OFFSET @Offset
                        LIMIT @Limit";

            async Task<IEnumerable<Sample2>> map(IDbConnection cnn) =>
                await cnn.QueryAsync<Sample2, Address2, Sample2>(sql, (sampleEntity, addressEntity) =>
                {
                    sampleEntity.Address = addressEntity;
                    return sampleEntity;
                }, new { Report = report, navigation.Offset, navigation.Limit });

            var result = await RunSqlCommand(map);
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Create new sample.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>Created entity.</returns>
        public override async Task<Sample2> AddAsync(Sample2 entity)
        {
            // TODO: Add address, foundation_type, foundation_damage_cause
            var sql = @"INSERT INTO report.sample AS samp
                                    (report,
                                        monitoring_well,
                                        cpt,
                                        note,
                                        wood_level,
                                        groundlevel,
                                        groundwater_level,
                                        foundation_recovery_adviced,
                                        built_year,
                                        foundation_quality,
                                        enforcement_term,
                                        substructure,
                                        base_measurement_level,
                                        access_policy,
                                        address)
                         VALUES      (@Report,
                                        @MonitoringWell,
                                        @Cpt,
                                        @Note,
                                        @WoodLevel,
                                        @GroundLevel,
                                        @GroundwaterLevel,
                                        @FoundationRecoveryAdviced,
                                        @BuiltYear,
                                        @FoundationQuality,
                                        @EnforcementTerm,
                                        @Substructure,
                                        (enum_range(NULL::report.base_measurement_level))[@BaseMeasurementLevel + 1],
                                        (enum_range(NULL::attestation.access_policy_type))[@AccessPolicy + 1],
                                        @_Address)
                         RETURNING id";

            entity._Address = entity.Address.Id;

            var id = await RunSqlCommand(async cnn => await cnn.ExecuteScalarAsync<int>(sql, entity));
            return await GetByIdAsync(id);
        }

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public override Task UpdateAsync(Sample2 entity)
        {
            // TODO: Add address, foundation_type, foundation_damage_cause
            var sql = @"UPDATE report.sample AS samp
                        SET    monitoring_well = @MonitoringWell,
                                cpt = @Cpt,
                                note = @Note,
                                wood_level = @WoodLevel,
                                groundlevel = @GroundLevel,
                                groundwater_level = @GroundwaterLevel,
                                foundation_recovery_adviced = @FoundationRecoveryAdviced,
                                built_year = @BuiltYear,
                                foundation_quality = @FoundationQuality,
                                enforcement_term = @EnforcementTerm,
                                substructure = @Substructure
                                -- foundation_type = @FoundationType,
                                -- foundation_damage_cause = @FoundationDamageCause,
                                access_policy = (enum_range(NULL::attestation.access_policy_type))[@AccessPolicy + 1]
                        WHERE  samp.delete_date IS NULL
                                AND samp.id = @Id";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public override Task DeleteAsync(Sample2 entity)
        {
            var sql = @"UPDATE report.sample AS samp
                        SET    delete_date = CURRENT_TIMESTAMP
                        WHERE  samp.delete_date IS NULL
                               AND samp.id = @Id";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }

        /// <summary>
        /// Count entities and filter on access policy and organization.
        /// </summary>
        /// <param name="org_id">Organization identifier.</param>
        /// <param name="connection">Optional database connection.</param>
        /// <returns>Number of records.</returns>
        public Task<int> CountAsync(int org_id, IDbConnection connection = null)
        {
            var sql = @"SELECT COUNT(*)
                        FROM   report.sample AS samp
                                INNER JOIN report.address AS addr ON samp.address = addr.id
                                INNER JOIN report.report AS reprt ON samp.report = reprt.id
                                INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                        WHERE  samp.delete_date IS NULL
                                AND (attr.owner = @Owner
                                        OR samp.access_policy = 'public')";

            return RunSqlCommand(async cnn =>
                await cnn.QuerySingleAsync<int>(sql, new { Owner = org_id }),
                connection);
        }

        /// <summary>
        /// Count entities.
        /// </summary>
        /// <returns>Number of records.</returns>
        public override Task<int> CountAsync()
        {
            var sql = @"SELECT COUNT(*)
                        FROM   report.sample AS samp
                                INNER JOIN report.address AS addr ON samp.address = addr.id
                                INNER JOIN report.report AS reprt ON samp.report = reprt.id
                                INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                        WHERE  samp.delete_date IS NULL";

            return RunSqlCommand(async cnn => await cnn.QuerySingleAsync<int>(sql));
        }
    }
}
