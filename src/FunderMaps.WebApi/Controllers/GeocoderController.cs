using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for address operations.
/// </summary>
[Route("api/geocoder")]
public class GeocoderController(IGeocoderTranslation geocoderTranslation) : ControllerBase
{
    // GET: api/geocoder/address
    /// <summary>
    ///     Get address by identifier.
    /// </summary>
    /// <remarks>
    ///     Cache response for 8 hours. Addresses will not change often.
    /// </remarks>
    [HttpGet("address/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public Task<Address> GetAddressAsync(string id)
        => geocoderTranslation.GetAddressIdAsync(id);

    // GET: api/geocoder/building
    /// <summary>
    ///     Get building by identifier.
    /// </summary>
    /// <remarks>
    ///     Cache response for 8 hours. Building will not change often.
    /// </remarks>
    [HttpGet("building/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public Task<Building> GetBuildingAsync(string id)
        => geocoderTranslation.GetBuildingIdAsync(id);
}
