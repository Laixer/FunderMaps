using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Webservice.Controllers;

/// <summary>
///     Controller for all product quotas.
/// </summary>
[Route("api/v3/quota")]
public sealed class QuotaController(ITelemetryRepository telemetryRepository) : ControllerBase
{
    // GET: api/quota/usage
    /// <summary>
    ///     Request product quota usage.
    /// </summary>
    public async IAsyncEnumerable<ProductTelemetry> GetQuotaUsageAsync()
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        await foreach (var telemetry in telemetryRepository.ListAllUsageAsync(tenantId))
        {
            yield return telemetry;
        }
    }
}
