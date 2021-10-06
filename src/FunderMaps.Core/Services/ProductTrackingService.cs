using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;

namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Retrieves products and tracks user behaviour.
    /// </summary>
    public class ProductTrackingService : ProductService
    {
        private const string statisticsProductName = "statistics";

        private readonly ITelemetryRepository _trackingRepository;

        /// <summary>
        ///     Create new instance and invoke base.
        /// </summary>
        public ProductTrackingService(
            IAnalysisRepository analysisRepository,
            IStatisticsRepository statisticsRepository,
            ITelemetryRepository trackingRepository,
            IGeocoderParser geocoderParser)
            : base(analysisRepository, statisticsRepository, geocoderParser)
            => _trackingRepository = trackingRepository ?? throw new ArgumentNullException(nameof(trackingRepository));

        /// <summary>
        ///     Get an analysis product and log product hit.
        /// </summary>
        /// <param name="productType">Product type.</param>
        /// <param name="input">Input query.</param>
        public override async IAsyncEnumerable<AnalysisProduct> GetAnalysisAsync(AnalysisProductType productType, string input)
        {
            int itemCount = 0;

            try
            {
                await foreach (var product in base.GetAnalysisAsync(productType, input))
                {
                    ++itemCount;
                    yield return product;
                }
            }
            finally
            {
                await _trackingRepository.ProductHitAsync(productType.ToString(), itemCount);
            }
        }

        /// <summary>
        ///     Get statistics per region and log product hit.
        /// </summary>
        /// <param name="input">Input query.</param>
        public override async IAsyncEnumerable<StatisticsProduct> GetStatisticsAsync(string input)
        {
            int itemCount = 0;

            try
            {
                await foreach (var product in base.GetStatisticsAsync(input))
                {
                    ++itemCount;
                    yield return product;
                }
            }
            finally
            {
                await _trackingRepository.ProductHitAsync(statisticsProductName);
            }
        }
    }
}
