using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Geocoder;

/// <summary>
///     Endpoint controller for address operations.
/// </summary>
[Route("api/report")]
public sealed class ReportController(
    IIncidentRepository incidentRepository,
    IInquirySampleRepository inquirySampleRepository,
    IRecoverySampleRepository recoverySampleRepository) : ControllerBase
{
    // GET: api/report/{id}
    /// <summary>
    ///     Get all reports for a building by identifier.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReportsByBuildingAsync(string id)
    {
        var incidentList = new List<Incident>();
        await foreach (var incident in incidentRepository.ListAllByBuildingIdAsync(id))
        {
            incidentList.Add(incident);
        }

        var inquirySampleList = new List<InquirySample>();
        await foreach (var inquirySample in inquirySampleRepository.ListAllByBuildingIdAsync(id))
        {
            inquirySampleList.Add(inquirySample);
        }

        var recoverySampleList = new List<RecoverySample>();
        await foreach (var recoverySample in recoverySampleRepository.ListAllByBuildingIdAsync(id))
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
