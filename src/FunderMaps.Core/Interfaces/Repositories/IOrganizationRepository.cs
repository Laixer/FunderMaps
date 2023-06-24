using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Organization repository.
/// </summary>
public interface IOrganizationRepository : IAsyncRepository<Organization, Guid>
{
    /// <summary>
    ///     Create new <see cref="Organization"/> from a proposal.
    /// </summary>
    /// <param name="id">Organization proposal identifier.</param>
    /// <param name="email">Superuser email.</param>
    /// <param name="passwordHash">Superuser password.</param>
    /// <returns>Created <see cref="Organization"/> identifier.</returns>
    Task<Guid> AddFromProposalAsync(Guid id, string email, string passwordHash);
}
