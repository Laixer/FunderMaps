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
    /// Foundation recovery repository.
    /// </summary>
    public class FoundationRecoveryRepository : RepositoryBase<FoundationRecovery, int>, IFoundationRecoveryRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public FoundationRecoveryRepository(DbProvider dbProvider) : base(dbProvider) { }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="FoundationRecovery"/> on success, null on error.</returns>
        public override async Task<FoundationRecovery> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT  reco.id,
                        reco.note,
                        reco.create_date,
                        reco.update_date,
                        reco.delete_date,
                        reco.year,
                        reco.type,
                        reco.access_policy,
                        reco.repair,
	                    addr.id,
	                    addr.street_name,
	                    addr.building_number,
	                    addr.building_number_suffix,
                        attr.id,
                        attr.project,
						attr.reviewer,
						attr.creator,
						attr.owner,
						attr.contractor
                FROM    application.foundation_recovery AS reco
                            INNER JOIN application.address AS addr ON reco.address = addr.id
                            INNER JOIN application.attribution AS attr ON reco.attribution = attr.id
                WHERE   reco.delete_date IS NULL
                        AND reco.id = @Id
                LIMIT   1";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationRecoveryLocation>("application.foundation_recovery_location");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationRecoveryType>("application.foundation_recovery_type");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");

            FoundationRecovery map(FoundationRecovery foundationRecoveryEntity, Address addressEntity, Attribution attributionEntity)
            {
                foundationRecoveryEntity.Address = addressEntity;
                foundationRecoveryEntity.Attribution = attributionEntity;
                return foundationRecoveryEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<FoundationRecovery, Address, Attribution, FoundationRecovery>(
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
        /// <returns><see cref="FoundationRecovery"/> on success, null on error.</returns>
        public async Task<FoundationRecovery> GetByIdAsync(int id, Guid orgId)
        {
            var sql = @"
                SELECT  reco.id,
                        reco.note,
                        reco.create_date,
                        reco.update_date,
                        reco.delete_date,
                        reco.year,
                        reco.type,
                        reco.access_policy,
                        reco.repair,
	                    addr.id,
	                    addr.street_name,
	                    addr.building_number,
	                    addr.building_number_suffix,
                        attr.id,
                        attr.project,
						attr.reviewer,
						attr.creator,
						attr.owner,
						attr.contractor
                FROM    application.foundation_recovery AS reco
                            INNER JOIN application.address AS addr ON reco.address = addr.id
                            INNER JOIN application.attribution AS attr ON reco.attribution = attr.id
                WHERE   reco.delete_date IS NULL
                        AND reco.id = @Id
                        AND attr.owner = @Owner
                LIMIT   1";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationRecoveryLocation>("application.foundation_recovery_location");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationRecoveryType>("application.foundation_recovery_type");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");

            FoundationRecovery map(FoundationRecovery foundationRecoveryEntity, Address addressEntity, Attribution attributionEntity)
            {
                foundationRecoveryEntity.Address = addressEntity;
                foundationRecoveryEntity.Attribution = attributionEntity;
                return foundationRecoveryEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<FoundationRecovery, Address, Attribution, FoundationRecovery>(
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
        /// <returns><see cref="FoundationRecovery"/> on success, null on error.</returns>
        public async Task<FoundationRecovery> GetPublicAndByIdAsync(int id, Guid orgId)
        {
            var sql = @"
                SELECT  reco.id,
                        reco.note,
                        reco.create_date,
                        reco.update_date,
                        reco.delete_date,
                        reco.year,
                        reco.type,
                        reco.access_policy,
                        reco.repair,
	                    addr.id,
	                    addr.street_name,
	                    addr.building_number,
	                    addr.building_number_suffix,
                        attr.id,
                        attr.project,
						attr.reviewer,
						attr.creator,
						attr.owner,
						attr.contractor
                FROM    application.foundation_recovery AS reco
                            INNER JOIN application.address AS addr ON reco.address = addr.id
                            INNER JOIN application.attribution AS attr ON reco.attribution = attr.id
                WHERE   reco.delete_date IS NULL
                        AND reco.id = @Id
                        AND (attr.owner = @Owner
                            OR reco.access_policy = 'public')
                LIMIT   1";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationRecoveryLocation>("application.foundation_recovery_location");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationRecoveryType>("application.foundation_recovery_type");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");

            FoundationRecovery map(FoundationRecovery foundationRecoveryEntity, Address addressEntity, Attribution attributionEntity)
            {
                foundationRecoveryEntity.Address = addressEntity;
                foundationRecoveryEntity.Attribution = attributionEntity;
                return foundationRecoveryEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<FoundationRecovery, Address, Attribution, FoundationRecovery>(
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
        /// Returns all the records.
        /// </summary>
        /// <param name="navigation">The navigation paramters for offsetting en limiting.</param>
        /// <returns>List of records.</returns>
        public override async Task<IReadOnlyList<FoundationRecovery>> ListAllAsync(Navigation navigation)
        {
            var sql = @"
                SELECT  reco.id,
                        reco.note,
                        reco.create_date,
                        reco.update_date,
                        reco.delete_date,
                        reco.year,
                        reco.type,
                        reco.access_policy,
                        reco.repair,
	                    addr.id,
	                    addr.street_name,
	                    addr.building_number,
	                    addr.building_number_suffix,
                        attr.id,
                        attr.project,
						attr.reviewer,
						attr.creator,
						attr.owner,
						attr.contractor
                FROM    application.foundation_recovery AS reco
                            INNER JOIN application.address AS addr ON reco.address = addr.id
                            INNER JOIN application.attribution AS attr ON reco.attribution = attr.id
                WHERE   reco.delete_date IS NULL
                ORDER BY reco.create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationRecoveryLocation>("application.foundation_recovery_location");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationRecoveryType>("application.foundation_recovery_type");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");

            FoundationRecovery map(FoundationRecovery foundationRecoveryEntity, Address addressEntity, Attribution attributionEntity)
            {
                foundationRecoveryEntity.Address = addressEntity;
                foundationRecoveryEntity.Attribution = attributionEntity;
                return foundationRecoveryEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<FoundationRecovery, Address, Attribution, FoundationRecovery>(
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
        /// Admin function. Returns ALL the records.
        /// </summary>
        /// <param name="orgId">The id of the organization.</param>
        /// <param name="navigation">The navigation paramters for offsetting en limiting.</param>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<FoundationRecovery>> ListAllAsync(Guid orgId, Navigation navigation)
        {
            var sql = @"
                SELECT  reco.id,
                        reco.note,
                        reco.create_date,
                        reco.update_date,
                        reco.delete_date,
                        reco.year,
                        reco.type,
                        reco.access_policy,
                        reco.repair,
	                    addr.id,
	                    addr.street_name,
	                    addr.building_number,
	                    addr.building_number_suffix,
                        attr.id,
                        attr.project,
						attr.reviewer,
						attr.creator,
						attr.owner,
						attr.contractor
                FROM    application.foundation_recovery AS reco
                            INNER JOIN application.address AS addr ON reco.address = addr.id
                            INNER JOIN application.attribution AS attr ON reco.attribution = attr.id
                WHERE   reco.delete_date IS NULL
                        AND (attr.owner = @Owner
                            OR reco.access_policy = 'public')
                ORDER BY reco.create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationRecoveryLocation>("application.foundation_recovery_location");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<FoundationRecoveryType>("application.foundation_recovery_type");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");

            FoundationRecovery map(FoundationRecovery foundationRecoveryEntity, Address addressEntity, Attribution attributionEntity)
            {
                foundationRecoveryEntity.Address = addressEntity;
                foundationRecoveryEntity.Attribution = attributionEntity;
                return foundationRecoveryEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<FoundationRecovery, Address, Attribution, FoundationRecovery>(
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
        /// Create new foundation recovery.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>Created entity.</returns>
        public override async Task<int> AddAsync(FoundationRecovery entity)
        {
            var sql = @"
                INSERT INTO application.attribution
                    (project, reviewer, creator, owner, contractor)
	            VALUES
                    (@Project, @Reviewer, @Creator, @Owner, @Contractor)
                RETURNING id";

            var attributionId = await RunSqlCommand(async cnn => await cnn.ExecuteScalarAsync<int>(sql, entity.Attribution));

            var sql2 = @"
                INSERT INTO application.foundation_recovery
                                (year,
                                note,
                                type,
                                access_policy,
                                attribution,
                                address)
                VALUES      (@Year,
                            @Note,
                            @ConvType::application.foundation_recovery_type,
                            @ConvAccessPolicy::application.access_policy,
                            @ConvAttribution,
                            @ConvAddress)
                RETURNING id";

            var dynamicParameters = new DynamicParameters(entity);
            dynamicParameters.Add("ConvType", entity.Type.ToString().ToSnakeCase());
            dynamicParameters.Add("ConvAccessPolicy", entity.AccessPolicy.ToString().ToSnakeCase());
            dynamicParameters.Add("ConvAttribution", attributionId);
            dynamicParameters.Add("ConvAddress", entity.Address.Id);

            return await RunSqlCommand(async cnn => await cnn.ExecuteScalarAsync<int>(sql2, dynamicParameters));
        }

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public override Task UpdateAsync(FoundationRecovery entity)
        {
            var sql = @"
                UPDATE application.foundation_recovery AS reco
                SET    year = @Year,
                       note = @Note,
                       type = @ConvType::application.foundation_recovery_type,
                       access_policy = @ConvAccessPolicy::application.access_policy
                WHERE  reco.delete_date IS NULL
                       AND reco.id = @Id";

            var dynamicParameters = new DynamicParameters(entity);
            dynamicParameters.Add("ConvType", entity.Type.ToString().ToSnakeCase());
            dynamicParameters.Add("ConvAccessPolicy", entity.AccessPolicy.ToString().ToSnakeCase());

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, dynamicParameters));
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public override Task DeleteAsync(FoundationRecovery entity)
        {
            var sql = @"
                UPDATE  application.foundation_recovery AS reco
                SET     delete_date = CURRENT_TIMESTAMP
                WHERE   reco.delete_date IS NULL
                        AND reco.id = @Id";

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
                FROM    application.foundation_recovery as reco
                            INNER JOIN application.attribution AS attr ON reco.attribution = attr.id
                WHERE   reco.delete_date IS NULL
                        AND (attr.owner = @Owner
                            OR reco.access_policy = 'public')";

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
                FROM    application.foundation_recovery as reco
                WHERE   reco.delete_date IS NULL";

            return RunSqlCommand(async cnn => await cnn.QuerySingleAsync<uint>(sql));
        }
    }
}
