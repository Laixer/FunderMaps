using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Webservice.Controllers;

/// <summary>
///     Controller for all product quotas.
/// </summary>
[Route("api/v3/quota")]
public sealed class QuotaController(ITelemetryRepository telemetryRepository) : FunderMapsController
{
    // GET: api/quota/usage
    /// <summary>
    ///     Request product quota usage.
    /// </summary>
    public async Task<IEnumerable<ProductTelemetry>> GetQuotaUsageAsync()
        => await telemetryRepository.ListAllUsageAsync(TenantId).ToListAsync();
}
