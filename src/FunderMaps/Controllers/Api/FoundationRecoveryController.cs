using FunderMaps.Authorization.Requirement;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;
using FunderMaps.Data.Authorization;
using FunderMaps.Extensions;
using FunderMaps.Helpers;
using FunderMaps.Interfaces;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Endpoint for recovery operations.
    /// </summary>
    [Authorize]
    [Route("api/foundationrecovery")]
    [ApiController]
    public class FoundationRecoveryController : BaseApiController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IFoundationRecoveryRepository _recoveryRepository;

        /// <summary>
        /// Create a new instance of the foundation recovery controller.
        /// </summary>
        public FoundationRecoveryController(
            IAuthorizationService authorizationService,
            IFoundationRecoveryRepository recoveryRepository)
        {
            _authorizationService = authorizationService;
            _recoveryRepository = recoveryRepository;
        }

        // functions as a read all method
        /// <summary>
        /// Get all the foundation recovery data based on the organisation id
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        // GET: api/foundationrecovery
        [HttpGet]
        [ProducesResponseType(typeof(FoundationRecovery), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAllAsync([FromQuery] uint offset = 0, [FromQuery] uint limit = 25)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // If its not able to convert it to an integer
            // this also catches it if the attestationOrganizationId equals null
            if (!int.TryParse(attestationOrganizationId, out int orgId))
            {
                return ResourceForbid();
            }

            // Administrator can query anything
            if (User.IsInRole(Constants.AdministratorRole))
            {
                return Ok(await _recoveryRepository.ListAllAsync(new Navigation(offset, limit)));
            }

            return Ok(await _recoveryRepository.ListAllAsync(orgId, new Navigation(offset, limit)));
        }


        // Functions as a read something method
        /// <summary>
        /// Get all the data of a foundation recovery report based on the ID given in the get request
        /// </summary>
        /// <param name="id">The id of the foundation recovery report</param>
        /// <returns>The foundation recovery report</returns>
        // GET: api/foundationrecovery/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FoundationRecovery), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // If its not able to convert it to an integer
            // this also catches it if the attestationOrganizationId equals null
            if (!int.TryParse(attestationOrganizationId, out int orgId))
            {
                return ResourceForbid();
            }

            // TODO: Check permissions.

            return Ok(await _recoveryRepository.GetByIdAsync(id));
        }

        // TODO: met of zonder de "deleted" reports

        // Hit this endpoint to retrieve the amount of foundation recovery reports
        // GET: api/foundationrecovery/stats
        [HttpGet("stats")]
        [ProducesResponseType(typeof(EntityStatsOutputModel), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetStatsAsync()
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // If its not able to convert it to an integer
            // this also catches it if the attestationOrganizationId equals null
            if (!int.TryParse(attestationOrganizationId, out int orgId))
            {
                return ResourceForbid();
            }

            // Administrator can query anything
            if (User.IsInRole(Constants.AdministratorRole))
            {
                //yeet back to the admin
                return Ok(new EntityStatsOutputModel
                {
                    Count = await _recoveryRepository.CountAsync()
                });
            }

            return Ok(new EntityStatsOutputModel
            {
                Count = await _recoveryRepository.CountAsync(orgId)
            });
        }

        // This is like a create method. This pushes the foundation recovery info into the database
        // Create a new foundation recovery report.
        // POST: api/foundationrecovery
        [HttpPost]
        [ProducesResponseType(typeof(FoundationRecovery), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PostAsync([FromBody]FoundationRecovery input)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // NOTE: If it's not able to convert it to an integer
            //this also catches it if the attestationOrganizationId equals null
            if (!int.TryParse(attestationOrganizationId, out int orgId))
            {
                return ResourceForbid();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, orgId, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                //var recovery = new FoundationRecovery
                //{
                //    Note = input.Note,
                //    Type = input.Type,
                //    Year = input.Year,
                //    Address = input.Address,
                //    Attribution = input.Attribution,
                //    AccessPolicy = input.AccessPolicy,
                //    AddressNavigation = input.AddressNavigation,
                //    AttributionNavigation = input.AttributionNavigation,
                //    FoundationRecoveryRepair = input.FoundationRecoveryRepair,
                //    FoundationRecoveryEvidence = input.FoundationRecoveryEvidence                    
                //};

                return Ok(await _recoveryRepository.AddAsync(input));
            }
            return ResourceForbid();
        }

        // Update info about the fundation recovery
        // PUT: api/foundationrecovery/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] FoundationRecovery input)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            if (!int.TryParse(attestationOrganizationId, out int orgId))
            {
                return ResourceForbid();
            }

            // Check if the id of the url matches the id of the foundation recovery report
            if (id != input.Id)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            // Put all the info from the request body into a new foundation recovery object
            //var recovery = new FoundationRecovery
            //{
            //    AccessPolicy = input.AccessPolicy,
            //    Address = input.Address,
            //    AddressNavigation = input.AddressNavigation,
            //    Attribution = input.Attribution,
            //    AttributionNavigation = input.AttributionNavigation,
            //    FoundationRecoveryEvidence = input.FoundationRecoveryEvidence,
            //    FoundationRecoveryRepair = input.FoundationRecoveryRepair,
            //    Id = input.Id,
            //    Note = input.Note,
            //    Type = input.Type,
            //    Year = input.Year                
            //};

            // Send the created recovery object to the repo
            await _recoveryRepository.UpdateAsync(input);

            return NoContent();
        }

        // Set a report as deleted wehn hitting this endpoint
        // DELETE: api/foundationrecovery/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, [FromBody] FoundationRecovery input)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            if (!int.TryParse(attestationOrganizationId, out int orgId))
            {
                return ResourceForbid();
            }

            // Check if the url id matches the input id
            if (id != input.Id)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            // retrieve the report based on the id
            var report = await _recoveryRepository.GetByIdAsync(id);
            if (report == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, orgId, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                await _recoveryRepository.DeleteAsync(report);

                return NoContent();
            }

            return ResourceForbid();
        }
    }
}
