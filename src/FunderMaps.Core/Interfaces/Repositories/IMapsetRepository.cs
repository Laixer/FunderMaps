using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the analysis repository.
/// </summary>
public interface IMapsetRepository
{
    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    Task<Mapset> GetAsync(Guid id);

    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    Task<Mapset> GetByOrganizationIdAsync(Guid id);
}
