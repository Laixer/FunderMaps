using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Organization repository.
/// </summary>
public interface IOrganizationRepository : IAsyncRepository<Organization, Guid>
{
}
