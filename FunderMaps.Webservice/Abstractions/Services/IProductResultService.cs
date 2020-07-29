using FunderMaps.Webservice.Enums;
using FunderMaps.Webservice.ResponseModels;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Abstractions.Services
{
    /// <summary>
    /// Contains functionality for processing a product with its parameters.
    /// TODO Docs
    /// </summary>
    public interface IProductResultService
    {
        Task<ResponseWrapper<BuildingResponseModelBase>> GetBuildingByQueryAsync(Guid userId, ProductType product, string query, uint? limit);

        Task<ResponseWrapper<BuildingResponseModelBase>> GetBuildingByIdAsync(Guid userId, ProductType product, string id);

        Task<ResponseWrapper<BuildingResponseModelBase>> GetBuildingByBagidAsync(Guid userId, ProductType product, string bagid);

        Task<ResponseWrapper<BuildingResponseModelBase>> GetBuildingAllAsync(Guid userId, ProductType product);

        Task<ResponseWrapper<StatisticsResponseModelBase>> GetStatistics(Guid userId, ProductType product);

        // TODO Expand
    }
}
