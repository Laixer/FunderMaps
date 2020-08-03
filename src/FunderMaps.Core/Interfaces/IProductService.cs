using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Abstractions.Services
{
    /// <summary>
    /// Contract for retrieving products from the data store.
    /// </summary>
    public interface IProductService
    {
        Task<AnalysisProduct> GetAnalysisByIdAsync(AnalysisProductType productType, string id, uint pageNumber = 1, uint pageCount = 25);

        Task<AnalysisProduct> GetAnalysisByExternalIdAsync(AnalysisProductType productType, string externalId, ExternalDataSource externalSource, uint pageNumber = 1, uint pageCount = 25);

        Task<IEnumerable<AnalysisProduct>> GetAnalysisByQueryAsync(AnalysisProductType productType, string query, uint pageNumber = 1, uint pageCount = 25);

        Task<IEnumerable<AnalysisProduct>> GetAnalysisInFenceAsync(AnalysisProductType productType, uint pageNumber = 1, uint pageCount = 25);

        Task<StatisticsProduct> GetStatisticsByAreaAsync(StatisticsProductType productType, uint pageNumber = 1, uint pageCount = 25);

        Task<StatisticsProduct> GetStatisticsInFenceAsync(StatisticsProductType productType, uint pageNumber = 1, uint pageCount = 25);
    }
}
