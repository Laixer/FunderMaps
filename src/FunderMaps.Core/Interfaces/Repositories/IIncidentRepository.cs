using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the incident repository.
/// </summary>
public interface IIncidentRepository : IAsyncRepository<Incident, string>
{
    /// <summary>
    ///     Create new incident.
    /// </summary>
    Task<string> AddAsync(Incident entity);

    /// <summary>
    ///     Count number of incidents.
    /// </summary>
    Task<long> CountAsync();

    /// <summary>
    ///     Retrieve all incidents.
    /// </summary>
    IAsyncEnumerable<Incident> ListAllAsync(Navigation navigation);

    /// <summary>
    ///     Delete incident by identifier.
    /// </summary>
    Task DeleteAsync(string id);

    /// <summary>
    ///     Update incident.
    /// </summary>
    Task UpdateAsync(Incident entity);

    /// <summary>
    ///    Retrieve all incidents by building identifier.
    /// </summary>
    IAsyncEnumerable<Incident> ListAllByBuildingIdAsync(string id);
}
