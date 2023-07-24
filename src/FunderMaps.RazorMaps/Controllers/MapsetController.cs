using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.RazorMaps.Controllers;

/// <summary>
///     Controller for all product endpoints.
/// </summary>
[Route("mapset")]
public class MapsetController : ControllerBase
{
    private readonly IMapsetRepository _mapsetRepository;
    private readonly IIncidentRepository _incidentRepository;
    private readonly IInquirySampleRepository _inquirySampleRepository;
    private readonly IRecoverySampleRepository _recoverySampleRepository;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public MapsetController(
        IMapsetRepository mapsetRepository,
        IIncidentRepository incidentRepository,
        IInquirySampleRepository inquirySampleRepository,
        IRecoverySampleRepository recoverySampleRepository)
    {
        _mapsetRepository = mapsetRepository;
        _incidentRepository = incidentRepository;
        _inquirySampleRepository = inquirySampleRepository;
        _recoverySampleRepository = recoverySampleRepository;
    }

    [HttpGet("building/{buildingId}")]
    public async Task<IActionResult> GetReportsByBuildingAsync(string buildingId)
    {
        var incidentList = new List<Core.Entities.Incident>();
        await foreach (var incident in _incidentRepository.ListAllByBuildingIdAsync(buildingId))
        {
            incidentList.Add(incident);
        }

        var inquirySampleList = new List<Core.Entities.InquirySample>();
        await foreach (var inquirySample in _inquirySampleRepository.ListAllByBuildingIdAsync(buildingId))
        {
            inquirySampleList.Add(inquirySample);
        }

        var recoverySampleList = new List<Core.Entities.RecoverySample>();
        await foreach (var recoverySample in _recoverySampleRepository.ListAllByBuildingIdAsync(buildingId))
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
        List<Core.Entities.Mapset> mapSetList = new();

        if (id != Guid.Empty)
        {
            var set = await _mapsetRepository.GetPublicAsync2(id);
            mapSetList.Add(set);
        }

        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

            await foreach (var set in _mapsetRepository.GetByOrganizationIdAsync2(tenantId))
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
