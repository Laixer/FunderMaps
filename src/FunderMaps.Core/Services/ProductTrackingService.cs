using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    // FUTURE: Rename to TelemetryProductService
    /// <summary>
    ///     Retrieves products and tracks user behaviour.
    /// </summary>
    public class ProductTrackingService : ProductService
    {
        private readonly ITrackingRepository _trackingRepository;

        /// <summary>
        ///     Create new instance and invoke base.
        /// </summary>
        public ProductTrackingService(
            IAnalysisRepository analysisRepository,
            IStatisticsRepository statisticsRepository,
            ITrackingRepository trackingRepository)
            : base(analysisRepository, statisticsRepository)
            => _trackingRepository = trackingRepository ?? throw new ArgumentNullException(nameof(trackingRepository));

        /// <summary>
        ///     Gets a single analysis based on an external id. Also track product
        ///     usage in the data store.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="externalId">External building id.</param>
        /// <param name="externalSource">External data source.</param>
        public override async ValueTask<AnalysisProduct> GetAnalysisByExternalIdAsync(
            Guid userId,
            AnalysisProductType productType,
            string externalId)
        {
            externalId.ThrowIfNullOrEmpty();

            var result = await base.GetAnalysisByExternalIdAsync(userId, productType, externalId);

            await _trackingRepository.ProcessAnalysisUsageAsync(userId, productType, 1U);

            return result;
        }

        /// <summary>
        ///     Gets a single analysis based on an external id. Also track product
        ///     usage in the data store.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="externalId">External address id.</param>
        /// <param name="externalSource">External data source.</param>
        public override async ValueTask<AnalysisProduct> GetAnalysisByAddressExternalIdAsync(
            Guid userId,
            AnalysisProductType productType,
            string externalId)
        {
            externalId.ThrowIfNullOrEmpty();

            var result = await base.GetAnalysisByAddressExternalIdAsync(userId, productType, externalId);

            await _trackingRepository.ProcessAnalysisUsageAsync(userId, productType, 1U);

            return result;
        }

        /// <summary>
        ///     Gets a single analysis based on an internal id. Also track product
        ///     usage in the data store.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="id">Internal building id.</param>
        public override async ValueTask<AnalysisProduct> GetAnalysisByIdAsync(
            Guid userId,
            AnalysisProductType productType,
            string id)
        {
            id.ThrowIfNullOrEmpty();

            try
            {
                return await base.GetAnalysisByIdAsync(userId, productType, id);
            }
            finally
            {
                await _trackingRepository.ProcessAnalysisUsageAsync(userId, productType, 1U);
            }
        }

        /// <summary>
        ///     Gets an analysis collection based on a query string. Also track 
        ///     product usage in the data store.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="query">Query search string.</param>
        public override async IAsyncEnumerable<AnalysisProduct> GetAnalysisByQueryAsync(
            Guid userId,
            AnalysisProductType productType,
            string query,
            INavigation navigation)
        {
            userId.ThrowIfNullOrEmpty();
            query.ThrowIfNullOrEmpty();

            navigation.Validate();

            uint itemCount = 0;

            try
            {
                await foreach (var product in base.GetAnalysisByQueryAsync(userId, productType, query, navigation))
                {
                    ++itemCount;
                    yield return product;
                }
            }
            finally
            {
                await _trackingRepository.ProcessAnalysisUsageAsync(userId, productType, itemCount);
            }
        }

        /// <summary>
        ///     Scrapped for now.
        /// </summary>
        /// <param name="userId">Internal user id</param>
        public override IAsyncEnumerable<AnalysisProduct> GetAnalysisInFenceAsync(
            Guid userId,
            AnalysisProductType productType,
            INavigation navigation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets statistics for a given area code. Also track product usage 
        ///     in the data store.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="neighborhoodCode"/> can be a district or municipality
        ///     based on the <paramref name="productType"/>.
        /// </remarks>
        /// <param name="userId">Internal user id.</param>
        /// <param name="neighborhoodCode">Neighborhood code.</param>
        public override async ValueTask<StatisticsProduct> GetStatisticsByNeighborhoodAsync(
            Guid userId,
            StatisticsProductType productType,
            string neighborhoodCode)
        {
            userId.ThrowIfNullOrEmpty();
            neighborhoodCode.ThrowIfNullOrEmpty();

            try
            {
                return await base.GetStatisticsByNeighborhoodAsync(userId, productType, neighborhoodCode);
            }
            finally
            {
                await _trackingRepository.ProcessStatisticsUsageAsync(userId, productType, 1);
            }
        }

        /// <summary>
        ///     Scrapped for now.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        public override async ValueTask<StatisticsProduct> GetStatisticsInFenceAsync(
            Guid userId,
            StatisticsProductType productType,
            INavigation navigation)
        {
            try
            {
                return await base.GetStatisticsInFenceAsync(userId, productType, navigation);
            }
            finally
            {
                await _trackingRepository.ProcessStatisticsUsageAsync(userId, productType, 1);
            }
        }
    }
}
