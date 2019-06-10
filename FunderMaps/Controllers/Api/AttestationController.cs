using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Interfaces;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Return the attestation objects in the fis database.
    /// </summary>
    [Authorize]
    [Route("api/attestation")]
    [ApiController]
    public class AttestationController : ControllerBase
    {
        private readonly IPrincipalRepository _principalRepository;
        private readonly IOrganizationRepository _organizationRepository;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public AttestationController(
            IPrincipalRepository principalRepository,
            IOrganizationRepository organizationRepository)
        {
            _principalRepository = principalRepository;
            _organizationRepository = organizationRepository;
        }

        // TODO: Match partial name against list.
        // POST: api/attestation/organization
        /// <summary>
        /// Find the organization by partial name and return list of possible values.
        /// </summary>
        /// <param name="name">Partial organization name.</param>
        /// <returns>List of organizations.</returns>
        [HttpPost("organization")]
        [ProducesResponseType(typeof(List<Organization>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PostOrganizationAsync([FromBody] string name)
        {
            return Ok(await _organizationRepository.ListAllAsync());
        }

        // TODO: Match partial name against list.
        // POST: api/attestation/principal
        /// <summary>
        /// Find the principal by partial name and return list of possible values.
        /// </summary>
        /// <param name="name">Partial principal name.</param>
        /// <returns>List of principals.</returns>
        [HttpPost("principal")]
        [ProducesResponseType(typeof(List<Principal>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PostPrincipalAsync([FromBody] string name)
        {
            return Ok(await _principalRepository.ListAllAsync());
        }
    }
}
