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
        Task<BuildingComplete> GetBuildingCompleteByIdAsync(string id);

        Task<BuildingComplete> GetBuildingCompleteByBagidAsync(string id);

        Task<IEnumerable<BuildingComplete>> GetBuildingCompleteByQueryAsync(string query, uint? limit);

        Task<IEnumerable<BuildingComplete>> GetBuildingCompleteAllAsync();
    }
}
