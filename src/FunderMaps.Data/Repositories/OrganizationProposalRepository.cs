using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories;

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
    public override async Task<Guid> AddAsync(OrganizationProposal entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        // FUTURE: Call SP directly
        var sql = @"
                SELECT application.create_organization_proposal(
                    @name,
                    @email)";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("name", entity.Name);
        context.AddParameterWithValue("email", entity.Email);

        await using var reader = await context.ReaderAsync();

        return reader.GetGuid(0);
    }

    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
                SELECT  COUNT(*)
                FROM    application.organization_proposal";

        await using var context = await DbContextFactory.CreateAsync(sql);

        return await context.ScalarAsync<long>();
    }

    /// <summary>
    ///     Delete <see cref="OrganizationProposal"/>.
    /// </summary>
    /// <param name="id">Entity id.</param>
    public override async Task DeleteAsync(Guid id)
    {
        ResetCacheEntity(id);

        var sql = @"
                DELETE
                FROM    application.organization_proposal
                WHERE   id = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await context.NonQueryAsync();
    }

    private static OrganizationProposal MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
        => new()
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
    public override async Task<OrganizationProposal> GetByIdAsync(Guid id)
    {
        if (TryGetEntity(id, out OrganizationProposal entity))
        {
            return entity;
        }

        var sql = @"
                SELECT  -- OrganizationProposal
                        op.id,
                        op.name,
                        op.email
                FROM    application.organization_proposal AS op
                WHERE   op.id = @id
                LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await using var reader = await context.ReaderAsync();

        return CacheEntity(MapFromReader(reader));
    }

    /// <summary>
    ///     Retrieve all <see cref="OrganizationProposal"/>.
    /// </summary>
    /// <returns>List of <see cref="OrganizationProposal"/>.</returns>
    public override async IAsyncEnumerable<OrganizationProposal> ListAllAsync(Navigation navigation)
    {
        var sql = @"
                SELECT  id,
                        name,
                        email
                FROM    application.organization_proposal";

        sql = ConstructNavigation(sql, navigation);

        await using var context = await DbContextFactory.CreateAsync(sql);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return CacheEntity(MapFromReader(reader));
        }
    }

    /// <summary>
    ///     Cannot update a proposal.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    public override Task UpdateAsync(OrganizationProposal entity)
        => throw new InvalidOperationException();
}
