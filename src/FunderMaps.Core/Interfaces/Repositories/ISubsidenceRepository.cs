using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

public interface ISubsidenceRepository
{
    /// <summary>
    ///    List all history by building id.
    /// </summary>
    IAsyncEnumerable<SubsidenceHistory> ListAllHistoryByIdAsync(string id);
}
