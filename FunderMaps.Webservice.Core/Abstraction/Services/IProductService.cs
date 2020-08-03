using FunderMaps.Webservice.Models.Building;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Abstractions.Services
{
    /// <summary>
    /// Contract for retrieving products from the data store.
    /// </summary>
    public interface IProductService
    {
        Task<BuildingComplete> GetBuildingCompleteByIdAsync(string id, uint pageNumber, uint pageCount);

        Task<BuildingComplete> GetBuildingCompleteByExternalIdAsync(string externalId, uint pageNumber, uint pageCount);

        Task<IEnumerable<BuildingComplete>> GetBuildingCompleteByQueryAsync(string query, uint pageNumber, uint pageCount);

        Task<IEnumerable<BuildingComplete>> GetBuildingCompleteInFenceAsync(uint pageNumber, uint pageCount);
    }
}
