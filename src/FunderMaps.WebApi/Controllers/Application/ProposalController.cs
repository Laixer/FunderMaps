using AutoMapper;
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
    ///     Endpoint controller for organization proposal.
    /// </summary>
    [Authorize(Policy = "AdministratorPolicy")]
    [Route("organization/proposal")]
    public class ProposalController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly OrganizationManager _organizationManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProposalController(IMapper mapper, OrganizationManager organizationManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationManager = organizationManager ?? throw new ArgumentNullException(nameof(organizationManager));
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
            organization = await _organizationManager.CreateProposalAsync(organization);

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
            OrganizationProposal organization = await _organizationManager.GetProposalAsync(id);

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
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationModel pagination)
        {
            // Act.
            IAsyncEnumerable<OrganizationProposal> organizationList = _organizationManager.GetAllProposalAsync(pagination.Navigation);

            // Map.
            var result = await _mapper.MapAsync<IList<OrganizationProposalDto>, OrganizationProposal>(organizationList);

            // Return.
            return Ok(result);
        }

        // DELETE: api/organization/proposal/{id}
        /// <summary>
        ///     Delete organization proposal by id.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            // Act.
            await _organizationManager.DeleteProposalAsync(id);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
