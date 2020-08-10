using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.ResponseModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Abstractions.Services
{
    /// <summary>
    /// Contains functionality for processing a product with its parameters.
    /// TODO Docs
    /// </summary>
    public interface IProductResultService
    {
        Task<ResponseWrapper> GetAnalysisByIdAsync(Guid userId, AnalysisProductType productType, string id, INavigation navigation, CancellationToken token);

        Task<ResponseWrapper> GetAnalysisByBagIdAsync(Guid userId, AnalysisProductType productType, string bagId, INavigation navigation, CancellationToken token);

        Task<ResponseWrapper> GetAnalysisByQueryAsync(Guid userId, AnalysisProductType productType, string query, INavigation navigation, CancellationToken token);

        Task<ResponseWrapper> GetAnalysisInFenceAsync(Guid userId, AnalysisProductType productType, INavigation navigation, CancellationToken token);

        Task<ResponseWrapper> GetStatisticsByAreaAsync(Guid userId, StatisticsProductType productType, string areaCode, INavigation navigation, CancellationToken token);

        Task<ResponseWrapper> GetStatisticsInFenceAsync(Guid userId, StatisticsProductType productType, INavigation navigation, CancellationToken token);
    }
}
