using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Abstractions.Services
{
    /// <summary>
    /// Contract for retrieving products from the data store.
    /// </summary>
    public interface IProductService
    {
        Task<AnalysisProduct> GetAnalysisByIdAsync(Guid userId, AnalysisProductType productType, string id, uint pageNumber = 1, uint pageCount = 25);

        Task<AnalysisProduct> GetAnalysisByExternalIdAsync(Guid userId, AnalysisProductType productType, string externalId, ExternalDataSource externalSource, uint pageNumber = 1, uint pageCount = 25);

        Task<IEnumerable<AnalysisProduct>> GetAnalysisByQueryAsync(Guid userId, AnalysisProductType productType, string query, uint pageNumber = 1, uint pageCount = 25);

        Task<IEnumerable<AnalysisProduct>> GetAnalysisInFenceAsync(Guid userId, AnalysisProductType productType, uint pageNumber = 1, uint pageCount = 25);

        Task<StatisticsProduct> GetStatisticsByAreaAsync(Guid userId, StatisticsProductType productType, uint pageNumber = 1, uint pageCount = 25);

        Task<StatisticsProduct> GetStatisticsInFenceAsync(Guid userId, StatisticsProductType productType, uint pageNumber = 1, uint pageCount = 25);
    }
}
