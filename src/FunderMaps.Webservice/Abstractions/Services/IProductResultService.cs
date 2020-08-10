using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.ResponseModels;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Abstractions.Services
{
    /// <summary>
    /// Contains functionality for processing a product with its parameters.
    /// TODO Docs
    /// TODO Is it correct to use the core enums here?
    /// </summary>
    public interface IProductResultService
    {
        Task<ResponseWrapper> GetAnalysisByIdAsync(Guid userId, AnalysisProductType productType, string id, INavigation navigation);

        Task<ResponseWrapper> GetAnalysisByBagIdAsync(Guid userId, AnalysisProductType productType, string bagId, INavigation navigation);

        Task<ResponseWrapper> GetAnalysisByQueryAsync(Guid userId, AnalysisProductType productType, string query, INavigation navigation);

        Task<ResponseWrapper> GetAnalysisInFenceAsync(Guid userId, AnalysisProductType productType, INavigation navigation);

        Task<ResponseWrapper> GetStatisticsByAreaAsync(Guid userId, StatisticsProductType productType, string areaCode, INavigation navigation);

        Task<ResponseWrapper> GetStatisticsInFenceAsync(Guid userId, StatisticsProductType productType, INavigation navigation);
    }
}
