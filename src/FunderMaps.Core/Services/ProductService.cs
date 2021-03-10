using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        private async Task<StatisticsProduct> GetStatisticsByIdAsync(string id)
            => new()
            {
                FoundationTypeDistribution = await _statisticsRepository.GetFoundationTypeDistributionByIdAsync(id),
                ConstructionYearDistribution = await _statisticsRepository.GetConstructionYearDistributionByIdAsync(id),
                DataCollectedPercentage = await _statisticsRepository.GetDataCollectedPercentageByIdAsync(id),
                FoundationRiskDistribution = await _statisticsRepository.GetFoundationRiskDistributionByIdAsync(id),
                TotalBuildingRestoredCount = await _statisticsRepository.GetTotalBuildingRestoredCountByIdAsync(id),
                TotalIncidentCount = await _statisticsRepository.GetTotalIncidentCountByIdAsync(id),
                MunicipalityIncidentCount = await _statisticsRepository.GetMunicipalityIncidentCountByIdAsync(id),
                TotalReportCount = await _statisticsRepository.GetTotalReportCountByIdAsync(id),
                MunicipalityReportCount = await _statisticsRepository.GetMunicipalityReportCountByIdAsync(id),
            };

        private async Task<StatisticsProduct> GetStatisticsByExternalIdAsync(string id)
            => new()
            {
                FoundationTypeDistribution = await _statisticsRepository.GetFoundationTypeDistributionByExternalIdAsync(id),
                ConstructionYearDistribution = await _statisticsRepository.GetConstructionYearDistributionByExternalIdAsync(id),
                DataCollectedPercentage = await _statisticsRepository.GetDataCollectedPercentageByExternalIdAsync(id),
                FoundationRiskDistribution = await _statisticsRepository.GetFoundationRiskDistributionByExternalIdAsync(id),
                TotalBuildingRestoredCount = await _statisticsRepository.GetTotalBuildingRestoredCountByExternalIdAsync(id),
                TotalIncidentCount = await _statisticsRepository.GetTotalIncidentCountByExternalIdAsync(id),
                MunicipalityIncidentCount = await _statisticsRepository.GetMunicipalityIncidentCountByExternalIdAsync(id),
                TotalReportCount = await _statisticsRepository.GetTotalReportCountByExternalIdAsync(id),
                MunicipalityReportCount = await _statisticsRepository.GetMunicipalityReportCountByExternalIdAsync(id),
            };

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
                _ => throw new InvalidIdentifierException(),
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
                        product.Statistics = await GetStatisticsByIdAsync(product.NeighborhoodId);
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
                GeocoderDatasource.FunderMaps => AsyncEnumerableHelper.AsEnumerable(await GetStatisticsByIdAsync(input)),
                GeocoderDatasource.NlCbsNeighborhood => AsyncEnumerableHelper.AsEnumerable(await GetStatisticsByExternalIdAsync(input)),
                _ => throw new InvalidIdentifierException(),
            })
            {
                yield return product;
            }
        }
    }
}
