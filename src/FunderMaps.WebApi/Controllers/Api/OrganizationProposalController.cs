using FunderMaps.Core.Entities;
using FunderMaps.Core.Repositories;
using FunderMaps.Helpers;
using FunderMaps.Interfaces;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Process organization proposals.
    /// </summary>
    [Authorize(Roles = Constants.AdministratorRole)]
    [Route("api/organization_proposal")]
    [ApiController]
    public class OrganizationProposalController : BaseApiController
    {
        private readonly IOrganizationProposalRepository _organizationProposalRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ILookupNormalizer _keyNormalizer;

        /// <summary>
        /// Create a new instance of the OrganizationProposalController.
        /// </summary>
        public OrganizationProposalController(
            IOrganizationProposalRepository organizationProposalRepository,
            IOrganizationRepository organizationRepository,
            ILookupNormalizer keyNormalizer)
        {
            _organizationProposalRepository = organizationProposalRepository;
            _organizationRepository = organizationRepository;
            _keyNormalizer = keyNormalizer;
        }

        // GET: api/organization_proposal
        /// <summary>
        /// Get all organization proposals.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<OrganizationProposal>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int offset = 0, [FromQuery] int limit = 25)
            => Ok(await _organizationProposalRepository.ListAllAsync(new Navigation(offset, limit)));

        // GET: api/organization_proposal/stats
        /// <summary>
        /// Return entity statistics.
        /// </summary>
        /// <returns>EntityStatsOutputModel.</returns>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(EntityStatsOutputModel), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetStatsAsync()
            => Ok(new EntityStatsOutputModel
            {
                Count = await _organizationProposalRepository.CountAsync()
            });

        // GET: api/organization_proposal/{token}
        /// <summary>
        /// Get the organization proposal by token.
        /// </summary>
        /// <param name="token">Proposal token.</param>
        [HttpGet("{token:guid}")]
        [ProducesResponseType(typeof(OrganizationProposal), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        public async Task<IActionResult> GetAsync(Guid token)
        {
            var proposal = await _organizationProposalRepository.GetByIdAsync(token);
            if (proposal == null)
            {
                return ResourceNotFound();
            }

            return Ok(proposal);
        }

        // POST: api/organization_proposal
        /// <summary>
        /// Submit new organization proposal.
        /// </summary>
        /// <param name="input">See <see cref="OrganizationProposal"/>.</param>
        /// <returns>See <see cref="OrganizationProposal"/>.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(OrganizationProposal), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 409)]
        public async Task<IActionResult> PostAsync([FromBody] OrganizationProposal input)
        {
            input.NormalizedName = _keyNormalizer.NormalizeName(input.Name);

            // Organization proposals must be unique.
            if (await _organizationProposalRepository.GetByNormalizedNameAsync(input.NormalizedName) != null)
            {
                return Conflict();
            }

            // Organization proposals cannot use an existing organization name.
            if (await _organizationRepository.GetByNormalizedNameAsync(input.NormalizedName) != null)
            {
                return Conflict();
            }

            var id = await _organizationProposalRepository.AddAsync(input);

            // FUTURE: Send email with registration link
            // FUTURE: Kick registration done event

            return Ok(await _organizationProposalRepository.GetByIdAsync(id));
        }

        // DELETE: api/organization_proposal/{token}
        /// <summary>
        /// Remove the proposal from the datastore. This operation will
        /// not return an error if the proposal cannot be found.
        /// </summary>
        /// <param name="token">Proposal token.</param>
        [HttpDelete("{token:guid}")]
        [ProducesResponseType(typeof(OrganizationProposal), 204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        public async Task<IActionResult> DeleteAsync(Guid token)
        {
            var proposal = await _organizationProposalRepository.GetByIdAsync(token);
            if (proposal == null)
            {
                return ResourceNotFound();
            }

            await _organizationProposalRepository.DeleteAsync(proposal);

            return NoContent();
        }
    }
}
