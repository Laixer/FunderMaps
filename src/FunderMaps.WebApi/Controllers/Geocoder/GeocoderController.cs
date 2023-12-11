using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Geocoder;

/// <summary>
///     Endpoint controller for address operations.
/// </summary>
[Route("api/geocoder")]
public sealed class GeocoderController(
    IGeocoderTranslation geocoderTranslation,
    IIncidentRepository incidentRepository,
    IInquirySampleRepository inquirySampleRepository,
    IRecoverySampleRepository recoverySampleRepository) : ControllerBase
{
    // GET: api/geocoder/address/{id}
    /// <summary>
    ///     Get address by identifier.
    /// </summary>
    /// <remarks>
    ///     Cache response for 8 hours. Addresses will not change often.
    /// </remarks>
    [HttpGet("address/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public Task<Address> GetAddressAsync(string id)
        => geocoderTranslation.GetAddressIdAsync(id);

    // GET: api/geocoder/building/{id}
    /// <summary>
    ///     Get building by identifier.
    /// </summary>
    /// <remarks>
    ///     Cache response for 8 hours. Building will not change often.
    /// </remarks>
    [HttpGet("building/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public Task<Building> GetBuildingAsync(string id)
        => geocoderTranslation.GetBuildingIdAsync(id);

    // GET: api/geocoder/building-reports/{id}
    /// <summary>
    ///     Get all reports for a building by identifier.
    /// </summary>
    [HttpGet("building-reports/{id}")]
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
