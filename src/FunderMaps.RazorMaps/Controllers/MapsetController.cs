using System.Security.Claims;
using FunderMaps.Core.Authentication;
using FunderMaps.Core.Controllers;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.RazorMaps.Controllers;

/// <summary>
///     Controller for all product endpoints.
/// </summary>
[Route("mapset")]
public class MapsetController(
    IMapsetRepository mapsetRepository,
    IIncidentRepository incidentRepository,
    IInquirySampleRepository inquirySampleRepository,
    IRecoverySampleRepository recoverySampleRepository) : FunderMapsController
{
    [HttpGet("building/{buildingId}")]
    public async Task<IActionResult> GetReportsByBuildingAsync(string buildingId)
    {
        var incidentList = new List<Core.Entities.Incident>();
        await foreach (var incident in incidentRepository.ListAllByBuildingIdAsync(buildingId))
        {
            incidentList.Add(incident);
        }

        var inquirySampleList = new List<Core.Entities.InquirySample>();
        await foreach (var inquirySample in inquirySampleRepository.ListAllByBuildingIdAsync(buildingId))
        {
            inquirySampleList.Add(inquirySample);
        }

        var recoverySampleList = new List<Core.Entities.RecoverySample>();
        await foreach (var recoverySample in recoverySampleRepository.ListAllByBuildingIdAsync(buildingId))
        {
            recoverySampleList.Add(recoverySample);
        }

        var buildingProfile = new
        {
            incidentList,
            inquirySampleList,
            recoverySampleList,
        };

        return Ok(buildingProfile);
    }

    // GET: mapset
    /// <summary>
    ///     Request the analysis product.
    /// </summary>
    // [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
    [HttpGet("{id:guid?}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        List<Core.Entities.Mapset> mapSetList = [];

        if (id != Guid.Empty)
        {
            var set = await mapsetRepository.GetPublicAsync2(id);
            mapSetList.Add(set);
        }

        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

            await foreach (var set in mapsetRepository.GetByOrganizationIdAsync2(tenantId))
            {
                mapSetList.Add(set);
            }

            // foreach (var organization in _appContext.Organizations)
            // {
            //     await foreach (var set in _mapsetRepository.GetByOrganizationIdAsync2(organization.Id))
            //     {
            //         mapSetList.Add(set);
            //     }
            // }
        }

        return Ok(mapSetList);
    }
}
