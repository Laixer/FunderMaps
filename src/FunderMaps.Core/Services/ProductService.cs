﻿using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Service for retrieving products from our data store.
    /// </summary>
    public sealed class ProductService : IProductService
    {
        private readonly IUserTrackingService _userTrackingService;
        private readonly IAnalysisRepository _analysisRepository;
        private readonly IStatisticsRepository _statisticsRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProductService(IUserTrackingService userTrackingService,
            IAnalysisRepository analysisRepository,
            IStatisticsRepository statisticsRepository)
        {
            _userTrackingService = userTrackingService ?? throw new ArgumentNullException(nameof(userTrackingService));
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
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        public async Task<AnalysisProduct> GetAnalysisByExternalIdAsync(Guid userId, AnalysisProductType productType, string externalId, ExternalDataSource externalDataSource, INavigation navigation, CancellationToken token)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            externalId.ThrowIfNullOrEmpty();
            navigation.Validate();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Get analysis product.
            var product = await _analysisRepository.GetByExternalIdAsync(userId, externalId, externalDataSource, token);

            // Process and mark.
            product = await ProcessAnalysisAsync(userId, productType, product, token);
            await _userTrackingService.ProcessSingleAnalysisRequest(userId, productType, token);
            return product;
        }

        /// <summary>
        ///     Gets a single analysis based on an internal id.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="id">Internal building id.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        public async Task<AnalysisProduct> GetAnalysisByIdAsync(Guid userId, AnalysisProductType productType, string id, INavigation navigation, CancellationToken token)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            id.ThrowIfNullOrEmpty();
            navigation.Validate();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Get analysis product.
            var product = await _analysisRepository.GetByIdAsync(userId, id, token);

            // Process and mark.
            product = await ProcessAnalysisAsync(userId, productType, product, token);
            await _userTrackingService.ProcessSingleAnalysisRequest(userId, productType, token);
            return product;
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
        public async Task<IEnumerable<AnalysisProduct>> GetAnalysisByQueryAsync(Guid userId, AnalysisProductType productType, string query, INavigation navigation, CancellationToken token)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            query.ThrowIfNullOrEmpty();
            navigation.Validate();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Get analysis product.
            var products = await _analysisRepository.GetByQueryAsync(userId, query, navigation, token);

            // Process and mark.
            // FUTURE Clean up, make async foreach or similar.
            var result = new Collection<AnalysisProduct>();
            foreach (var product in products)
            {
                result.Add(await ProcessAnalysisAsync(userId, productType, product, token));
            }
            await _userTrackingService.ProcessMultipleAnalysisRequest(userId, productType, (uint) result.Count, token);
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
        public Task<IEnumerable<AnalysisProduct>> GetAnalysisInFenceAsync(Guid userId, AnalysisProductType productType, INavigation navigation, CancellationToken token) => throw new NotImplementedException();

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
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="StatisticsProduct"/></returns>
        public async Task<StatisticsProduct> GetStatisticsByAreaAsync(Guid userId, StatisticsProductType productType, string neighborhoodCode, INavigation navigation, CancellationToken token)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            neighborhoodCode.ThrowIfNullOrEmpty();
            navigation.Validate();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Create and assign.
            var product = new StatisticsProduct();

            // Get statistics operation.
            switch (productType)
            {
                case StatisticsProductType.FoundationRatio:
                    product.FoundationTypeDistribution = await _statisticsRepository.GetFoundationTypeDistributionByExternalIdAsync(neighborhoodCode, token);
                    break;
                case StatisticsProductType.ConstructionYears:
                    product.ConstructionYearDistribution = await _statisticsRepository.GetConstructionYearDistributionByExternalIdAsync(neighborhoodCode, token);
                    break;
                case StatisticsProductType.FoundationRisk:
                    product.FoundationRiskDistribution = await _statisticsRepository.GetFoundationRiskDistributionByExternalIdAsync(neighborhoodCode, token);
                    break;
                case StatisticsProductType.DataCollected:
                    product.DataCollectedPercentage = await _statisticsRepository.GetDataCollectedPercentageByExternalIdAsync(neighborhoodCode, token);
                    break;
                case StatisticsProductType.BuildingsRestored:
                    product.TotalBuildingRestoredCount = await _statisticsRepository.GetTotalBuildingRestoredCountByExternalIdAsync(neighborhoodCode, token);
                    break;
                case StatisticsProductType.Incidents:
                    product.TotalIncidentCount = await _statisticsRepository.GetTotalIncidentCountByExternalIdAsync(neighborhoodCode, token);
                    break;
                case StatisticsProductType.Reports:
                    product.TotalReportCount = await _statisticsRepository.GetTotalReportCountByExternalIdAsync(neighborhoodCode, token);
                    break;
                default:
                    throw new InvalidOperationException(nameof(productType));
            }

            // Track and return.
            await _userTrackingService.ProcessStatisticsRequestAsync(userId, productType, token);
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
        public Task<StatisticsProduct> GetStatisticsInFenceAsync(Guid userId, StatisticsProductType productType, INavigation navigation, CancellationToken token) => throw new NotImplementedException();

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
        /// <param name="userId">Internal user id</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="product"><see cref="AnalysisProduct"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        private async Task<AnalysisProduct> ProcessAnalysisAsync(Guid userId, AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            await ProcessAnalysisFoundationTypeDistributionAsync(productType, product, token);
            await ProcessAnalysisConstructionYearDistributionAsync(productType, product, token);
            await ProcessAnalysisFoundationRiskDistributionAsync(productType, product, token);
            await ProcessAnalysisDataCollectedPercentageAsync(productType, product, token);
            await ProcessAnalysisTotalBuildingsRestoredCountAsync(productType, product, token);
            await ProcessAnalysisTotalIncidentCountAsync(productType, product, token);
            await ProcessAnalysisTotalReportCountAsync(productType, product, token);

            return product;
        }

        #region Process statistic part of analysis

        private async Task ProcessAnalysisFoundationTypeDistributionAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.FoundationTypeDistribution = await _statisticsRepository.GetFoundationTypeDistributionByIdAsync(product.NeighborhoodId, token);
                    break;
            };
        }

        private async Task ProcessAnalysisConstructionYearDistributionAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.ConstructionYearDistribution = await _statisticsRepository.GetConstructionYearDistributionByIdAsync(product.NeighborhoodId, token);
                    break;
            };
        }

        private async Task ProcessAnalysisFoundationRiskDistributionAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.FoundationRiskDistribution = await _statisticsRepository.GetFoundationRiskDistributionByIdAsync(product.NeighborhoodId, token);
                    break;
            };
        }

        private async Task ProcessAnalysisDataCollectedPercentageAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.DataCollectedPercentage = await _statisticsRepository.GetDataCollectedPercentageByIdAsync(product.NeighborhoodId, token);
                    break;
            };
        }

        private async Task ProcessAnalysisTotalBuildingsRestoredCountAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.Costs:
                case AnalysisProductType.Complete:
                    product.TotalBuildingRestoredCount = await _statisticsRepository.GetTotalBuildingRestoredCountByIdAsync(product.NeighborhoodId, token);
                    break;
            };
        }

        private async Task ProcessAnalysisTotalIncidentCountAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.Costs:
                case AnalysisProductType.Complete:
                    product.TotalIncidentCount = await _statisticsRepository.GetTotalIncidentCountByIdAsync(product.NeighborhoodId, token);
                    break;
            };
        }

        private async Task ProcessAnalysisTotalReportCountAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.TotalReportCount = await _statisticsRepository.GetTotalReportCountByIdAsync(product.NeighborhoodId, token);
                    break;
            };
        }

        #endregion

    }
}