using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for application contractors.
    /// </summary>
    public class ContractorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IOrganizationRepository _organizationRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ContractorController(IMapper mapper, IMemoryCache cache, IOrganizationRepository organizationRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
        }

        // GET: api/contractor
        /// <summary>
        ///     Return all contractors.
        /// </summary>
        /// <remarks>
        ///     Cache response for 8 hours. Contractors will not change often.
        ///     Contractors are tenant independent.
        /// </remarks>
        [HttpGet("contractor"), ResponseCache(Duration = 60 * 60 * 8)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            // README: XXX: Response caching is a test

            // Fetch.
            if (!_cache.TryGetValue(nameof(ContractorDto), out IList<ContractorDto> result))
            {
                // Assign.
                IAsyncEnumerable<Organization> organizationList = _organizationRepository.ListAllAsync(pagination.Navigation);

                // Map.
                result = await _mapper.MapAsync<IList<ContractorDto>, Organization>(organizationList);

                // Set.
                _cache.Set(nameof(ContractorDto), result, TimeSpan.FromHours(1));
            }

            // Return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
