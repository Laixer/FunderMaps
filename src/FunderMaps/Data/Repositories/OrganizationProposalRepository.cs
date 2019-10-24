using Dapper;
using FunderMaps.Core.Entities;
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
    /// Organization proposal repository.
    /// </summary>
    public class OrganizationProposalRepository : RepositoryBase<OrganizationProposal, Guid>, IOrganizationProposalRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public OrganizationProposalRepository(DbProvider dbProvider) : base(dbProvider) { }

        /// <summary>
        /// Retrieve entity by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="OrganizationProposal"/> on success, null on error.</returns>
        public override async Task<OrganizationProposal> GetByIdAsync(Guid id)
        {
            var sql = @"
                SELECT  prop.token,
                        prop.name,
                        prop.normalized_name,
                        prop.email
                FROM    application.organization_proposal AS prop
                WHERE   prop.token = @Token
                LIMIT   1";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<OrganizationProposal>(sql, new { Token = id }));
            if (!result.Any())
            {
                return null;
            }

            return result.First();
        }

        /// <summary>
        /// Retrieve entity by name.
        /// </summary>
        /// <param name="name">Organization name.</param>
        /// <returns><see cref="OrganizationProposal"/> on success, null on error.</returns>
        public async Task<OrganizationProposal> GetByNormalizedNameAsync(string name)
        {
            var sql = @"
                SELECT  prop.token,
                        prop.name,
                        prop.normalized_name,
                        prop.email
                FROM    application.organization_proposal AS prop
                WHERE   prop.normalized_name = @NormalizedName
                LIMIT   1";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<OrganizationProposal>(sql, new { NormalizedName = name }));
            if (!result.Any())
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
        public override async Task<IReadOnlyList<OrganizationProposal>> ListAllAsync(Navigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  prop.token,
                        prop.name,
                        prop.normalized_name,
                        prop.email
                FROM    application.organization_proposal AS prop
                OFFSET  @Offset
                LIMIT   @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<OrganizationProposal>(sql, navigation));
            if (!result.Any())
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Create new organization proposal.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>Created entity.</returns>
        public override Task<Guid> AddAsync(OrganizationProposal entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                INSERT INTO application.organization_proposal
                    (name, normalized_name, email)
	            VALUES
                    (@Name, @NormalizedName, @Email)
                RETURNING token";

            return RunSqlCommand(async cnn => await cnn.ExecuteScalarAsync<Guid>(sql, entity));
        }

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public override Task UpdateAsync(OrganizationProposal entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                UPDATE application.organization_proposal AS prop
                SET    name = @Name,
                       normalized_name = @NormalizedName,
                       email = @Email,
                WHERE  prop.token = @Token";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public override Task DeleteAsync(OrganizationProposal entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                DELETE FROM application.organization_proposal AS prop
                WHERE  prop.token = @Token";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }

        /// <summary>
        /// Count entities.
        /// </summary>
        /// <returns>Number of records.</returns>
        public override Task<uint> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    application.organization_proposal";

            return RunSqlCommand(async cnn => await cnn.QuerySingleAsync<uint>(sql));
        }
    }
}
