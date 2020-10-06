using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Organization proposal repository.
    /// </summary>
    internal class OrganizationProposalRepository : RepositoryBase<OrganizationProposal, Guid>, IOrganizationProposalRepository
    {
        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public OrganizationProposalRepository(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

        /// <summary>
        ///     Create new <see cref="OrganizationProposal"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="OrganizationProposal"/>.</returns>
        public override async ValueTask<Guid> AddAsync(OrganizationProposal entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // FUTURE: Call SP directly
            var sql = @"
                SELECT application.create_organization_proposal(
                    @name,
                    @email)";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("name", entity.Name);
            cmd.AddParameterWithValue("email", entity.Email);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return reader.GetGuid(0);
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    application.create_organization_proposal";

            return ExecuteScalarUnsignedLongCommandAsync(sql);
        }

        /// <summary>
        ///     Delete <see cref="OrganizationProposal"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(Guid id)
        {
            var sql = @"
                DELETE
                FROM    application.organization_proposal
                WHERE   id = @id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);
            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }

        private static OrganizationProposal MapFromReader(DbDataReader reader)
            => new OrganizationProposal
            {
                Id = reader.GetGuid(0),
                Name = reader.GetSafeString(1),
                Email = reader.GetSafeString(2),
            };

        /// <summary>
        ///     Retrieve <see cref="OrganizationProposal"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="OrganizationProposal"/>.</returns>
        public override async ValueTask<OrganizationProposal> GetByIdAsync(Guid id)
        {
            var sql = @"
                SELECT  id,
                        name,
                        email
                FROM    application.organization_proposal
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve <see cref="OrganizationProposal"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="OrganizationProposal"/>.</returns>
        public async ValueTask<OrganizationProposal> GetByNameAsync(string name)
        {
            var sql = @"
                SELECT  id,
                        name,
                        email
                FROM    application.organization_proposal
                WHERE   normalized_name = application.normalize(@name)
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("@name", name);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve <see cref="OrganizationProposal"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="OrganizationProposal"/>.</returns>
        public async ValueTask<OrganizationProposal> GetByEmailAsync(string email)
        {
            var sql = @"
                SELECT  id,
                        name,
                        email
                FROM    application.organization_proposal
                WHERE   normalized_email = application.normalize(@email)
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("email", email);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve all <see cref="OrganizationProposal"/>.
        /// </summary>
        /// <returns>List of <see cref="OrganizationProposal"/>.</returns>
        public override async IAsyncEnumerable<OrganizationProposal> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        name,
                        email
                FROM    application.organization_proposal";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        /// <summary>
        ///     Cannot update a proposal.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override ValueTask UpdateAsync(OrganizationProposal entity)
            => throw new InvalidOperationException();
    }
}
