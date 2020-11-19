using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for organization proposal.
    /// </summary>
    [Authorize(Policy = "AdministratorPolicy")]
    [Route("organization/proposal")]
    public class ProposalController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrganizationProposalRepository _organizationProposalRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProposalController(IMapper mapper, IOrganizationProposalRepository organizationProposalRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationProposalRepository = organizationProposalRepository ?? throw new ArgumentNullException(nameof(organizationProposalRepository));
        }

        // GET: api/organization/stats
        /// <summary>
        ///     Return organization proposal statistics.
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetStatsAsync()
        {
            // Map.
            DatasetStatsDto output = new()
            {
                Count = await _organizationProposalRepository.CountAsync(),
            };

            // Return.
            return Ok(output);
        }

        // POST: api/organization/proposal
        /// <summary>
        ///     Create organization proposal.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] OrganizationProposalDto input)
        {
            // Map.
            var organization = _mapper.Map<OrganizationProposal>(input);

            // Act.
            organization = await _organizationProposalRepository.AddGetAsync(organization);

            // Map.
            var output = _mapper.Map<OrganizationProposalDto>(organization);

            // Return.
            return Ok(output);
        }

        // GET: api/organization/proposal/{id}
        /// <summary>
        ///     Return organization proposal by id.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            // Act.
            OrganizationProposal organization = await _organizationProposalRepository.GetByIdAsync(id);

            // Map.
            var output = _mapper.Map<OrganizationProposalDto>(organization);

            // Return.
            return Ok(output);
        }

        // GET: api/organization/proposal
        /// <summary>
        ///     Return all organization proposals.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            // Act.
            IAsyncEnumerable<OrganizationProposal> organizationList = _organizationProposalRepository.ListAllAsync(pagination.Navigation);

            // Map.
            var output = await _mapper.MapAsync<IList<OrganizationProposalDto>, OrganizationProposal>(organizationList);

            // Return.
            return Ok(output);
        }

        // DELETE: api/organization/proposal/{id}
        /// <summary>
        ///     Delete organization proposal by id.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            // Act.
            await _organizationProposalRepository.DeleteAsync(id);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
