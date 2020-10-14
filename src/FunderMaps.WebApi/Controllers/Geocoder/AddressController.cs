using AutoMapper;
using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AddressController(IMapper mapper, IAddressRepository addressRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        }

        /// <summary>
        ///     Get address by identifier.
        /// </summary>
        /// <param name="id">Address identifier.</param>
        /// <returns>Matching address.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([Geocoder] string id)
        {
            // Assign.
            Address address = await _addressRepository.GetByIdAsync(id);

            // Map.
            var output = _mapper.Map<AddressBuildingDto>(address);

            // Return.
            return Ok(output);
        }

        /// <summary>
        ///     Get address suggestions.
        /// </summary>
        /// <param name="input">Address search query.</param>
        /// <returns>List of matching addresses.</returns>
        [HttpGet("suggest")]
        public async Task<IActionResult> GetAllSuggestionAsync([FromQuery] AddressSearchDto input)
        {
            // Assign.
            IAsyncEnumerable<Address> addressList = _addressRepository.GetBySearchQueryAsync(input.Query, input.Navigation);

            // Map.
            var result = await _mapper.MapAsync<IList<AddressBuildingDto>, Address>(addressList);

            // Return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
