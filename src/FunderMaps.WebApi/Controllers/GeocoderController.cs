using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

// TODO: Move
public sealed class GeocoderInfo
{
    public Building Building { get; set; } = default!;
    public Address Address { get; set; } = default!;
    public Neighborhood? Neighborhood { get; set; }
    public District? District { get; set; }
    public Municipality? Municipality { get; set; }
    public State? State { get; set; }
}

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

    // GET: api/geocoder/district/{id}
    /// <summary>
    ///    Get district by identifier.
    /// </summary>
    /// <remarks>
    ///    Cache response for 8 hours. District will not change often.
    /// </remarks>
    [HttpGet("district/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public Task<District> GetDistrictAsync(string id)
        => geocoderTranslation.GetDistrictIdAsync(id);

    // GET: api/geocoder/municipality/{id}
    /// <summary>
    ///    Get municipality by identifier.
    /// </summary>
    /// <remarks>
    ///   Cache response for 8 hours. Municipality will not change often.
    /// </remarks>
    [HttpGet("municipality/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public Task<Municipality> GetMunicipalityAsync(string id)
        => geocoderTranslation.GetMunicipalityIdAsync(id);

    // GET: api/geocoder/state/{id}
    /// <summary>
    ///   Get state by identifier.
    /// </summary>
    /// <remarks>
    ///   Cache response for 8 hours. State will not change often.
    /// </remarks>
    [HttpGet("state/{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public Task<State> GetStateAsync(string id)
        => geocoderTranslation.GetStateIdAsync(id);

    // GET: api/geocoder/{id}
    /// <summary>
    ///     Get geocoder information by identifier.
    /// </summary>
    /// <remarks>
    ///     Cache response for 8 hours. Building will not change often.
    /// </remarks>
    [HttpGet("{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public async Task<GeocoderInfo> GetAsync(string id)
    {
        var building = await geocoderTranslation.GetBuildingIdAsync(id);
        var address = await geocoderTranslation.GetAddressIdAsync(building.ExternalId);
        var neighborhood = building.NeighborhoodId is not null
            ? await geocoderTranslation.GetNeighborhoodIdAsync(building.NeighborhoodId)
            : null;
        // TODO: Get district from geocoderTranslation
        var district = neighborhood!.DistrictId is not null
            ? await geocoderTranslation.GetDistrictIdAsync(neighborhood.DistrictId)
            : null;
        var municipality = district!.MunicipalityId is not null
            ? await geocoderTranslation.GetMunicipalityIdAsync(district.MunicipalityId)
            : null;
        var state = municipality!.StateId is not null
            ? await geocoderTranslation.GetStateIdAsync(municipality.StateId)
            : null;

        return new GeocoderInfo
        {
            Building = building,
            Address = address,
            Neighborhood = neighborhood,
            District = district,
            Municipality = municipality,
            State = state,
        };
    }
}
