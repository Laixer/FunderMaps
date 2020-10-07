using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("name", entity.Name);
            context.AddParameterWithValue("email", entity.Email);

            await using var reader = await context.ReaderAsync();

            return reader.GetGuid(0);
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async ValueTask<long> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    application.create_organization_proposal";

            await using var context = await DbContextFactory(sql);

            return await context.ScalarAsync<long>();
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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        private static OrganizationProposal MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
            => new OrganizationProposal
            {
                Id = reader.GetGuid(offset + 0),
                Name = reader.GetSafeString(offset + 1),
                Email = reader.GetSafeString(offset + 2),
            };

        /// <summary>
        ///     Retrieve <see cref="OrganizationProposal"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="OrganizationProposal"/>.</returns>
        public override async ValueTask<OrganizationProposal> GetByIdAsync(Guid id)
        {
            var sql = @"
                SELECT  -- OrganizationProposal
                        op.id,
                        op.name,
                        op.email
                FROM    application.organization_proposal AS op
                WHERE   op.id = @id
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

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
                SELECT  -- OrganizationProposal
                        op.id,
                        op.name,
                        op.email
                FROM    application.organization_proposal AS op
                WHERE   op.normalized_name = application.normalize(@name)
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("@name", name);

            await using var reader = await context.ReaderAsync();

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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("email", email);

            await using var reader = await context.ReaderAsync();

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

            await using var context = await DbContextFactory(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
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
