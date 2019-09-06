using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Entities.Fis;
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
    /// Organization user repository.
    /// </summary>
    public class OrganizationUserRepository : RepositoryBase<OrganizationUser, KeyValuePair<Guid, Guid>>, IOrganizationUserRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public OrganizationUserRepository(DbProvider dbProvider) : base(dbProvider) { }

        /// <summary>
        /// Create new organization user.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created entity primary key.</returns>
        public override async Task<KeyValuePair<Guid, Guid>> AddAsync(OrganizationUser entity)
        {
            var sql = @"
                INSERT INTO application.organization_user
                    (user_id, organization_id, role)
                VALUES
                    (@UserId, @OrganizationId, @ConvRole::application.organization_role)";

            var dynamicParameters = new DynamicParameters(entity);
            dynamicParameters.Add("ConvRole", entity.Role.ToString().ToSnakeCase());

            await RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, dynamicParameters));

            return new KeyValuePair<Guid, Guid>(entity.UserId, entity.OrganizationId);
        }

        /// <summary>
        /// Count entities.
        /// </summary>
        /// <returns>Number of records.</returns>
        public override Task<uint> CountAsync()
        {
            var sql = @"
                SELECT COUNT(*)
                FROM   application.organization_user";

            return RunSqlCommand(async cnn => await cnn.QuerySingleAsync<uint>(sql));
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public override Task DeleteAsync(OrganizationUser entity)
        {
            var sql = @"
                DELETE FROM application.organization_user AS orguser
                WHERE   orguser.user_id = @UserId
                        AND orguser.organization_id = @OrganizationId";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="OrganizationUser"/> on success, null on error.</returns>
        public override async Task<OrganizationUser> GetByIdAsync(KeyValuePair<Guid, Guid> id)
        {
            var sql = @"
                SELECT  orguser.user_id,
                        orguser.organization_id,
                        orguser.role
                FROM    application.organization_user AS orguser
                WHERE   orguser.user_id = @UserId
                        AND orguser.organization_id = @OrganizationId
                LIMIT  1";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<OrganizationRole>("application.organization_role");

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<OrganizationUser>(sql, new { OrganizationId = id.Key, UserId = id.Value }));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
        }

        /// <summary>
        /// Retrieve entity by user id.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>List of entities.</returns>
        public async Task<OrganizationUser> GetByUserIdAsync(Guid userId)
        {
            var sql = @"
                SELECT  orguser.user_id,
                        orguser.organization_id,
                        orguser.role
                FROM    application.organization_user AS orguser
                WHERE   orguser.user_id = @UserId
                LIMIT  1";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<OrganizationRole>("application.organization_role");

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<OrganizationUser>(sql, new { UserId = userId }));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
        }

        /// <summary>
        /// Return all organization users.
        /// </summary>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>List of records.</returns>
        public override async Task<IReadOnlyList<OrganizationUser>> ListAllAsync(Navigation navigation)
        {
            var sql = @"
                SELECT  orguser.user_id,
                        orguser.organization_id,
                        orguser.role
                FROM    application.organization_user
                OFFSET @Offset
                LIMIT @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<OrganizationUser>(sql, navigation));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Return all organization users by organization.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<OrganizationUser>> ListAllByOrganizationIdAsync(Guid orgId, Navigation navigation)
        {
            var sql = @"
                SELECT  orguser.user_id,
                        orguser.organization_id,
                        orguser.role
                FROM    application.organization_user AS orguser
                WHERE   orguser.organization_id = @organizatonId
                OFFSET @Offset
                LIMIT @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<OrganizationUser>(sql, new { organizatonId = orgId, navigation.Offset, navigation.Limit }));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public override Task UpdateAsync(OrganizationUser entity)
        {
            var sql = @"
                UPDATE  application.organization_user AS orguser
                SET     role = @ConvRole::application.organization_role
                WHERE   orguser.user_id = @UserId
                        AND orguser.organization_id = @OrganizationId";

            var dynamicParameters = new DynamicParameters(entity);
            dynamicParameters.Add("ConvRole", entity.Role.ToString().ToSnakeCase());

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, dynamicParameters));
        }
    }
}
