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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Endpoint controller for sample operations.
    /// </summary>
    [Authorize]
    [Route("api/sample")]
    [ApiController]
    public class SampleController : BaseApiController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ISampleRepository _sampleRepository;
        private readonly IReportRepository _reportRepository;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public SampleController(
            IAuthorizationService authorizationService,
            ISampleRepository sampleRepository,
            IReportRepository reportRepository)
        {
            _authorizationService = authorizationService;
            _sampleRepository = sampleRepository;
            _reportRepository = reportRepository;
        }

        // GET: api/sample
        /// <summary>
        /// Get all samples filtered either by organization or as public data.
        /// </summary>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of samples.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Sample>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int offset = 0, [FromQuery] int limit = 25)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // Administrator can query anything without organization filter
            if (User.IsInRole(Constants.AdministratorRole))
            {
                return Ok(await _sampleRepository.ListAllAsync(new Navigation(offset, limit)));
            }

            if (!int.TryParse(attestationOrganizationId, out int attOrgId))
            {
                return ResourceForbid();
            }

            return Ok(await _sampleRepository.ListAllAsync(attOrgId, new Navigation(offset, limit)));
        }

        // GET: api/sample/report/{id}
        /// <summary>
        /// Get all samples filtered by report.
        /// </summary>
        /// <param name="id">Report identifier, see <see cref="Report.Id"/>.</param>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of samples, see <see cref="Report"/>.</returns>
        [HttpGet("report/{id}")]
        [ProducesResponseType(typeof(List<Sample>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAllAsync(int id, [FromQuery] int offset = 0, [FromQuery] int limit = 25)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // Administrator can query anything without organization filter
            if (User.IsInRole(Constants.AdministratorRole))
            {
                return Ok(await _sampleRepository.ListAllReportAsync(id, new Navigation(offset, limit)));
            }

            if (!int.TryParse(attestationOrganizationId, out int attOrgId))
            {
                return ResourceForbid();
            }

            return Ok(await _sampleRepository.ListAllReportAsync(id, attOrgId, new Navigation(offset, limit)));
        }

        // GET: api/sample/stats
        /// <summary>
        /// Return entity statistics.
        /// </summary>
        /// <returns>EntityStatsOutputModel.</returns>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(EntityStatsOutputModel), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetStatsAsync()
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // Administrator can query anything without organization filter
            if (User.IsInRole(Constants.AdministratorRole))
            {
                return Ok(new EntityStatsOutputModel
                {
                    Count = await _sampleRepository.CountAsync()
                });
            }

            if (!int.TryParse(attestationOrganizationId, out int attOrgId))
            {
                return ResourceForbid();
            }

            return Ok(new EntityStatsOutputModel
            {
                Count = await _sampleRepository.CountAsync(attOrgId)
            });
        }

        // POST: api/sample
        /// <summary>
        /// Create a new sample for a report.
        /// </summary>
        /// <param name="input">See <see cref="Sample"/>.</param>
        /// <returns>See <see cref="Sample"/>.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Sample), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PostAsync([FromBody] Sample input)
        {
            var report = await _reportRepository.GetByIdAsync(input.Report.Value);
            if (report == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                switch (report.Status)
                {
                    case ReportStatus.Todo:
                        {
                            // TODO: HACK. Should not cast to known type.
                            await (_reportRepository as Data.Repositories.ReportRepository).UpdateStatusAsync(report, ReportStatus.Pending);
                        }
                        break;
                    case ReportStatus.Pending:
                        break;
                    case ReportStatus.Done:
                    case ReportStatus.Discarded:
                    case ReportStatus.PendingReview:
                    case ReportStatus.Rejected:
                    default:
                        return Forbid(0, "Resource modification forbidden with current status");
                }

                return Ok(await _sampleRepository.AddAsync(input));
            }

            return ResourceForbid();
        }

        // GET: api/sample/{id}
        /// <summary>
        /// Retrieve the sample by identifier. The sample is returned
        /// if the the record is public or if the organization user has
        /// access to the record.
        /// </summary>
        /// <param name="id">Sample identifier.</param>
        /// <returns>Report.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Sample), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var sample = await _sampleRepository.GetByIdAsync(id);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            // TODO: sample.ReportNavigation.IsPublic()
            if (sample.IsPublic())
            {
                return Ok(sample);
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.Attribution, OperationsRequirement.Read);
            if (authorizationResult.Succeeded)
            {
                return Ok(sample);
            }

            return ResourceForbid();
        }

        // PUT: api/sample/{id}
        /// <summary>
        /// Update sample if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Sample identifier.</param>
        /// <param name="input">Sample data.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Sample input)
        {
            if (id != input.Id)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            var sample = await _sampleRepository.GetByIdAsync(id);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.Attribution, OperationsRequirement.Update);
            if (authorizationResult.Succeeded)
            {
                await _sampleRepository.UpdateAsync(input);

                return NoContent();
            }

            return ResourceForbid();
        }

        // DELETE: api/sample/{id}
        /// <summary>
        /// Soft delete the sample if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Sample identifier.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var sample = await _sampleRepository.GetByIdAsync(id);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.Attribution, OperationsRequirement.Delete);
            if (authorizationResult.Succeeded)
            {
                await _sampleRepository.DeleteAsync(sample);

                return NoContent();
            }

            return ResourceForbid();
        }
    }
}
