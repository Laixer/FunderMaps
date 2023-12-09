using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for mapset.
/// </summary>
[Route("api/mapset")]
public sealed class MapsetController(
    IMapsetRepository mapsetRepository,
    IIncidentRepository incidentRepository,
    IInquirySampleRepository inquirySampleRepository,
    IRecoverySampleRepository recoverySampleRepository) : ControllerBase
{
    // TODO: Create a separate controller for builing.
    // GET: api/mapset/building/{buildingId}
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

        return Ok(new
        {
            incidentList,
            inquirySampleList,
            recoverySampleList,
        });
    }

    // GET: api/mapset/{id}
    /// <summary>
    ///     Return all mapsets the user has access to.
    /// </summary>
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    [HttpGet("{id:guid?}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var mapSetList = new List<Core.Entities.Mapset>();

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
