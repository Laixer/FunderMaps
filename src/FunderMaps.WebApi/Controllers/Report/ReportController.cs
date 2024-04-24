using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Geocoder;

/// <summary>
///     Endpoint controller for address operations.
/// </summary>
[Route("api/report")]
public sealed class ReportController(
    IIncidentRepository incidentRepository,
    IInquirySampleRepository inquirySampleRepository,
    IRecoverySampleRepository recoverySampleRepository,
    GeocoderTranslation geocoderTranslation) : FunderMapsController
{
    // TODO: Maybe move the whole geocoder translation to the database.
    // GET: api/report/{id}
    /// <summary>
    ///     Get all reports for a building by identifier.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReportsByBuildingAsync(string id)
    {
        var building = await geocoderTranslation.GetBuildingIdAsync(id);

        var incidentList = new List<Incident>();
        await foreach (var incident in incidentRepository.ListAllByBuildingIdAsync(building.Id))
        {
            incidentList.Add(incident);
        }

        var inquirySampleList = new List<InquirySample>();
        await foreach (var inquirySample in inquirySampleRepository.ListAllByBuildingIdAsync(building.Id))
        {
            inquirySampleList.Add(inquirySample);
        }

        var recoverySampleList = new List<RecoverySample>();
        await foreach (var recoverySample in recoverySampleRepository.ListAllByBuildingIdAsync(building.Id))
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
