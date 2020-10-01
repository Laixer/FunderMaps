using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Service for retrieving products from our data store. This completes
    ///     each product.
    /// </summary>
    public class ProductService : IProductService
    {
        /// <summary>
        ///     <see cref="IAnalysisRepository"/>.
        /// </summary>
        protected readonly IAnalysisRepository _analysisRepository;

        /// <summary>
        ///     <see cref="IStatisticsRepository"/>.
        /// </summary>
        protected readonly IStatisticsRepository _statisticsRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProductService(IAnalysisRepository analysisRepository, IStatisticsRepository statisticsRepository)
        {
            _analysisRepository = analysisRepository ?? throw new ArgumentNullException(nameof(analysisRepository));
            _statisticsRepository = statisticsRepository ?? throw new ArgumentNullException(nameof(statisticsRepository));
        }

        /// <summary>
        ///     Gets a single analysis based on an external id.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="externalId">External building id.</param>
        /// <param name="externalDataSource">External data source.</param>
        public virtual async ValueTask<AnalysisProduct> GetAnalysisByExternalIdAsync(
            Guid userId,
            AnalysisProductType productType,
            string externalId,
            ExternalDataSource externalDataSource)
        {
            userId.ThrowIfNullOrEmpty();
            externalId.ThrowIfNullOrEmpty();

            var product = await _analysisRepository.GetByExternalIdAsync(userId, externalId, externalDataSource);

            await ProcessAnalysisAsync(userId, productType, product);
            return product;
        }

        /// <summary>
        ///     Gets a single analysis based on an internal id.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="id">Internal building id.</param>
        public virtual async ValueTask<AnalysisProduct> GetAnalysisByIdAsync(
            Guid userId,
            AnalysisProductType productType,
            string id)
        {
            id.ThrowIfNullOrEmpty();

            // FUTURE: This is a temporary fix, need another call?
            // Get analysis product.
            var product = userId != Guid.Empty
                ? await _analysisRepository.GetByIdInFenceAsync(userId, id)
                : await _analysisRepository.GetByIdAsync(id);

            await ProcessAnalysisAsync(userId, productType, product);
            return product;
        }

        /// <summary>
        ///     Gets an analysis collection based on a query string.
        /// </summary>
        /// <remarks>
        ///     All items outside the geofence get removed from the result set.
        /// </remarks>
        /// <param name="userId">Internal user id.</param>
        /// <param name="query">Query search string.</param>
        public virtual async IAsyncEnumerable<AnalysisProduct> GetAnalysisByQueryAsync(
            Guid userId,
            AnalysisProductType productType,
            string query,
            INavigation navigation)
        {
            userId.ThrowIfNullOrEmpty();
            query.ThrowIfNullOrEmpty();

            navigation.Validate();

            await foreach (var product in _analysisRepository.GetBySearchQueryAsync(userId, query, navigation))
            {
                await ProcessAnalysisAsync(userId, productType, product);
                yield return product;
            }
        }

        /// <summary>
        ///     Scrapped for now.
        /// </summary>
        /// <param name="userId">Internal user id</param>
        public virtual IAsyncEnumerable<AnalysisProduct> GetAnalysisInFenceAsync(
            Guid userId,
            AnalysisProductType productType,
            INavigation navigation)
        {
            throw new NotImplementedException();
        }

        /// FUTURE: Should not have userId
        /// <summary>
        ///     Gets statistics for a given area code.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="neighborhoodCode"/> can be a district or municipality
        ///     based on the <paramref name="productType"/>.
        /// </remarks>
        /// <param name="userId">Internal user id.</param>
        /// <param name="neighborhoodCode">Neighborhood code.</param>
        public virtual async ValueTask<StatisticsProduct> GetStatisticsByNeighborhoodAsync(
            Guid userId,
            StatisticsProductType productType,
            string neighborhoodCode)
        {
            neighborhoodCode.ThrowIfNullOrEmpty();

            var product = new StatisticsProduct();

            switch (productType)
            {
                case StatisticsProductType.FoundationRatio:
                    product.FoundationTypeDistribution = await _statisticsRepository.GetFoundationTypeDistributionByExternalIdAsync(userId, neighborhoodCode);
                    break;
                case StatisticsProductType.ConstructionYears:
                    product.ConstructionYearDistribution = await _statisticsRepository.GetConstructionYearDistributionByExternalIdAsync(userId, neighborhoodCode);
                    break;
                case StatisticsProductType.FoundationRisk:
                    product.FoundationRiskDistribution = await _statisticsRepository.GetFoundationRiskDistributionByExternalIdAsync(userId, neighborhoodCode);
                    break;
                case StatisticsProductType.DataCollected:
                    product.DataCollectedPercentage = await _statisticsRepository.GetDataCollectedPercentageByExternalIdAsync(userId, neighborhoodCode);
                    break;
                case StatisticsProductType.BuildingsRestored:
                    product.TotalBuildingRestoredCount = await _statisticsRepository.GetTotalBuildingRestoredCountByExternalIdAsync(userId, neighborhoodCode);
                    break;
                case StatisticsProductType.Incidents:
                    product.TotalIncidentCount = await _statisticsRepository.GetTotalIncidentCountByExternalIdAsync(userId, neighborhoodCode);
                    break;
                case StatisticsProductType.Reports:
                    product.TotalReportCount = await _statisticsRepository.GetTotalReportCountByExternalIdAsync(userId, neighborhoodCode);
                    break;
                default:
                    throw new InvalidOperationException(nameof(productType));
            }

            return product;
        }

        /// <summary>
        ///     Scrapped for now.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        public virtual ValueTask<StatisticsProduct> GetStatisticsInFenceAsync(
            Guid userId,
            StatisticsProductType productType,
            INavigation navigation)
        {
            throw new NotImplementedException();
        }

        // FUTURE: Parallel
        /// <summary>
        ///     This function does the following:
        ///     <list type="bullet">
        ///         <item>Fill all statistic fields if required</item>
        ///         <item>Process user tracking</item>
        ///         <item>Return the complete result</item>
        ///     </list>
        /// </summary>
        /// <remarks>
        ///     If a statistic addon property should not be populated, it will
        ///     remain as it was (null).
        /// </remarks>
        private async Task ProcessAnalysisAsync(
            Guid userId,
            AnalysisProductType productType,
            AnalysisProduct product)
        {
            await ProcessAnalysisFoundationTypeDistributionAsync(userId, productType, product);
            await ProcessAnalysisConstructionYearDistributionAsync(userId, productType, product);
            await ProcessAnalysisFoundationRiskDistributionAsync(userId, productType, product);
            await ProcessAnalysisDataCollectedPercentageAsync(userId, productType, product);
            await ProcessAnalysisTotalBuildingsRestoredCountAsync(userId, productType, product);
            await ProcessAnalysisTotalIncidentCountAsync(userId, productType, product);
            await ProcessAnalysisTotalReportCountAsync(userId, productType, product);
        }

        #region Process statistic part of analysis

        private async Task ProcessAnalysisFoundationTypeDistributionAsync(Guid userId, AnalysisProductType productType, AnalysisProduct product, CancellationToken token = default)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.FoundationTypeDistribution = await _statisticsRepository.GetFoundationTypeDistributionByIdAsync(userId, product.NeighborhoodId);
                    break;
            };
        }

        private async Task ProcessAnalysisConstructionYearDistributionAsync(Guid userId, AnalysisProductType productType, AnalysisProduct product, CancellationToken token = default)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.ConstructionYearDistribution = await _statisticsRepository.GetConstructionYearDistributionByIdAsync(userId, product.NeighborhoodId);
                    break;
            };
        }

        private async Task ProcessAnalysisFoundationRiskDistributionAsync(Guid userId, AnalysisProductType productType, AnalysisProduct product, CancellationToken token = default)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.FoundationRiskDistribution = await _statisticsRepository.GetFoundationRiskDistributionByIdAsync(userId, product.NeighborhoodId);
                    break;
            };
        }

        private async Task ProcessAnalysisDataCollectedPercentageAsync(Guid userId, AnalysisProductType productType, AnalysisProduct product, CancellationToken token = default)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.DataCollectedPercentage = await _statisticsRepository.GetDataCollectedPercentageByIdAsync(userId, product.NeighborhoodId);
                    break;
            };
        }

        private async Task ProcessAnalysisTotalBuildingsRestoredCountAsync(Guid userId, AnalysisProductType productType, AnalysisProduct product, CancellationToken token = default)
        {
            switch (productType)
            {
                case AnalysisProductType.Costs:
                case AnalysisProductType.Complete:
                    product.TotalBuildingRestoredCount = await _statisticsRepository.GetTotalBuildingRestoredCountByIdAsync(userId, product.NeighborhoodId);
                    break;
            };
        }

        private async Task ProcessAnalysisTotalIncidentCountAsync(Guid userId, AnalysisProductType productType, AnalysisProduct product, CancellationToken token = default)
        {
            switch (productType)
            {
                case AnalysisProductType.Costs:
                case AnalysisProductType.Complete:
                    product.TotalIncidentCount = await _statisticsRepository.GetTotalIncidentCountByIdAsync(userId, product.NeighborhoodId);
                    break;
            };
        }

        private async Task ProcessAnalysisTotalReportCountAsync(Guid userId, AnalysisProductType productType, AnalysisProduct product, CancellationToken token = default)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.TotalReportCount = await _statisticsRepository.GetTotalReportCountByIdAsync(userId, product.NeighborhoodId);
                    break;
            };
        }

        #endregion
    }
}
