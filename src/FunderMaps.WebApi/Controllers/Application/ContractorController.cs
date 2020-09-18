using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Managers;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for application contractors.
    /// </summary>
    public class ContractorController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly OrganizationManager _organizationManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ContractorController(IMapper mapper, OrganizationManager organizationManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationManager = organizationManager ?? throw new ArgumentNullException(nameof(organizationManager));
        }

        // GET: api/contractor
        /// <summary>
        ///     Return all contractors.
        /// </summary>
        /// <remarks>
        ///     Cache response for 24 hours. Contractors will not change often.
        /// </remarks>
        [HttpGet("contractor"), ResponseCache(Duration = 60 * 60 * 24)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationModel pagination)
        {
            // Assign.
            IAsyncEnumerable<Organization> organizationList = _organizationManager.GetAllAsync(pagination.Navigation);

            // Map.
            var result = await _mapper.MapAsync<IList<ContractorDto>, Organization>(organizationList);

            // Return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
