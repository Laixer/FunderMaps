using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Repositories;
using FunderMaps.Interfaces;
using FunderMaps.Models.Identity;
using FunderMaps.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Organization repository.
    /// </summary>
    public class OrganizationRepository : RepositoryBase<Organization, int>, IOrganizationRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public OrganizationRepository(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

        public override Task<Organization> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public override Task<Organization> AddAsync(Organization entity)
        {
            throw new System.NotImplementedException();
        }

        public override Task<uint> CountAsync()
        {
            throw new System.NotImplementedException();
        }

        public override Task DeleteAsync(Organization entity)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IReadOnlyList<Organization>> ListAllAsync(Navigation navigation)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Get all organizations this user is part of.
        /// </summary>
        /// <param name="user">The user to get organizations by.</param>
        /// <returns>List of organizations.</returns>
        public async Task<Organization> GetOrganizationAsync(FunderMapsUser user)
        {
            var sql = @"
                SELECT org.*
                FROM   application.organization_user AS orguser
                       JOIN application.organization AS org ON org.id = orguser.organization_id
                WHERE  user_id = @UserId
                LIMIT  1";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Organization>(sql, new { UserId = user.Id }));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
        }

        // TODO: convert to role enum
        /// <summary>
        /// Get user role in organization.
        /// </summary>
        /// <param name="organization">The organization to find the role by</param>
        /// <param name="user">The user to find the role by.</param>
        /// <returns>User role or null.</returns>
        public async Task<OrganizationRole> GetRoleAsync(Organization organization, FunderMapsUser user)
        {
            var sql = @"
                SELECT role
                FROM   application.organization_user
                WHERE  user_id = @UserId
                       AND organization_id = @OrganizationId";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<OrganizationRole>("application.organization_role");

            return await RunSqlCommand(async cnn => await cnn.QueryFirstAsync<OrganizationRole>(sql, new { UserId = user.Id, OrganizationId = organization.Id }));
        }

        public override Task UpdateAsync(Organization entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<Organization> GetOrAddAsync(Organization organization)
        {
            throw new System.NotImplementedException();
        }
    }
}
