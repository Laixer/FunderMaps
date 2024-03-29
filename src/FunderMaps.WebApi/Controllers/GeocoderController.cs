using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for address operations.
/// </summary>
[Route("api/geocoder")]
public sealed class GeocoderController(GeocoderTranslation geocoderTranslation) : FunderMapsController
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

    // GET: api/geocoder/neighborhood/{id}
    /// <summary>
    ///     Get neighborhood by identifier.
    /// </summary>
    /// <remarks>
    ///     Cache response for 8 hours. Neighborhood will not change often.
    /// </remarks>
    [HttpGet("neighborhood/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public Task<Neighborhood> GetNeighborhoodAsync(string id)
        => geocoderTranslation.GetNeighborhoodIdAsync(id);
}
