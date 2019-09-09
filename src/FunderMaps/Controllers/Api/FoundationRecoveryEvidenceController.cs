using FunderMaps.Core.Repositories;
using FunderMaps.Authorization;
using FunderMaps.Extensions;
using FunderMaps.Helpers;
using FunderMaps.Interfaces;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    // TODO: Check the document url if it is an string or an integer.
    // For now the document string is converted to an integer so that it is possible to use the command for the repository.
    // There needs to be a better way to check for the document url.

    /// <summary>
    /// Endpoint for the foundation recovery evidence reports
    /// </summary>
    [Authorize]
    [Route("api/recoveryevidence")]
    [ApiController]
    public class FoundationRecoveryEvidenceController : BaseApiController
    {
#if _DISABLED
        private readonly IAuthorizationService _authorizationService;
        private readonly IFoundationRecoveryEvidenceRepository _recoveryEvidenceRepository;

        /// <summary>
        /// Creates a new instance of the foudnatio recovery evidence controller
        /// </summary>
        /// <param name="authorizationService">The authorization service to check users</param>
        /// <param name="recoveryEvidenceRepository">The interface for the repository</param>
        public FoundationRecoveryEvidenceController(
            IAuthorizationService authorizationService,
            IFoundationRecoveryEvidenceRepository recoveryEvidenceRepository)
        {
            _authorizationService = authorizationService;
            _recoveryEvidenceRepository = recoveryEvidenceRepository;
        }

        // GET: api/recoveryevidence
        [HttpGet]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)] // resource forbidden
        public async Task<IActionResult> GetAllAsync([FromQuery] int offset = 0, [FromQuery] int limit = 25)
        {
            var attestationOrganizationId = User.GetClaim(ClaimTypes.OrganizationAttestationIdentifier);

            // If its not able to convert it to an integer
            // This also catches it if the attestationOrganizationId equals null
            if (!int.TryParse(attestationOrganizationId, out int orgId))
            {
                return ResourceForbid();
            }

            // Hardcoded to check implementation
            orgId = 91114;

            // Administrator can query anything
            if (User.IsInRole(Constants.AdministratorRole))
            {
                // Return a list of everything
                return Ok(await _recoveryEvidenceRepository.ListAllAsync(new Navigation(offset, limit)));
            }
            // Return a list of recovery evidences based on the organization id
            return Ok(await _recoveryEvidenceRepository.ListAllAsync(orgId, new Navigation(offset, limit)));
        }

        // GET: api/recoveryevidence/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)] // resource forbidden
        public async Task<IActionResult> GetAsync(string id)
        {
            var attestationOrganizationId = User.GetClaim(ClaimTypes.OrganizationAttestationIdentifier);

            // If its not able to convert it to an integer
            // This also catches it if the attestationOrganizationId equals null
            // We discard the parsed output. We cant match the organization id with the document 
            if (!int.TryParse(attestationOrganizationId, out _))
            {
                return ResourceForbid();
            }

            // TODO: Check permissions.
            return Ok(await _recoveryEvidenceRepository.GetByIdAsync(id));
        }

        // Hit this endpoint to retrieve the amount of foundation recovery reports
        // GET: api/recoveryevidence/stats
        [HttpGet("stats")]
        [ProducesResponseType(typeof(EntityStatsOutputModel), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetStatsAsync()
        {
            var attestationOrganizationId = User.GetClaim(ClaimTypes.OrganizationAttestationIdentifier);

            // If its not able to convert it to an integer
            // this also catches it if the attestationOrganizationId equals null
            if (!int.TryParse(attestationOrganizationId, out int orgId))
            {
                return ResourceForbid();
            }

            // Hardcoded to check for item names...
            orgId = 91114;
            // Administrator can query anything
            if (User.IsInRole(Constants.AdministratorRole))
            {
                //yeet back to the admin
                return Ok(new EntityStatsOutputModel
                {
                    Count = await _recoveryEvidenceRepository.CountAsync()
                });
            }

            // TODO: Check permissions.
            return Ok(new EntityStatsOutputModel
            {
                Count = await _recoveryEvidenceRepository.CountAsync(orgId)
            });
        }


        //TODO: Document URL check -> make sure the link points to an Azure storage link
        // POST: api/recoveryevidence
        [HttpPost]
        [ProducesResponseType(typeof(FoundationRecoveryEvidence), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        [Authorize()]
        public async Task<IActionResult> PostAsync([FromBody]FoundationRecoveryEvidence input)
        {
            var attestationOrganizationId = User.GetClaim(ClaimTypes.OrganizationAttestationIdentifier);

            // NOTE: If it's not able to convert it to an integer
            //this also catches it if the attestationOrganizationId equals null
            if (!int.TryParse(attestationOrganizationId, out int orgId))
            {
                return ResourceForbid();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, orgId, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                return Ok(await _recoveryEvidenceRepository.AddAsync(input));
            }
            return ResourceForbid();
        }

        // PUT: api/recoveryevidence/5
        [HttpPut("{document}")]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)] // bad request
        [ProducesResponseType(typeof(ErrorOutputModel), 401)] // resource forbidden
        public async Task<IActionResult> PutAsync(string document, [FromBody] FoundationRecoveryEvidence input)
        {
            var attestationOrganizationId = User.GetClaim(ClaimTypes.OrganizationAttestationIdentifier);

            if (!int.TryParse(attestationOrganizationId, out int orgId))
            {
                return ResourceForbid();
            }

            // Check if the id of the url matches the id of the foundation recovery report
            if (document != input.Document)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            // pass the entity to the update method
            await _recoveryEvidenceRepository.UpdateAsync(input);
            return NoContent();
        }

        // DELETE: api/recoveryevidence/5
        [HttpDelete("{document}")]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)] // bad request
        [ProducesResponseType(typeof(ErrorOutputModel), 401)] // resource forbidden
        public async Task<IActionResult> Delete(string document, [FromBody] FoundationRecoveryEvidence input)
        {
            var attestationOrganizationId = User.GetClaim(ClaimTypes.OrganizationAttestationIdentifier);

            if (!int.TryParse(attestationOrganizationId, out int orgId))
            {
                return ResourceForbid();
            }

            // Check if the url id matches the input id
            if (document != input.Document)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            // retrieve the report based on the id
            var report = await _recoveryEvidenceRepository.GetByIdAsync(document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, orgId, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                await _recoveryEvidenceRepository.DeleteAsync(report);
                return NoContent();
            }
            return ResourceForbid();
        }
#endif
    }
}
