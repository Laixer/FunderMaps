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
        ///     Generate drystand description text.
        /// </summary>
        private string DescriptionDrystand(FoundationRisk? risk, double? drystand)
            => risk switch
            {
                var risk_low when (
                    risk_low == FoundationRisk.A ||
                    risk_low == FoundationRisk.B) &&
                    drystand is not null => $"Er komt doorgaans geen droogstand voor. Er is een veilige marge van {drystand}m dat het grondwater hoger staat dan het hoogstgelegen funderingshout. Hierdoor kan het funderingshout niet rotten door een tekort aan zuurstof.",

                var risk_medium when (
                    risk_medium == FoundationRisk.C ||
                    risk_medium == FoundationRisk.D) &&
                    drystand is not null => $"Er kan een droogstand van ca. {drystand}m van het hoogstgelegen funderingshout voorkomen maar deze bevindt zich doorgaans in een acceptabele marge. Bij wijzigende omstandigheden kan de situatie verergeren waardoor het funderingshout regelmatig droog kan komen te staan en het hout kan gaan rotten. Na jarenlange droogstand kan het draagvermogen van de fundering zijn aangetast waardoor het pand kan gaan deformeren. Indien dit in vergevorderd stadium is, zijn er scheuren in de gevel zichtbaar, klemmen ramen en deuren en vertoont het gevelaanzicht tekenen van verzakkingen.​",

                var risk_low when (
                    risk_low == FoundationRisk.E) &&
                    drystand is not null => $"Er kan een droogstand van ca. {drystand}m van het hoogstgelegen funderingshout voorkomen. Hierdoor kan het funderingshout regelmatig droog komen te staan waardoor het hout kan gaan rotten. Na jarenlange droogstand kan het draagvermogen van de fundering zijn aangetast waardoor het pand kan gaan deformeren. Indien dit in vergevorderd stadium is, zijn er scheuren in de gevel zichtbaar, klemmen ramen en deuren en vertoont het gevelaanzicht tekenen van verzakkingen.",

                _ => "Onbekend",
            };

        /// <summary>
        ///     Generate dewatering depth description text.
        /// </summary>
        private string DescriptionDewateringDepth(FoundationRisk? risk, double? dewateringDepth, string soil)
            => risk switch
            {
                var risk_low when (
                    risk_low == FoundationRisk.A ||
                    risk_low == FoundationRisk.B) &&
                    dewateringDepth is not null => $"De gemiddelde grondwaterlevelfluctuatie is {dewateringDepth}m. Bij deze waarden is de kans op verzakkingen van een fundering op staal bij de ondergrond die bestaat uit {soil} laag.​",

                var risk_medium when (
                    risk_medium == FoundationRisk.C ||
                    risk_medium == FoundationRisk.D) &&
                    dewateringDepth is not null => $"De gemiddelde grondwaterlevelfluctuatie is {dewateringDepth}m. Bij deze waarden is de kans op verzakkingen van een fundering op staal bij de ondergrond die bestaat uit {soil} gemiddeld.​",

                var risk_low when (
                    risk_low == FoundationRisk.E) &&
                    dewateringDepth is not null => $"De gemiddelde grondwaterlevelfluctuatie is {dewateringDepth}m. Bij deze waarden is de kans op verzakkingen van een fundering op staal bij de ondergrond die bestaat uit {soil} verhoogd.",

                _ => "Onbekend",
            };

        /// <summary>
        ///     Generate bioinfection description text.
        /// </summary>
        private string DescriptionBioInfection(FoundationRisk? risk)
            => risk switch
            {
                var risk_low when
                    risk_low == FoundationRisk.A ||
                    risk_low == FoundationRisk.B => "Toepassing van grenen palen is hoogstwaarschijnlijk niet toegepast bij dit adres gelet op de diepteligging van de draagkrachtige zandlaag. Deze ligt dieper dan 10 m onder het maaiveld. Grenen palen zijn zelden langer dan 10 m. Toepassing is daarmee onwaarschijnlijk.​",

                var risk_medium when
                    risk_medium == FoundationRisk.C ||
                    risk_medium == FoundationRisk.D => "Toepassing van grenen palen kan voorkomen bij dit adres gelet op de diepteligging van de ondiepe draagkrachtige zandlaag. Grenen palen zijn zelden langer dan 10 m. Grenen palen zijn gevoelig voor bacteriële aantasting waardoor het hout van buitenaf wordt aangetast over de gehele lengte van de paal. Het draagvermogen van de paal neemt daarmee af waardoor het pand gaat verzakken. Indien dit in vergevorderd stadium is, zijn er scheuren in de gevel zichtbaar, klemmen ramen en deuren en vertoont het gevelaanzicht tekenen van verzakkingen.​",

                FoundationRisk.E => "Grenen palen zijn hoogstwaarschijnlijk toegepast bij dit adres, gelet op de diepteligging van de ondiepe draagkrachtige zandlaag en het bouwjaar. Grenen palen zijn zelden langer dan 10 m. Grenen palen zijn gevoelig voor bacteriële aantasting waardoor het hout van buitenaf wordt aangetast over de gehele lengte van de paal. Het draagvermogen van de paal neemt daarmee af waardoor het pand gaat verzakken. Indien dit in vergevorderd stadium is, zijn er scheuren in de gevel zichtbaar, klemmen ramen en deuren en vertoont het gevelaanzicht tekenen van verzakkingen.",

                _ => "Onbekend",
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
                GeocoderDatasource.NlBagBerth => AsyncEnumerableHelper.AsEnumerable(await _analysisRepository.GetByExternalIdAsync(id)),
                GeocoderDatasource.NlBagPosting => AsyncEnumerableHelper.AsEnumerable(await _analysisRepository.GetByExternalIdAsync(id)),
                GeocoderDatasource.NlBagAddress => AsyncEnumerableHelper.AsEnumerable(await _analysisRepository.GetByAddressExternalIdAsync(id)),
                _ => throw new InvalidIdentifierException(),
            })
            {
                yield return product with
                {
                    DescriptionDrystand = DescriptionDrystand(product.DrystandRisk, product.Drystand),
                    DescriptionDewateringDepth = DescriptionDewateringDepth(product.DewateringDepthRisk, product.DewateringDepth, product.Soil),
                    DescriptionBioInfection = DescriptionBioInfection(product.BioInfectionRisk),

                    Statistics = productType switch
                    {
                        AnalysisProductType.RiskPlus => await GetStatisticsByIdAsync(product.NeighborhoodId),
                        AnalysisProductType.Complete => await GetStatisticsByIdAsync(product.NeighborhoodId),
                        _ => null,
                    },
                };
            }
        }

        // TODO: Remove the foreach
        /// <summary>
        ///     Get an analysis product v2.
        /// </summary>
        /// <param name="input">Input query.</param>
        public virtual async IAsyncEnumerable<AnalysisProduct2> GetAnalysis2Async(string input)
        {
            await foreach (var product in _geocoderParser.FromIdentifier(input, out string id) switch
            {
                GeocoderDatasource.FunderMaps => AsyncEnumerableHelper.AsEnumerable(await _analysisRepository.GetById2Async(id)),
                GeocoderDatasource.NlBagBuilding => AsyncEnumerableHelper.AsEnumerable(await _analysisRepository.GetByExternalId2Async(id)),
                GeocoderDatasource.NlBagBerth => AsyncEnumerableHelper.AsEnumerable(await _analysisRepository.GetByExternalId2Async(id)),
                GeocoderDatasource.NlBagPosting => AsyncEnumerableHelper.AsEnumerable(await _analysisRepository.GetByExternalId2Async(id)),
                GeocoderDatasource.NlBagAddress => AsyncEnumerableHelper.AsEnumerable(await _analysisRepository.GetByAddressExternalId2Async(id)),
                _ => throw new InvalidIdentifierException(),
            })
            {
                yield return product;
            }
        }

        /// <summary>
        ///     Get risk index on id.
        /// </summary>
        /// <param name="input">Input query.</param>
        public virtual Task<bool> GetRiskIndexAsync(string input)
            => _geocoderParser.FromIdentifier(input, out string id) switch
            {
                GeocoderDatasource.FunderMaps => _analysisRepository.GetRiskIndexByIdAsync(id),
                GeocoderDatasource.NlBagBuilding => _analysisRepository.GetRiskIndexByExternalIdAsync(id),
                GeocoderDatasource.NlBagBerth => _analysisRepository.GetRiskIndexByExternalIdAsync(id),
                GeocoderDatasource.NlBagPosting => _analysisRepository.GetRiskIndexByExternalIdAsync(id),
                GeocoderDatasource.NlBagAddress => _analysisRepository.GetRiskIndexByAddressExternalIdAsync(id),
                _ => throw new InvalidIdentifierException(),
            };

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
