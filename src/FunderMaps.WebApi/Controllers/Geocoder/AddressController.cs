using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Geocoder
{
    /// <summary>
    ///     Endpoint controller for address operations.
    /// </summary>
    [Route("address")]
    public class AddressController : ControllerBase
    {
        private readonly IMapper _mapper;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AddressController(IMapper mapper, IGeocoderTranslation geocoderTranslation)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/address
        /// <summary>
        ///     Get address by identifier.
        /// </summary>
        /// <remarks>
        ///     Cache response for 8 hours. Addresses will not change often.
        ///     Contractors are tenant independent.
        /// </remarks>
        [HttpGet("{id}"), ResponseCache(Duration = 60 * 60 * 8)]
        public async Task<IActionResult> GetAsync(string id, [FromServices] IGeocoderTranslation geocoderTranslation)
        {
            // Assign.
            Address address = await geocoderTranslation.GetAddressIdAsync(id);

            // Map.
            var output = _mapper.Map<AddressDto>(address);

            // Return.
            return Ok(output);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
