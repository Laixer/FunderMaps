using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.Utility;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Services
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
            var product = await _analysisRepository.GetByExternalIdAsync(userId, externalId, externalDataSource, token).ConfigureAwait(false);

            // Pass on for further processing
            return product == null ? null : await ProcessAnalysisAsync(userId, productType, product, token).ConfigureAwait(false);
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
            var product = await _analysisRepository.GetByIdAsync(userId, id, token).ConfigureAwait(false);

            // Pass on for further processing
            return product == null ? null : await ProcessAnalysisAsync(userId, productType, product, token).ConfigureAwait(false);
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
            var products = await _analysisRepository.GetByQueryAsync(userId, query, token).ConfigureAwait(false);

            // Pass on for further processing
            // TODO Clean up, make async foreach or similar.
            var result = new Collection<AnalysisProduct>();
            foreach (var product in products)
            {
                result.Add(await ProcessAnalysisAsync(userId, productType, product, token).ConfigureAwait(false));
            }
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
        ///     The <paramref name="areaCode"/> can be a district or municipality
        ///     based on the <paramref name="productType"/>.
        /// </remarks>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="StatisticsProductType"/></param>
        /// <param name="areaCode">Area code.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="StatisticsProduct"/></returns>
        public async Task<StatisticsProduct> GetStatisticsByAreaAsync(Guid userId, StatisticsProductType productType, string areaCode, INavigation navigation, CancellationToken token)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            areaCode.ThrowIfNullOrEmpty();
            navigation.Validate();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Get statistics.
            var product = new StatisticsProduct();
            switch (productType)
            {
                case StatisticsProductType.FoundationRatio:
                    product.FoundationTypeDistribution = await _statisticsRepository.GetFoundationTypeDistributionAsync(areaCode, token).ConfigureAwait(false);
                    break;
                case StatisticsProductType.ConstructionYears:
                    product.ConstructionYearDistribution = await _statisticsRepository.GetConstructionYearDistributionAsync(areaCode, token).ConfigureAwait(false);
                    break;
                case StatisticsProductType.FoundationRisk:
                    product.FoundationRiskDistribution = await _statisticsRepository.GetFoundationRiskDistributionAsync(areaCode, token).ConfigureAwait(false);
                    break;
                case StatisticsProductType.DataCollected:
                    product.DataCollectedPercentage = await _statisticsRepository.GetDataCollectedPercentageAsync(areaCode, token).ConfigureAwait(false);
                    break;
                case StatisticsProductType.BuildingsRestored:
                    product.TotalBuildingRestored = await _statisticsRepository.GetTotalBuildingRestoredCountAsync(areaCode, token).ConfigureAwait(false);
                    break;
                case StatisticsProductType.Incidents:
                    product.TotalIncidents = await _statisticsRepository.GetTotalIncidentCountAsync(areaCode, token).ConfigureAwait(false);
                    break;
                case StatisticsProductType.Reports:
                    product.TotalReportCount = await _statisticsRepository.GetTotalReportCountAsync(areaCode, token).ConfigureAwait(false);
                    break;
                default:
                    throw new InvalidOperationException(nameof(productType));
            }

            // Track user request.
            await _userTrackingService.ProcessStatisticsRequestAsync(userId, productType).ConfigureAwait(false);

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
            // The product can never be null.
            if (product == null) { throw new ArgumentNullException(nameof(product)); }

            // Process analysis statistics part.
            await ProcessAnalysisFoundationTypeDistributionAsync(productType, product, token).ConfigureAwait(false);
            await ProcessAnalysisConstructionYearDistributionAsync(productType, product, token).ConfigureAwait(false);
            await ProcessAnalysisFoundationRiskDistributionAsync(productType, product, token).ConfigureAwait(false);
            await ProcessAnalysisDataCollectedPercentageAsync(productType, product, token).ConfigureAwait(false);
            await ProcessAnalysisTotalBuildingsRestoredCountAsync(productType, product, token).ConfigureAwait(false);
            await ProcessAnalysisTotalIncidentCountAsync(productType, product, token).ConfigureAwait(false);
            await ProcessAnalysisTotalReportCountAsync(productType, product, token).ConfigureAwait(false);

            // Process user product usage.
            await _userTrackingService.ProcessAnalysisRequest(userId, productType).ConfigureAwait(false);

            // Return result.
            return product;
        }

        #region Process statistic part of analysis

        private async Task ProcessAnalysisFoundationTypeDistributionAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.FoundationTypeDistribution = await _statisticsRepository.GetFoundationTypeDistributionAsync(product.Id, token).ConfigureAwait(false);
                    break;
            };
        }

        private async Task ProcessAnalysisConstructionYearDistributionAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.ConstructionYearDistribution = await _statisticsRepository.GetConstructionYearDistributionAsync(product.Id, token).ConfigureAwait(false);
                    break;
            };
        }

        private async Task ProcessAnalysisFoundationRiskDistributionAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.FoundationRiskDistribution = await _statisticsRepository.GetFoundationRiskDistributionAsync(product.Id, token).ConfigureAwait(false);
                    break;
            };
        }

        private async Task ProcessAnalysisDataCollectedPercentageAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.DataCollectedPercentage = await _statisticsRepository.GetDataCollectedPercentageAsync(product.Id, token).ConfigureAwait(false);
                    break;
            };
        }

        private async Task ProcessAnalysisTotalBuildingsRestoredCountAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.Costs:
                case AnalysisProductType.Complete:
                    product.TotalBuildingRestoredCount = await _statisticsRepository.GetTotalBuildingRestoredCountAsync(product.Id, token).ConfigureAwait(false);
                    break;
            };
        }

        private async Task ProcessAnalysisTotalIncidentCountAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.Costs:
                case AnalysisProductType.Complete:
                    product.TotalIncidentCount = await _statisticsRepository.GetTotalIncidentCountAsync(product.Id, token).ConfigureAwait(false);
                    break;
            };
        }

        private async Task ProcessAnalysisTotalReportCountAsync(AnalysisProductType productType, AnalysisProduct product, CancellationToken token)
        {
            switch (productType)
            {
                case AnalysisProductType.FoundationPlus:
                case AnalysisProductType.Complete:
                    product.TotalReportCount = await _statisticsRepository.GetTotalReportCountAsync(product.Id, token).ConfigureAwait(false);
                    break;
            };
        }

        #endregion

    }
}
