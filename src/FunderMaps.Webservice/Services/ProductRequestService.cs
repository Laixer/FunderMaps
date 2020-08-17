using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Extensions;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.InputModels;
using FunderMaps.Webservice.Mapping;
using FunderMaps.Webservice.ResponseModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Services
{
    /// <summary>
    ///     Handles product requests by calling the <see cref="IProductResultService"/>.
    /// </summary>
    public sealed class ProductRequestService : IProductRequestService
    {
        private readonly IProductResultService _productResultService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProductRequestService(IProductResultService productResultService) => _productResultService = productResultService ?? throw new ArgumentNullException(nameof(productResultService));

        /// <summary>
        ///     Processes an analysis request.
        /// </summary>
        /// <param name="userId">Internal user id</param>
        /// <param name="inputModel"><see cref="AnalysisInputModel"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="ResponseWrapper{AnalysisResponseModelBase}"/></returns>
        public async Task<ResponseWrapper> ProcessAnalysisRequestAsync(Guid userId, AnalysisInputModel inputModel, CancellationToken token)
        {
            userId.ThrowIfNullOrEmpty();
            if (inputModel == null) { throw new ArgumentNullException(nameof(inputModel)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Map product type.
            var product = ProductTypeMapper.MapAnalysis(inputModel.Product ?? throw new InvalidOperationException(nameof(inputModel.Product)));

            // Process according to specified parameters
            if (!string.IsNullOrEmpty(inputModel.Query))
            {
                return await _productResultService.GetAnalysisByQueryAsync(userId, product, inputModel.Query, inputModel.Navigation, token).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(inputModel.Id))
            {
                return await _productResultService.GetAnalysisByIdAsync(userId, product, inputModel.Id, inputModel.Navigation, token).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(inputModel.BagId))
            {
                return await _productResultService.GetAnalysisByBagIdAsync(userId, product, inputModel.BagId, inputModel.Navigation, token).ConfigureAwait(false);
            }

            // If we reach this point we can't process the request.
            throw new InvalidProductRequestException(nameof(inputModel.Product));
        }

        /// <summary>
        ///     Processes a statistics request.
        /// </summary>
        /// <param name="userId">Internal user id</param>
        /// <param name="inputModel"><see cref="StatisticsInputModel"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="ResponseWrapper{StatisticsResponseModelBase}"/></returns>
        public async Task<ResponseWrapper> ProcessStatisticsRequestAsync(Guid userId, StatisticsInputModel inputModel, CancellationToken token)
        {
            userId.ThrowIfNullOrEmpty();
            if (inputModel == null) { throw new ArgumentNullException(nameof(inputModel)); }
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Map product type.
            var product = ProductTypeMapper.MapStatistics(inputModel.Product ?? throw new InvalidOperationException(nameof(inputModel.Product)));

            // Process according to specified parameters
            if (!string.IsNullOrEmpty(inputModel.NeighborhoodCode))
            {
                return await _productResultService.GetStatisticsByAreaAsync(userId, product, inputModel.NeighborhoodCode, inputModel.Navigation, token).ConfigureAwait(false);
            }

            // If we reach this point we can't process the request.
            throw new InvalidProductRequestException(nameof(inputModel.Product));
        }
    }
}
