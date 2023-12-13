using System.Diagnostics;
using System.Net;
using FunderMaps.Core.ExternalServices.FunderMaps;
using FunderMaps.Core.Types.Products;
using Microsoft.Extensions.Logging;

namespace FunderMaps.WsClient;

/// <summary>
///     Webservice client logger.
/// </summary>
public class WebserviceClientLogger(FunderMapsClient webserviceClient, ILogger<WebserviceClientLogger> logger) : IDisposable
{
    private AnalysisProduct? product;

    private async Task GetAnalysisAsync(string id, bool force = false)
    {
        if (product is not null && !force)
        {
            return;
        }

        try
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            product = await webserviceClient.GetAnalysisAsync(id);

            stopwatch.Stop();

            logger.LogDebug("Elapsed: {Elapsed}", stopwatch.Elapsed);
        }
        catch (HttpRequestException ex)
        {
            switch (ex.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    logger.LogError("Analysis product with id {id} not found", id);
                    break;

                case HttpStatusCode.Unauthorized:
                    logger.LogError("Webservice call unauthorized");
                    break;

                default:
                    logger.LogError(ex, "Error while calling webservice");
                    break;
            }
        }
    }

    private async Task<StatisticsProduct?> GetStatisticsAsync(string neighborhoodId)
    {
        try
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var statistics = await webserviceClient.GetStatisticsAsync(neighborhoodId);

            stopwatch.Stop();

            logger.LogDebug("Elapsed: {Elapsed}", stopwatch.Elapsed);

            return statistics;
        }
        catch (HttpRequestException ex)
        {
            switch (ex.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    logger.LogError("Statistics product with neighborhood {neighborhoodId} not found", neighborhoodId);
                    break;

                case HttpStatusCode.Unauthorized:
                    logger.LogError("Webservice call unauthorized");
                    break;

                default:
                    logger.LogError(ex, "Error while calling webservice");
                    break;
            }

            return null;
        }
    }

    public async Task LogAnalysisAsync(string id)
    {
        await GetAnalysisAsync(id);

        if (product is null)
        {
            return;
        }

        logger.LogInformation($"Building ID: {product.BuildingId}\n"
            + $"ExternalBuilding ID: {product.ExternalBuildingId}\n"
            + $"Address ID: {product.AddressId}\n"
            + $"ExternalAddress ID: {product.ExternalAddressId}\n"
            + $"NeighborhoodId ID: {product.NeighborhoodId}");

        logger.LogInformation($"ConstructionYear: {product.ConstructionYear}\n"
            + $"ConstructionYearReliability: {product.ConstructionYearReliability}\n"
            + $"FoundationType: {product.FoundationType}\n"
            + $"FoundationTypeReliability: {product.FoundationTypeReliability}\n"
            + $"RecoveryType: {product.RecoveryType}\n"
            + $"RestorationCosts: {product.RestorationCosts}\n"
            + $"Height: {product.Height}\n"
            + $"Velocity: {product.Velocity}\n"
            + $"GroundLevel: {product.GroundLevel}\n"
            + $"GroundWaterLevel: {product.GroundWaterLevel}\n"
            + $"Soil: {product.Soil}\n"
            + $"SurfaceArea: {product.SurfaceArea}\n"
            + $"DamageCause: {product.DamageCause}\n"
            + $"EnforcementTerm: {product.EnforcementTerm}\n"
            + $"OverallQuality: {product.OverallQuality}\n"
            + $"InquiryType: {product.InquiryType}");

        logger.LogInformation(
            $"Drystand: {product.Drystand}\n"
            + $"DrystandRisk: {product.DrystandRisk}\n"
            + $"DrystandReliability: {product.DrystandReliability}\n"
            + $"DewateringDepth: {product.DewateringDepth}\n"
            + $"DewateringDepthRisk: {product.DewateringDepthRisk}\n"
            + $"DewateringDepthReliability: {product.DewateringDepthReliability}\n"
            + $"BioInfectionRisk: {product.BioInfectionRisk}\n"
            + $"BioInfectionReliability: {product.BioInfectionReliability}\n"
            + $"UnclassifiedRisk: {product.UnclassifiedRisk}");
    }

    public async Task LogStatisticsAsync(string id)
    {
        await GetAnalysisAsync(id);

        if (product is null)
        {
            return;
        }

        var statistics = await GetStatisticsAsync(product.NeighborhoodId);
    }

    public void Dispose() => webserviceClient.Dispose();
}
