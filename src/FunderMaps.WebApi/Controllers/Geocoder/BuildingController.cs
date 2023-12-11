using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for building.
/// </summary>
[Route("api/geocoder/building")]
public class BuildingController(
    IIncidentRepository incidentRepository,
    IInquirySampleRepository inquirySampleRepository,
    IRecoverySampleRepository recoverySampleRepository) : ControllerBase
{
    // GET: api/geocoder/building/{buildingId}
    [HttpGet("{buildingId}")]
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
}
