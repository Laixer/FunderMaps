using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Entities;
using FunderMaps.Core.UseCases;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.WebApi.Controllers.Geocoder
{
    /// <summary>
    ///     Endpoint controller for address operations.
    /// </summary>
    [ApiController, Route("api/address")]
    public class AddressController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly GeocoderUseCase _geocoderUseCase;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AddressController(IMapper mapper, GeocoderUseCase geocoderUseCase)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _geocoderUseCase = geocoderUseCase ?? throw new ArgumentNullException(nameof(geocoderUseCase));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([Address] string id)
        {
            Address address = await _geocoderUseCase.GetAsync(id).ConfigureAwait(false);

            var output = _mapper.Map<AddressDto>(address);

            return Ok(output);
        }

        [HttpGet]
        [Route("suggest")]
        public async Task<IActionResult> GetAllSuggestionAsync([FromQuery] AddressSearchDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var result = await _mapper.MapAsync<IList<AddressDto>, Address>(
                _geocoderUseCase.GetAllBySuggestionAsync(input.Query, input.Navigation))
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
