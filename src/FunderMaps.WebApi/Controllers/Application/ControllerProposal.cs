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

namespace FunderMaps.WebApi.Controllers.Application
{
    [Authorize]
    [ApiController, Route("api/organization/proposal")]
    public class ControllerProposal : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly OrganizationManager _organizationManager;

        public ControllerProposal(IMapper mapper, OrganizationManager organizationManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationManager = organizationManager ?? throw new ArgumentNullException(nameof(organizationManager));
        }

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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            // Act.
            await _organizationManager.DeleteAsync(id).ConfigureAwait(false);

            // Return.
            return NoContent();
        }
    }
}
