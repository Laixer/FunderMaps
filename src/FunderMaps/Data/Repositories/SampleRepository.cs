using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Repositories;
using FunderMaps.Interfaces;
using FunderMaps.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Sample repository.
    /// </summary>
    public class SampleRepository : RepositoryBase<Sample, int>, ISampleRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public SampleRepository(DbProvider dbProvider) : base(dbProvider) { }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Sample"/> on success, null on error.</returns>
        public override async Task<Sample> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT  samp.id,
                        samp.report,
                        samp.foundation_type,
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
	                    addr.id,
	                    addr.street_name,
	                    addr.building_number,
	                    addr.building_number_suffix
                FROM    application.sample AS samp
                            INNER JOIN application.address AS addr ON samp.address = addr.id
                            INNER JOIN application.report AS reprt ON samp.report = reprt.id
                            INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE   samp.delete_date IS NULL
                        AND samp.id = @Id
                LIMIT   1";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<BaseLevel>("application.base_measurement_level");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<Substructure>("application.substructure");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<EnforcementTerm>("application.enforcement_term");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationDamageCause>("application.foundation_damage_cause");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationQuality>("application.foundation_quality");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationType>("application.foundation_type");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");

            static Sample map(Sample sampleEntity, Address addressEntity)
            {
                sampleEntity.Address = addressEntity;
                return sampleEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Sample, Address, Sample>(
                sql: sql,
                map: map,
                splitOn: "id",
                param: new { Id = id }));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
        }

        /// <summary>
        /// Retrieve entity by id and organization.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns><see cref="Sample"/> on success, null on error.</returns>
        public async Task<Sample> GetByIdAsync(int id, Guid orgId)
        {
            var sql = @"
                SELECT  samp.id,
                        samp.report,
                        samp.foundation_type,
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
	                    addr.id,
	                    addr.street_name,
	                    addr.building_number,
	                    addr.building_number_suffix
                FROM    application.sample AS samp
                            INNER JOIN application.address AS addr ON samp.address = addr.id
                            INNER JOIN application.report AS reprt ON samp.report = reprt.id
                            INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE   samp.delete_date IS NULL
                        AND samp.id = @Id
                        AND attr.owner = @Owner
                LIMIT  1";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<BaseLevel>("application.base_measurement_level");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<Substructure>("application.substructure");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<EnforcementTerm>("application.enforcement_term");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationDamageCause>("application.foundation_damage_cause");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationQuality>("application.foundation_quality");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationType>("application.foundation_type");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");

            static Sample map(Sample sampleEntity, Address addressEntity)
            {
                sampleEntity.Address = addressEntity;
                return sampleEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Sample, Address, Sample>(
                sql: sql,
                map: map,
                splitOn: "id",
                param: new { Id = id, Owner = orgId }));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
        }

        /// <summary>
        /// Retrieve entity by id and organization or public record.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns><see cref="Sample"/> on success, null on error.</returns>
        public async Task<Sample> GetPublicAndByIdAsync(int id, Guid orgId)
        {
            var sql = @"
                SELECT  samp.id,
                        samp.report,
                        samp.foundation_type,
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
	                    addr.id,
	                    addr.street_name,
	                    addr.building_number,
	                    addr.building_number_suffix
                FROM    application.sample AS samp
                            INNER JOIN application.address AS addr ON samp.address = addr.id
                            INNER JOIN application.report AS reprt ON samp.report = reprt.id
                            INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE   samp.delete_date IS NULL
                        AND samp.id = @Id
                        AND (attr.owner = @Owner
                            OR reprt.access_policy = 'public')
                LIMIT  1";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<BaseLevel>("application.base_measurement_level");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<Substructure>("application.substructure");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<EnforcementTerm>("application.enforcement_term");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationDamageCause>("application.foundation_damage_cause");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationQuality>("application.foundation_quality");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationType>("application.foundation_type");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");

            static Sample map(Sample sampleEntity, Address addressEntity)
            {
                sampleEntity.Address = addressEntity;
                return sampleEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Sample, Address, Sample>(
                sql: sql,
                map: map,
                splitOn: "id",
                param: new { Id = id, Owner = orgId }));
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
        public override async Task<IReadOnlyList<Sample>> ListAllAsync(Navigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  samp.id,
                        samp.report,
                        samp.foundation_type,
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
	                    addr.id,
	                    addr.street_name,
	                    addr.building_number,
	                    addr.building_number_suffix
                FROM    application.sample AS samp
                            INNER JOIN application.address AS addr ON samp.address = addr.id
                            INNER JOIN application.report AS reprt ON samp.report = reprt.id
                            INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE   samp.delete_date IS NULL
                ORDER BY create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<BaseLevel>("application.base_measurement_level");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<Substructure>("application.substructure");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<EnforcementTerm>("application.enforcement_term");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationDamageCause>("application.foundation_damage_cause");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationQuality>("application.foundation_quality");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationType>("application.foundation_type");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");

            static Sample map(Sample sampleEntity, Address addressEntity)
            {
                sampleEntity.Address = addressEntity;
                return sampleEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Sample, Address, Sample>(
                sql: sql,
                map: map,
                splitOn: "id",
                param: navigation));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Return all samples and filter on access policy and organization.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<Sample>> ListAllAsync(Guid orgId, Navigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  samp.id,
                        samp.report,
                        samp.foundation_type,
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
	                    addr.id,
	                    addr.street_name,
	                    addr.building_number,
	                    addr.building_number_suffix
                FROM    application.sample AS samp
                            INNER JOIN application.address AS addr ON samp.address = addr.id
                            INNER JOIN application.report AS reprt ON samp.report = reprt.id
                            INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE   samp.delete_date IS NULL
                        AND (attr.owner = @Owner
                            OR samp.access_policy = 'public')
                ORDER BY create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<BaseLevel>("application.base_measurement_level");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<Substructure>("application.substructure");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<EnforcementTerm>("application.enforcement_term");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationDamageCause>("application.foundation_damage_cause");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationQuality>("application.foundation_quality");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationType>("application.foundation_type");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");

            static Sample map(Sample sampleEntity, Address addressEntity)
            {
                sampleEntity.Address = addressEntity;
                return sampleEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Sample, Address, Sample>(
                sql: sql,
                map: map,
                splitOn: "id",
                param: new { Owner = orgId, navigation.Offset, navigation.Limit }));
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
        public async Task<IReadOnlyList<Sample>> ListAllReportAsync(int report, Navigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  samp.id,
                        samp.report,
                        samp.foundation_type,
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
	                    addr.id,
	                    addr.street_name,
	                    addr.building_number,
	                    addr.building_number_suffix
                FROM    application.sample AS samp
                            INNER JOIN application.address AS addr ON samp.address = addr.id
                            INNER JOIN application.report AS reprt ON samp.report = reprt.id
                            INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE   samp.delete_date IS NULL
                        AND reprt.id = @Report
                ORDER BY create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<BaseLevel>("application.base_measurement_level");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<Substructure>("application.substructure");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<EnforcementTerm>("application.enforcement_term");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationDamageCause>("application.foundation_damage_cause");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationQuality>("application.foundation_quality");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationType>("application.foundation_type");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");

            static Sample map(Sample sampleEntity, Address addressEntity)
            {
                sampleEntity.Address = addressEntity;
                return sampleEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Sample, Address, Sample>(
                sql: sql,
                map: map,
                splitOn: "id",
                param: new { Report = report, navigation.Offset, navigation.Limit }));
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
        /// <param name="orgId"></param>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<Sample>> ListAllReportAsync(int report, Guid orgId, Navigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  samp.id,
                        samp.report,
                        samp.foundation_type,
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
	                    addr.id,
	                    addr.street_name,
	                    addr.building_number,
	                    addr.building_number_suffix
                FROM    application.sample AS samp
                            INNER JOIN application.address AS addr ON samp.address = addr.id
                            INNER JOIN application.report AS reprt ON samp.report = reprt.id
                            INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE   samp.delete_date IS NULL
                        AND reprt.id = @Report
                        AND (attr.owner = @Owner
                            OR samp.access_policy = 'public')
                ORDER BY create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<BaseLevel>("application.base_measurement_level");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<Substructure>("application.substructure");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<EnforcementTerm>("application.enforcement_term");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationDamageCause>("application.foundation_damage_cause");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationQuality>("application.foundation_quality");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationType>("application.foundation_type");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");

            static Sample map(Sample sampleEntity, Address addressEntity)
            {
                sampleEntity.Address = addressEntity;
                return sampleEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Sample, Address, Sample>(
                sql: sql,
                map: map,
                splitOn: "id",
                param: new { Report = report, Owner = orgId, navigation.Offset, navigation.Limit }));
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
        public override Task<int> AddAsync(Sample entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"INSERT INTO application.sample AS samp
                                        (report,
                                        foundation_type,
                                        monitoring_well,
                                        cpt,
                                        note,
                                        wood_level,
                                        groundlevel,
                                        groundwater_level,
                                        foundation_recovery_adviced,
                                        foundation_damage_cause,
                                        built_year,
                                        foundation_quality,
                                        enforcement_term,
                                        substructure,
                                        base_measurement_level,
                                        access_policy,
                                        address)
                         VALUES     (@Report,
                                    @ConvFoundationType::application.foundation_type,
                                    @MonitoringWell,
                                    @Cpt,
                                    @Note,
                                    @WoodLevel,
                                    @GroundLevel,
                                    @GroundwaterLevel,
                                    @FoundationRecoveryAdviced,
                                    @ConvFoundationDamageCause::application.foundation_damage_cause,
                                    @BuiltYear,
                                    @ConvFoundationQuality::application.foundation_quality,
                                    @ConvEnforcementTerm::application.enforcement_term,
                                    @ConvSubstructure::application.substructure,
                                    @ConvBaseMeasurementLevel::application.base_measurement_level,
                                    @ConvAccessPolicy::application.access_policy,
                                    @ConvAddress)
                         RETURNING id";

            var dynamicParameters = new DynamicParameters(entity);
            dynamicParameters.Add("ConvFoundationType", entity.FoundationType.HasValue ? entity.FoundationType.HasValue.ToString().ToSnakeCase() : null);
            dynamicParameters.Add("ConvFoundationDamageCause", entity.FoundationDamageCause.ToString().ToSnakeCase());
            dynamicParameters.Add("ConvFoundationQuality", entity.FoundationQuality.HasValue ? entity.FoundationQuality.ToString().ToSnakeCase() : null);
            dynamicParameters.Add("ConvEnforcementTerm", entity.EnforcementTerm.HasValue ? entity.EnforcementTerm.ToString().ToSnakeCase() : null);
            dynamicParameters.Add("ConvSubstructure", entity.Substructure.HasValue ? entity.Substructure.ToString().ToSnakeCase() : null);
            dynamicParameters.Add("ConvBaseMeasurementLevel", entity.BaseMeasurementLevel.ToString().ToLower()); // NOTE: No ToSnakeCase()
            dynamicParameters.Add("ConvAccessPolicy", entity.AccessPolicy.ToString().ToSnakeCase());
            dynamicParameters.Add("ConvAddress", entity.Address.Id);

            return RunSqlCommand(async cnn => await cnn.ExecuteScalarAsync<int>(sql, dynamicParameters));
        }

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public override Task UpdateAsync(Sample entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                UPDATE  application.sample AS samp
                SET     monitoring_well = @MonitoringWell,
                        cpt = @Cpt,
                        note = @Note,
                        wood_level = @WoodLevel,
                        groundlevel = @GroundLevel,
                        groundwater_level = @GroundwaterLevel,
                        foundation_recovery_adviced = @FoundationRecoveryAdviced,
                        built_year = @BuiltYear,
                        foundation_quality = @ConvFoundationQuality::application.foundation_quality,
                        enforcement_term = @ConvEnforcementTerm::application.enforcement_term,
                        substructure = @ConvSubstructure::application.substructure,
                        foundation_type = @ConvFoundationType::application.foundation_type,
                        foundation_damage_cause = @ConvFoundationDamageCause::application.foundation_damage_cause,
                        access_policy = @ConvAccessPolicy::application.access_policy
                WHERE   samp.delete_date IS NULL
                        AND samp.id = @Id";

            var dynamicParameters = new DynamicParameters(entity);
            dynamicParameters.Add("ConvFoundationType", entity.FoundationType.HasValue ? entity.FoundationType.HasValue.ToString().ToSnakeCase() : null);
            dynamicParameters.Add("ConvFoundationDamageCause", entity.FoundationDamageCause.ToString().ToSnakeCase());
            dynamicParameters.Add("ConvFoundationQuality", entity.FoundationQuality.HasValue ? entity.FoundationQuality.ToString().ToSnakeCase() : null);
            dynamicParameters.Add("ConvEnforcementTerm", entity.EnforcementTerm.HasValue ? entity.EnforcementTerm.ToString().ToSnakeCase() : null);
            dynamicParameters.Add("ConvSubstructure", entity.Substructure.HasValue ? entity.Substructure.ToString().ToSnakeCase() : null);
            dynamicParameters.Add("ConvAccessPolicy", entity.AccessPolicy.ToString().ToSnakeCase());

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, dynamicParameters));
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public override Task DeleteAsync(Sample entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                UPDATE  application.sample AS samp
                SET     delete_date = CURRENT_TIMESTAMP
                WHERE   samp.delete_date IS NULL
                        AND samp.id = @Id";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }

        /// <summary>
        /// Count entities and filter on access policy and organization.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>Number of records.</returns>
        public Task<uint> CountAsync(Guid orgId)
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    application.sample AS samp
                        INNER JOIN application.address AS addr ON samp.address = addr.id
                        INNER JOIN application.report AS reprt ON samp.report = reprt.id
                        INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE   samp.delete_date IS NULL
                        AND (attr.owner = @Owner
                            OR samp.access_policy = 'public')";

            return RunSqlCommand(async cnn => await cnn.QuerySingleAsync<uint>(sql, new { Owner = orgId }));
        }

        /// <summary>
        /// Count entities.
        /// </summary>
        /// <returns>Number of records.</returns>
        public override Task<uint> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    application.sample AS samp
                            INNER JOIN application.address AS addr ON samp.address = addr.id
                            INNER JOIN application.report AS reprt ON samp.report = reprt.id
                            INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE   samp.delete_date IS NULL";

            return RunSqlCommand(async cnn => await cnn.QuerySingleAsync<uint>(sql));
        }
    }
}
