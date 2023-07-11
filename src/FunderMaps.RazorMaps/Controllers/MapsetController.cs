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
    private readonly Core.AppContext _appContext;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public MapsetController(
        IMapsetRepository mapsetRepository,
        IIncidentRepository incidentRepository,
        IInquirySampleRepository inquirySampleRepository,
        Core.AppContext appContext)
    {
        _mapsetRepository = mapsetRepository;
        _incidentRepository = incidentRepository;
        _inquirySampleRepository = inquirySampleRepository;
        _appContext = appContext;
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

        var kaas =  new
        {
            incidentList,
            inquirySampleList,
        };

        return Ok(kaas);
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
            foreach (var organization in _appContext.Organizations)
            {
                await foreach (var set in _mapsetRepository.GetByOrganizationIdAsync2(organization.Id))
                {
                    mapSetList.Add(set);
                }
            }
        }

        return Ok(mapSetList);
    }
}
