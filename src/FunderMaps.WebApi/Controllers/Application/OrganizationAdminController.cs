using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for organization administration.
    /// </summary>
    /// <remarks>
    ///     This controller provides organization administration.
    ///     <para>
    ///         For the variant based on the current session see 
    ///         <see cref="FunderMaps.AspNetCore.Controllers.OrganizationController"/>.
    ///     </para>
    /// </remarks>
    [Authorize(Policy = "AdministratorPolicy")]
    [Route("admin/organization")]
    public class OrganizationAdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrganizationRepository _organizationRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationAdminController(IMapper mapper, IOrganizationRepository organizationRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
        }

        // GET: api/admin/organization/stats
        /// <summary>
        ///     Return organization statistics.
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetStatsAsync()
        {
            // Map.
            DatasetStatsDto output = new()
            {
                Count = await _organizationRepository.CountAsync(),
            };

            // Return.
            return Ok(output);
        }

        // GET: api/admin/organization/{id}
        /// <summary>
        ///     Return organization by id.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            // Act.
            Organization organization = await _organizationRepository.GetByIdAsync(id);

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
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            // Act.
            IAsyncEnumerable<Organization> organizationList = _organizationRepository.ListAllAsync(pagination.Navigation);

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
            await _organizationRepository.UpdateAsync(organization);

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
            await _organizationRepository.DeleteAsync(id);

            // Return.
            return NoContent();
        }
    }
}
