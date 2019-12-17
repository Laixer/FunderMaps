using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Geocoder endpoint.
    /// </summary>
    [Authorize]
    [Route("api/geocoder")]
    [ApiController]
    public class GeocoderController : BaseApiController
    {
        private readonly IAddressService _addressService;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="addressService">See <see cref="IAddressService"/>.</param>
        public GeocoderController(IAddressService addressService) => _addressService = addressService;

        // GET: api/geocoder/address

        [HttpGet("address")]
        [ProducesResponseType(typeof(IEnumerable<Address2>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetByAddressAsync(string streetName)
        {
            if (string.IsNullOrEmpty(streetName))
            {
                return NoContent();
            }

            // Not providing suggestions on too many matches. The search needs
            // to be narrow so the request remains to be fast.
            if (streetName.Length < 4)
            {
                return NoContent();
            }

            return Ok(await _addressService.GetAddressByStreetNameAsync(streetName));
        }
    }
}
