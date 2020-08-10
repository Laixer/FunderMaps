using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Abstractions.Services
{
    /// <summary>
    /// Contract for retrieving products from the data store.
    /// TODO Docs
    /// </summary>
    public interface IProductService
    {
        Task<AnalysisProduct> GetAnalysisByIdAsync(Guid userId, AnalysisProductType productType, string id, INavigation navigation);

        Task<AnalysisProduct> GetAnalysisByExternalIdAsync(Guid userId, AnalysisProductType productType, string externalId, ExternalDataSource externalSource, INavigation navigation);

        Task<IEnumerable<AnalysisProduct>> GetAnalysisByQueryAsync(Guid userId, AnalysisProductType productType, string query, INavigation navigation);

        Task<IEnumerable<AnalysisProduct>> GetAnalysisInFenceAsync(Guid userId, AnalysisProductType productType, INavigation navigation);

        Task<StatisticsProduct> GetStatisticsByAreaAsync(Guid userId, StatisticsProductType productType, INavigation navigation);

        Task<StatisticsProduct> GetStatisticsInFenceAsync(Guid userId, StatisticsProductType productType, INavigation navigation);
    }
}
