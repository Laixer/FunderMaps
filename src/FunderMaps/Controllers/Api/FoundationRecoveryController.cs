using FunderMaps.Core.Entities;
using FunderMaps.Core.Repositories;
using FunderMaps.Extensions;
using FunderMaps.Helpers;
using FunderMaps.Interfaces;
using FunderMaps.Models.Identity;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Endpoint for recovery operations.
    /// </summary>
    [Authorize(Policy = Constants.OrganizationMemberPolicy)]
    [Route("api/foundationrecovery")]
    [ApiController]
    public class FoundationRecoveryController : BaseApiController
    {
        private readonly IFoundationRecoveryRepository _recoveryRepository;
        private readonly UserManager<FunderMapsUser> _userManager;

        /// <summary>
        /// Create a new instance of the foundation recovery controller.
        /// </summary>
        public FoundationRecoveryController(
            IFoundationRecoveryRepository recoveryRepository,
            UserManager<FunderMapsUser> userManager)
        {
            _recoveryRepository = recoveryRepository;
            _userManager = userManager;
        }

        // GET: api/foundationrecovery
        /// <summary>
        /// Get all the foundation recovery data based on the organisation id
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(FoundationRecovery), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int offset = 0, [FromQuery] int limit = 25)
            => Ok(await _recoveryRepository.ListAllAsync(User.GetOrganizationId(), new Navigation(offset, limit)));

        // GET: api/foundationrecovery/stats
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
                Count = await _recoveryRepository.CountAsync(User.GetOrganizationId())
            });

        // POST: api/foundationrecovery
        /// <summary>
        /// Create a new foundation recovery item.
        /// </summary>
        /// <param name="input">See <see cref="FoundationRecovery"/>.</param>
        /// <returns>See <see cref="FoundationRecovery"/>.</returns>
        [HttpPost]
        [Authorize(Policy = Constants.OrganizationMemberWritePolicy)]
        [ProducesResponseType(typeof(FoundationRecovery), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PostAsync([FromBody]FoundationRecovery input)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return ResourceNotFound();
            }

            // TODO: Check reviewer, contractor

            input.Attribution = new Attribution
            {
                Project = input.Attribution.Project,
                Reviewer = input.Attribution.Reviewer,
                Contractor = input.Attribution.Contractor,
                Creator = user.Id,
                Owner = User.GetOrganizationId(),
            };

            var id = await _recoveryRepository.AddAsync(input);
            return Ok(await _recoveryRepository.GetByIdAsync(id));
        }

        // GET: api/foundationrecovery/{id}
        /// <summary>
        /// Get all the data of an foundation recovery report based on the ID given in the get request
        /// functions as a read something method.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns>List of recovery items.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FoundationRecovery), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var recovery = await _recoveryRepository.GetPublicAndByIdAsync(id, User.GetOrganizationId());
            if (recovery == null)
            {
                return ResourceNotFound();
            }

            return Ok(recovery);
        }

        // PUT: api/foundationrecovery/id
        /// <summary>
        /// Update recovery.
        /// </summary>
        /// <param name="id">Recovery item identifier.</param>
        /// <param name="input">See <see cref="FoundationRecovery"/>.</param>
        [HttpPut("{id}")]
        [Authorize(Policy = Constants.OrganizationMemberWritePolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] FoundationRecovery input)
        {
            var recovery = await _recoveryRepository.GetByIdAsync(id, User.GetOrganizationId());
            if (recovery == null)
            {
                return ResourceNotFound();
            }

            // TODO: FoundationRecoveryRepair

            recovery.Year = input.Year;
            recovery.Note = input.Note;
            recovery.Type = input.Type;
            recovery.AccessPolicy = input.AccessPolicy;

            await _recoveryRepository.UpdateAsync(recovery);

            return NoContent();
        }

        // DELETE: api/foundationrecovery/id
        /// <summary>
        /// Soft delete the recovery.
        /// </summary>
        /// <param name="id">Recovery item identifier.</param>
        [HttpDelete("{id}")]
        [Authorize(Policy = Constants.OrganizationMemberWritePolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var recovery = await _recoveryRepository.GetByIdAsync(id, User.GetOrganizationId());
            if (recovery == null)
            {
                return ResourceNotFound();
            }

            await _recoveryRepository.DeleteAsync(recovery);

            return NoContent();
        }
    }
}
