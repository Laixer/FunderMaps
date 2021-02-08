using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        private readonly IAddressRepository _addressRepository;
        private readonly IGeocoderTranslation _geocoderTranslation;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AddressController(IMapper mapper, IAddressRepository addressRepository, IGeocoderTranslation geocoderTranslation)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
            _geocoderTranslation = geocoderTranslation ?? throw new ArgumentNullException(nameof(geocoderTranslation));
        }

        /// <summary>
        ///     Get address by identifier.
        /// </summary>
        /// <param name="id">Address identifier.</param>
        /// <returns>Matching address.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            // Assign.
            Address address = await _geocoderTranslation.GetAddressIdAsync(id);

            // Map.
            var output = _mapper.Map<AddressBuildingDto>(address);

            // Return.
            return Ok(output);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
