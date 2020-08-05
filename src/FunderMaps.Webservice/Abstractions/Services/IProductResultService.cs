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
        Task<ResponseWrapper> GetAnalysisByIdAsync(Guid userId, AnalysisProductType productType, string id, uint pageNumber = 1, uint pageCount = 25);

        Task<ResponseWrapper> GetAnalysisByBagIdAsync(Guid userId, AnalysisProductType productType, string bagId, uint pageNumber = 1, uint pageCount = 25);

        Task<ResponseWrapper> GetAnalysisByQueryAsync(Guid userId, AnalysisProductType productType, string query, uint pageNumber = 1, uint pageCount = 25);

        Task<ResponseWrapper> GetAnalysisInFenceAsync(Guid userId, AnalysisProductType productType, uint pageNumber = 1, uint pageCount = 25);

        Task<ResponseWrapper> GetStatisticsByAreaAsync(Guid userId, StatisticsProductType productType, string areaCode, uint pageNumber = 1, uint pageCount = 25);

        Task<ResponseWrapper> GetStatisticsInFenceAsync(Guid userId, StatisticsProductType productType, uint pageNumber = 1, uint pageCount = 25);
    }
}
