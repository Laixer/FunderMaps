using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Abstractions.Services;
using System.Collections.Generic;

namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Service to the analysis products.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IAnalysisRepository _analysisRepository;
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IGeocoderParser _geocoderParser;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProductService(
            IAnalysisRepository analysisRepository,
            IStatisticsRepository statisticsRepository,
            IGeocoderParser geocoderParser)
        {
            _analysisRepository = analysisRepository;
            _statisticsRepository = statisticsRepository;
            _geocoderParser = geocoderParser;
        }

        /// <summary>
        ///     Get an analysis product.
        /// </summary>
        /// <param name="productType">Product type.</param>
        /// <param name="input">Input query.</param>
        public virtual async IAsyncEnumerable<AnalysisProduct> GetAnalysisAsync(AnalysisProductType productType, string input)
        {
            await foreach (var product in _geocoderParser.FromIdentifier(input, out string id) switch
            {
                GeocoderDatasource.FunderMaps => AsyncEnumerableHelper.AsEnumerable(await _analysisRepository.GetByIdAsync(id)),
                GeocoderDatasource.NlBagBuilding => AsyncEnumerableHelper.AsEnumerable(await _analysisRepository.GetByExternalIdAsync(id)),
                GeocoderDatasource.NlBagAddress => AsyncEnumerableHelper.AsEnumerable(await _analysisRepository.GetByAddressExternalIdAsync(id)),
                _ => throw new System.InvalidOperationException(), // TODO
            })
            {
                // FUTURE: Retrieve the description from a service.
                product.DescriptionDrystand = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
                product.DescriptionDewateringDepth = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
                product.DescriptionBioInfection = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
                product.DescriptionRestorationCosts = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";

                switch (productType)
                {
                    case AnalysisProductType.RiskPlus:
                    case AnalysisProductType.Complete:
                        product.Statistics = await _statisticsRepository.GetStatisticsProductByIdAsync(product.NeighborhoodId);
                        break;
                };

                yield return product;
            }
        }

        /// <summary>
        ///     Get statistics per region.
        /// </summary>
        /// <param name="input">Input query.</param>
        public virtual async IAsyncEnumerable<StatisticsProduct> GetStatisticsAsync(string input)
        {
            await foreach (var product in _geocoderParser.FromIdentifier(input) switch
            {
                GeocoderDatasource.FunderMaps => AsyncEnumerableHelper.AsEnumerable(await _statisticsRepository.GetStatisticsProductByIdAsync(input)),
                GeocoderDatasource.NlCbsNeighborhood => AsyncEnumerableHelper.AsEnumerable(await _statisticsRepository.GetStatisticsProductByExternalIdAsync(input)),
                _ => throw new System.InvalidOperationException(), // TODO
            })
            {
                yield return product;
            }
        }
    }
}
