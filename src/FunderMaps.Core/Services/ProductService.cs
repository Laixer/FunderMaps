using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Abstractions.Services;
using System;
using System.Collections.Concurrent;
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
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        public virtual async Task<AnalysisProduct> GetAnalysisByExternalIdAsync(
            Guid userId,
            AnalysisProductType productType,
            string externalId,
            ExternalDataSource externalDataSource,
            CancellationToken token = default)
        {
            userId.ThrowIfNullOrEmpty();
            externalId.ThrowIfNullOrEmpty();

            // Get analysis product.
            var product = await _analysisRepository.GetByExternalIdAsync(userId, externalId, externalDataSource);

            // Process and return.
            return await ProcessAnalysisAsync(userId, productType, product, token);
        }

        /// <summary>
        ///     Gets a single analysis based on an internal id.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="id">Internal building id.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        public virtual async Task<AnalysisProduct> GetAnalysisByIdAsync(
            Guid userId,
            AnalysisProductType productType,
            string id,
            CancellationToken token = default)
        {
            id.ThrowIfNullOrEmpty();

            // FUTURE: This is a temporary fix, need another call?
            // Get analysis product.
            var product = userId != Guid.Empty
                ? await _analysisRepository.GetByIdInFenceAsync(userId, id)
                : await _analysisRepository.GetByIdAsync(id);

            // Process and return.
            return await ProcessAnalysisAsync(userId, productType, product);
        }

        /// <summary>
        ///     Gets an analysis collection based on a query string.
        /// </summary>
        /// <remarks>
        ///     All items outside the geofence get removed from the result set.
        /// </remarks>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="query">Query search string.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="IEnumerable{AnalysisProduct}"/></returns>
        public virtual async Task<IEnumerable<AnalysisProduct>> GetAnalysisByQueryAsync(
            Guid userId,
            AnalysisProductType productType,
            string query,
            INavigation navigation,
            CancellationToken token = default)
        {
            userId.ThrowIfNullOrEmpty();
            query.ThrowIfNullOrEmpty();
            navigation.Validate();

            // Get analysis product.
            var products = await _analysisRepository.GetByQueryAsync(userId, query, navigation);

            // Process and return.
            // FUTURE Clean up, make async foreach or similar.
            var result = new ConcurrentBag<AnalysisProduct>();
            Parallel.ForEach(products, (product) =>
            {
                result.Add(Task.Run(() => ProcessAnalysisAsync(userId, productType, product)).Result);
            });

            return result;
        }

        /// <summary>
        ///     Scrapped for now.
        /// </summary>
        /// <param name="userId">Internal user id</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="IEnumerable{AnalysisProduct}"/></returns>
        public virtual Task<IEnumerable<AnalysisProduct>> GetAnalysisInFenceAsync(
            Guid userId,
            AnalysisProductType productType,
            INavigation navigation,
            CancellationToken token = default)
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
        /// <param name="productType"><see cref="StatisticsProductType"/></param>
        /// <param name="neighborhoodCode">Neighborhood code.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="StatisticsProduct"/></returns>
        public virtual async Task<StatisticsProduct> GetStatisticsByNeighborhoodAsync(
            Guid userId,
            StatisticsProductType productType,
            string neighborhoodCode,
            CancellationToken token = default)
        {
            neighborhoodCode.ThrowIfNullOrEmpty();

            // Create and assign.
            var product = new StatisticsProduct();

            // Get statistics operation.
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
        /// <param name="productType"><see cref="StatisticsProductType"/></param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="StatisticsProduct"/></returns>
        public virtual Task<StatisticsProduct> GetStatisticsInFenceAsync(
            Guid userId,
            StatisticsProductType productType,
            INavigation navigation,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

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
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="product"><see cref="AnalysisProduct"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        private async Task<AnalysisProduct> ProcessAnalysisAsync(
            Guid userId,
            AnalysisProductType productType,
            AnalysisProduct product,
            CancellationToken token = default)
        {
            await Task.WhenAll(new Task[]
            {
                ProcessAnalysisFoundationTypeDistributionAsync(userId, productType, product, token),
                ProcessAnalysisConstructionYearDistributionAsync(userId, productType, product, token),
                ProcessAnalysisFoundationRiskDistributionAsync(userId, productType, product, token),
                ProcessAnalysisDataCollectedPercentageAsync(userId, productType, product, token),
                ProcessAnalysisTotalBuildingsRestoredCountAsync(userId, productType, product, token),
                ProcessAnalysisTotalIncidentCountAsync(userId, productType, product, token),
                ProcessAnalysisTotalReportCountAsync(userId, productType, product, token),
            });

            return product;
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
