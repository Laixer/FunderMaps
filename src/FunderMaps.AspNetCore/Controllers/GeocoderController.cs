using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.AspNetCore.Controllers;

/// <summary>
///     Endpoint controller for address operations.
/// </summary>
[Authorize, Route("api/geocoder")]
public class GeocoderController : ControllerBase
{
    private readonly IGeocoderTranslation _geocoderTranslation;

    public GeocoderController(IGeocoderTranslation geocoderTranslation)
        => _geocoderTranslation = geocoderTranslation ?? throw new ArgumentNullException(nameof(geocoderTranslation));

    // GET: api/geocoder/address
    /// <summary>
    ///     Get address by identifier.
    /// </summary>
    /// <remarks>
    ///     Cache response for 8 hours. Addresses will not change often.
    /// </remarks>
    [HttpGet("address/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public async Task<Address> GetAddressAsync(string id)
        => await _geocoderTranslation.GetAddressIdAsync(id);

    // GET: api/geocoder/building
    /// <summary>
    ///     Get building by identifier.
    /// </summary>
    /// <remarks>
    ///     Cache response for 8 hours. Building will not change often.
    /// </remarks>
    [HttpGet("building/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public async Task<Building> GetBuildingAsync(string id)
        => await _geocoderTranslation.GetBuildingIdAsync(id);
}
