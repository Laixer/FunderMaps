using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;

namespace FunderMaps.Core.Services;

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

    /// <summary>
    ///     Get an analysis product v3.
    /// </summary>
    /// <param name="input">Input query.</param>
    public virtual Task<AnalysisProduct3> GetAnalysis3Async(string input)
        => _analysisRepository.Get3Async(input);

    /// <summary>
    ///     Get risk index on id.
    /// </summary>
    /// <param name="input">Input query.</param>
    public virtual Task<bool> GetRiskIndexAsync(string input)
        => _analysisRepository.GetRiskIndexAsync(input);

    /// <summary>
    ///     Get statistics per region.
    /// </summary>
    /// <param name="input">Input query.</param>
    public virtual Task<StatisticsProduct> GetStatistics3Async(string input)
        => GetStatisticsByIdAsync(input);
}
