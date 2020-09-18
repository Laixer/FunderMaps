using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Managers;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for organization administration.
    /// </summary>
    /// <remarks>
    ///     This controller provides organization administration.
    ///     <para>
    ///         For the variant based on the current session see 
    ///         <see cref="OrganizationController"/>.
    ///     </para>
    /// </remarks>
    [Authorize(Policy = "AdministratorPolicy")]
    [Route("admin/organization")]
    public class OrganizationAdminController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly OrganizationManager _organizationManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationAdminController(IMapper mapper, OrganizationManager organizationManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationManager = organizationManager ?? throw new ArgumentNullException(nameof(organizationManager));
        }

        // GET: api/admin/organization/{id}
        /// <summary>
        ///     Return organization by id.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            // Act.
            Organization organization = await _organizationManager.GetAsync(id);

            // Map.
            var output = _mapper.Map<OrganizationDto>(organization);

            // Return.
            return Ok(output);
        }

        // GET: api/admin/organization
        /// <summary>
        ///     Return all organizations.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationModel pagination)
        {
            // Act.
            IAsyncEnumerable<Organization> organizationList = _organizationManager.GetAllAsync(pagination.Navigation);

            // Map.
            var result = await _mapper.MapAsync<IList<OrganizationDto>, Organization>(organizationList);

            // Return.
            return Ok(result);
        }

        // PUT: api/admin/organization/{id}
        /// <summary>
        ///     Update organization by id.
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] OrganizationDto input)
        {
            // Map.
            var organization = _mapper.Map<Organization>(input);
            organization.Id = id;

            // Act.
            await _organizationManager.UpdateAsync(organization);

            // Return.
            return NoContent();
        }

        // DELETE: api/admin/organization/{id}
        /// <summary>
        ///     Delete organization by id.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            // Act.
            await _organizationManager.DeleteAsync(id);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
