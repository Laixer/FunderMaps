using FunderMaps.AspNetCore.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Model;

/// <summary>
///     Controller for all product quotas.
/// </summary>
[Route("api/quota")]
public sealed class QuotaController(ITelemetryRepository telemetryRepository) : FunderMapsController
{
    // GET: api/quota/usage
    /// <summary>
    ///     Request product quota usage.
    /// </summary>
    public async IAsyncEnumerable<ProductTelemetry> GetQuotaUsageAsync()
    {
        await foreach (var telemetry in telemetryRepository.ListAllUsageAsync(TenantId))
        {
            yield return telemetry;
        }
    }
}
