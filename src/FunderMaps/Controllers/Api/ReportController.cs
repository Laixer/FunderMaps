using FunderMaps.Authorization.Requirement;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Repositories;
using FunderMaps.Extensions;
using FunderMaps.Interfaces;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Endpoint controller for report operations.
    /// </summary>
    [Authorize("OrganizationMember")]
    [Route("api/report")]
    [ApiController]
    public class ReportController : BaseApiController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IReportRepository _reportRepository;
        private readonly IOrganizationRepository _organizationRepository;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public ReportController(
            IAuthorizationService authorizationService,
            IReportRepository reportRepository,
            IOrganizationRepository organizationRepository)
        {
            _authorizationService = authorizationService;
            _reportRepository = reportRepository;
            _organizationRepository = organizationRepository;
        }

        // GET: api/report
        /// <summary>
        /// Get all reports filtered either by organization or as public data.
        /// </summary>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of reports.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Report>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int offset = 0, [FromQuery] int limit = 25)
            => Ok(await _reportRepository.ListAllAsync(User.GetOrganizationId(), new Navigation(offset, limit)));

        // GET: api/report/stats
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
                Count = await _reportRepository.CountAsync(User.GetOrganizationId()),
            });

        // POST: api/report
        /// <summary>
        /// Create a new report.
        /// </summary>
        /// <remarks>
        /// Report state:
        ///     [start] -> todo
        /// </remarks>
        /// <param name="input">Report data.</param>
        /// <returns>Report.</returns>
        [HttpPost]
        [Authorize("OrganizationMember")]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PostAsync([FromBody] Report input)
        {
            // TODO: Fix attribution
            input.Status = ReportStatus.Todo;
            input.Attribution = new Attribution
            {
                Project = input.Attribution.Project,
                Reviewer = input.Attribution.Reviewer,
                Contractor = input.Attribution.Contractor,
                Creator = System.Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value),
                Owner = User.GetOrganizationId(),
            };

            // TODO: AddAsync should not return new item
            return Ok(await _reportRepository.AddAsync(input));
        }

        // GET: api/report/{id}/{document}
        /// <summary>
        /// Retrieve the report by identifier. The report is returned
        /// if the the record is public or if the organization user has
        /// access to the record.
        /// </summary>
        /// <param name="id">Report identifier, see <see cref="Report.Id"/>.</param>
        /// <param name="document">Report identifier, <see cref="Report.DocumentId"/>.</param>
        /// <returns>Report.</returns>
        [HttpGet("{id}/{document}")]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAsync(int id, string document)
        {
            var report = await _reportRepository.GetByIdAsync(id, document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            // Public data is accessible to anyone
            if (report.IsPublic())
            {
                return Ok(report);
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution, OperationsRequirement.Read);
            if (authorizationResult.Succeeded)
            {
                return Ok(report);
            }

            return ResourceForbid();
        }

        // PUT: api/report/{id}/{document}
        /// <summary>
        /// Update report if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Report identifier.</param>
        /// <param name="document">Report identifier.</param>
        /// <param name="input">Report data.</param>
        [HttpPut("{id}/{document}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PutAsync(int id, string document, [FromBody] Report input)
        {
            var report = await _reportRepository.GetByIdAsync(id, document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            if (id != input.Id || document != input.DocumentId)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution, OperationsRequirement.Update);
            if (authorizationResult.Succeeded)
            {
                report.Inspection = input.Inspection;
                report.JointMeasurement = input.JointMeasurement;
                report.FloorMeasurement = input.FloorMeasurement;
                report.Note = input.Note;
                report.Norm = input.Norm;
                report.AccessPolicy = input.AccessPolicy;

                if (input.Norm != null)
                {
                    report.Norm = input.Norm;
                }

                await _reportRepository.UpdateAsync(report);

                return NoContent();
            }

            return ResourceForbid();
        }

        // PUT: api/report/{id}/{document}/review
        /// <summary>
        /// Mark the report as 'pending_review'.
        /// </summary>
        /// <remarks>
        /// This operation can be done by anyone with the create permissions
        /// which can be different than validation.
        ///
        /// Report state:
        ///     pending -> pending_review
        /// </remarks>
        /// <param name="id">Report identifier.</param>
        /// <param name="document">Report identifier.</param>
        [HttpPut("{id}/{document}/review")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PutSignalStatusDoneAsync(int id, string document)
        {
            var report = await _reportRepository.GetByIdAsync(id, document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                if (report.Status != ReportStatus.Pending)
                {
                    return Forbid(0, "Resource modification forbidden with current status");
                }

                // Report cannot be altered anymore
                report.Status = ReportStatus.PendingReview;

                await _reportRepository.UpdateAsync(report);

                return NoContent();
            }

            return ResourceForbid();
        }

        // PUT: api/report/{id}/{document}/validate
        /// <summary>
        /// Save the validation result to the report.
        /// </summary>
        /// <remarks>
        /// Report state:
        ///     pending_review -> done
        ///     pending_review -> rejected
        /// </remarks>
        /// <param name="id">Report identifier.</param>
        /// <param name="document">Report identifier.</param>
        /// <param name="input">Verification status.</param>
        [HttpPut("{id}/{document}/validate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PutValidateRequestAsync(int id, string document, [FromBody] VerificationInputModel input)
        {
            var report = await _reportRepository.GetByIdAsync(id, document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution, OperationsRequirement.Validate);
            if (authorizationResult.Succeeded)
            {
                if (report.Status != ReportStatus.PendingReview)
                {
                    return Forbid(0, "Resource modification forbidden with current status");
                }

                switch (input.Result)
                {
                    case VerificationInputModel.VerificationResult.Verified:
                        report.Status = ReportStatus.Done;
                        break;
                    case VerificationInputModel.VerificationResult.Rejected:
                        report.Status = ReportStatus.Rejected;
                        // TODO: Notify user via input.Message
                        break;
                }

                await _reportRepository.UpdateAsync(report);

                return NoContent();
            }

            return ResourceForbid();
        }

        // DELETE: api/report/{id}/{document}
        /// <summary>
        /// Soft delete the report if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Report identifier.</param>
        /// <param name="document">Report identifier.</param>
        [HttpDelete("{id}/{document}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> DeleteAsync(int id, string document)
        {
            var report = await _reportRepository.GetByIdAsync(id, document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution, OperationsRequirement.Delete);
            if (authorizationResult.Succeeded)
            {
                // TODO: Remove only when empty

                await _reportRepository.DeleteAsync(report);

                return NoContent();
            }

            return ResourceForbid();
        }
    }
}
