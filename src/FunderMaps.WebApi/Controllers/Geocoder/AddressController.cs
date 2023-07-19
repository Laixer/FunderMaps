using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Geocoder;

// TODO: Obsolete
/// <summary>
///     Endpoint controller for address operations.
/// </summary>
[Route("api/address")]
public class AddressController : ControllerBase
{
    // GET: api/address
    /// <summary>
    ///     Get address by identifier.
    /// </summary>
    /// <remarks>
    ///     Cache response for 8 hours. Addresses will not change often.
    ///     Contractors are tenant independent.
    /// </remarks>
    [HttpGet("{id}"), ResponseCache(Duration = 60 * 60 * 8)]
    public async Task<Address> GetAsync(string id, [FromServices] IGeocoderTranslation geocoderTranslation)
        => await geocoderTranslation.GetAddressIdAsync(id);
}
