using FunderMaps.Core.ExternalServices.FunderMaps;
using FunderMaps.Core.Types.Products;
using Microsoft.Extensions.Logging;

namespace FunderMaps.WsClient;

/// <summary>
///     Webservice client logger.
/// </summary>
public class WebserviceClientLogger : IDisposable
{
    private readonly FunderMapsClient _webserviceClient;
    private readonly ILogger<WebserviceClientLogger> _logger;

    private AnalysisProduct? _product;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public WebserviceClientLogger(FunderMapsClient webserviceClient, ILogger<WebserviceClientLogger> logger)
    {
        _webserviceClient = webserviceClient ?? throw new ArgumentNullException(nameof(webserviceClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private async Task GetAnalysisAsync(string id, bool force = false)
    {
        if (_product is not null && !force)
        {
            return;
        }

        try
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            _product = await _webserviceClient.GetAnalysisAsync(id);

            stopwatch.Stop();

            _logger.LogDebug($"Elapsed: {stopwatch.Elapsed}");
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogError($"Analysis producet with id {id} not found");
            }
            else
            {
                _logger.LogError(ex, "Error while calling webservice");
            }
        }
    }

    private async Task<StatisticsProduct?> GetStatisticsAsync(string neighborhoodId)
    {
        try
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var statistics = await _webserviceClient.GetStatisticsAsync(neighborhoodId);

            stopwatch.Stop();

            _logger.LogDebug($"Elapsed: {stopwatch.Elapsed}");

            return statistics;
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogError($"Statistics producet with id {neighborhoodId} not found");
            }
            else
            {
                _logger.LogError(ex, "Error while calling webservice");
            }

            return null;
        }
    }

    public async Task LogAnalysisAsync(string id)
    {
        await GetAnalysisAsync(id);

        if (_product is null)
        {
            return;
        }

        _logger.LogInformation($"Building ID: {_product.BuildingId}\n"
            + $"ExternalBuilding ID: {_product.ExternalBuildingId}\n"
            + $"Address ID: {_product.AddressId}\n"
            + $"ExternalAddress ID: {_product.ExternalAddressId}\n"
            + $"NeighborhoodId ID: {_product.NeighborhoodId}");

        _logger.LogInformation($"ConstructionYear: {_product.ConstructionYear}\n"
            + $"ConstructionYearReliability: {_product.ConstructionYearReliability}\n"
            + $"FoundationType: {_product.FoundationType}\n"
            + $"FoundationTypeReliability: {_product.FoundationTypeReliability}\n"
            + $"RecoveryType: {_product.RecoveryType}\n"
            + $"RestorationCosts: {_product.RestorationCosts}\n"
            + $"Height: {_product.Height}\n"
            + $"Velocity: {_product.Velocity}\n"
            + $"GroundLevel: {_product.GroundLevel}\n"
            + $"GroundWaterLevel: {_product.GroundWaterLevel}\n"
            + $"Soil: {_product.Soil}\n"
            + $"SurfaceArea: {_product.SurfaceArea}\n"
            + $"DamageCause: {_product.DamageCause}\n"
            + $"EnforcementTerm: {_product.EnforcementTerm}\n"
            + $"OverallQuality: {_product.OverallQuality}\n"
            + $"InquiryType: {_product.InquiryType}");

        _logger.LogInformation(
            $"Drystand: {_product.Drystand}\n"
            + $"DrystandRisk: {_product.DrystandRisk}\n"
            + $"DrystandReliability: {_product.DrystandReliability}\n"
            + $"DewateringDepth: {_product.DewateringDepth}\n"
            + $"DewateringDepthRisk: {_product.DewateringDepthRisk}\n"
            + $"DewateringDepthReliability: {_product.DewateringDepthReliability}\n"
            + $"BioInfectionRisk: {_product.BioInfectionRisk}\n"
            + $"BioInfectionReliability: {_product.BioInfectionReliability}\n"
            + $"UnclassifiedRisk: {_product.UnclassifiedRisk}");
    }

    public async Task LogStatisticsAsync(string id)
    {
        await GetAnalysisAsync(id);

        if (_product is null)
        {
            return;
        }

        var statistics = await GetStatisticsAsync(_product.NeighborhoodId);
    }

    public void Dispose() => _webserviceClient.Dispose();
}
