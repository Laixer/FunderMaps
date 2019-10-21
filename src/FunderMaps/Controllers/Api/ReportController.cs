using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Repositories;
using FunderMaps.Extensions;
using FunderMaps.Helpers;
using FunderMaps.Interfaces;
using FunderMaps.Models.Identity;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Endpoint controller for report operations.
    /// </summary>
    [Authorize(Policy = Constants.OrganizationMemberPolicy)]
    [Route("api/report")]
    [ApiController]
    public class ReportController : BaseApiController
    {
        private const int hoursValid = 1;

        private readonly IReportRepository _reportRepository;
        private readonly UserManager<FunderMapsUser> _userManager;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IStringLocalizer<ReportController> _localizer;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public ReportController(
            IReportRepository reportRepository,
            UserManager<FunderMapsUser> userManager,
            IOrganizationRepository organizationRepository,
            IOrganizationUserRepository organizationUserRepository,
            IFileStorageService fileStorageService,
            IStringLocalizer<ReportController> localizer)
        {
            _reportRepository = reportRepository;
            _userManager = userManager;
            _organizationRepository = organizationRepository;
            _organizationUserRepository = organizationUserRepository;
            _fileStorageService = fileStorageService;
            _localizer = localizer;
        }

        // GET: api/report
        /// <summary>
        /// Get all reports filtered either by organization or as public data.
        /// </summary>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of <see cref="Report"/>.</returns>
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
        [Authorize(Policy = Constants.OrganizationMemberWritePolicy)]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PostAsync([FromBody] Report input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                // TODO: send error code.
                return ResourceNotFound();
            }

            // TODO: We should not match in memory, but let the datastore do the work.
            var reviewers = await _organizationUserRepository.ListAllByOrganizationByRoleIdAsync(OrganizationRole.Verifier, User.GetOrganizationId(), new Navigation(0, 1000));
            if (!reviewers.Any(s => s.Id == input.Attribution.Reviewer))
            {
                // TODO: send error code.
                return ResourceNotFound();
            }

            // TODO: We should not match in memory, but let the datastore do the work.
            var contractors = await _organizationRepository.ListAllContractorsAsync(new Navigation(0, 1000));
            if (!contractors.Any(s => s.Id == input.Attribution.Contractor))
            {
                // TODO: send error code.
                return ResourceNotFound();
            }

            // FUTURE: This should be 'todo' and only when samples
            //         are added to the report will the status be
            //         set to 'pending'. However the frontend is not
            //         yet prepared to deal with this flow.

            input.Status = ReportStatus.Pending;
            input.Attribution = new Attribution
            {
                Project = input.Attribution.Project,
                Reviewer = input.Attribution.Reviewer,
                Contractor = input.Attribution.Contractor,
                Creator = user.Id,
                Owner = User.GetOrganizationId(),
            };

            return Ok(await _reportRepository.GetByIdAsync(await _reportRepository.AddAsync(input)));
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
            var report = await _reportRepository.GetPublicAndByIdAsync(id, document, User.GetOrganizationId());
            if (report == null)
            {
                return ResourceNotFound();
            }

            return Ok(report);
        }

        // GET: api/report/{id}/{document}/download
        /// <summary>
        /// Get link to the report file resource from the report.
        /// </summary>
        /// <param name="id">Report identifier, see <see cref="Report.Id"/>.</param>
        /// <param name="document">Report identifier, <see cref="Report.DocumentId"/>.</param>
        /// <returns>Report.</returns>
        [HttpGet("{id}/{document}/download")]
        [ProducesResponseType(typeof(FileDownloadOutputModel), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetDownloadLinkAsync(int id, string document)
        {
            var report = await _reportRepository.GetPublicAndByIdAsync(id, document, User.GetOrganizationId());
            if (report == null)
            {
                return ResourceNotFound();
            }

            // There is no document stored.
            if (string.IsNullOrEmpty(report.DocumentName))
            {
                return ResourceNotFound();
            }

            // There is no document stored.
            if (!await _fileStorageService.FileExists(Constants.ReportStorage, report.DocumentName))
            {
                return ResourceNotFound();
            }

            // TODO: There is no error handling so far.
            return Ok(new FileDownloadOutputModel
            {
                Url = _fileStorageService.GetAccessLink(Constants.ReportStorage, report.DocumentName, hoursValid),
                UrlValid = hoursValid,
            });
        }

        // PUT: api/report/{id}/{document}
        /// <summary>
        /// Update report if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Report identifier.</param>
        /// <param name="document">Report identifier.</param>
        /// <param name="input">Report data.</param>
        [HttpPut("{id}/{document}")]
        [Authorize(Policy = Constants.OrganizationMemberWritePolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PutAsync(int id, string document, [FromBody] Report input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var report = await _reportRepository.GetByIdAsync(id, document, User.GetOrganizationId());
            if (report == null)
            {
                return ResourceNotFound();
            }

            report.DocumentDate = input.DocumentDate;
            report.Type = input.Type;
            report.Inspection = input.Inspection;
            report.JointMeasurement = input.JointMeasurement;
            report.FloorMeasurement = input.FloorMeasurement;
            report.Note = input.Note;
            report.AccessPolicy = input.AccessPolicy;

            if (input.Norm != null)
            {
                report.Norm = input.Norm;
            }

            await _reportRepository.UpdateAsync(report);

            return NoContent();
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
        [Authorize(Policy = Constants.OrganizationMemberWritePolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PutSignalStatusDoneAsync(int id, string document)
        {
            var report = await _reportRepository.GetByIdAsync(id, document, User.GetOrganizationId());
            if (report == null)
            {
                return ResourceNotFound();
            }

            if (report.Status != ReportStatus.Pending)
            {
                return Forbid(0, _localizer["Resource modification forbidden with current status"]);
            }

            report.Status = ReportStatus.PendingReview;

            await _reportRepository.UpdateAsync(report);

            return NoContent();
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
        [Authorize(Policy = Constants.OrganizationMemberVerifyPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PutValidateRequestAsync(int id, string document, [FromBody] VerificationInputModel input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var report = await _reportRepository.GetByIdAsync(id, document, User.GetOrganizationId());
            if (report == null)
            {
                return ResourceNotFound();
            }

            if (report.Status != ReportStatus.PendingReview)
            {
                return Forbid(0, _localizer["Resource modification forbidden with current status"]);
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

        // DELETE: api/report/{id}/{document}
        /// <summary>
        /// Soft delete the report if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Report identifier.</param>
        /// <param name="document">Report identifier.</param>
        [HttpDelete("{id}/{document}")]
        [Authorize(Policy = Constants.OrganizationMemberWritePolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> DeleteAsync(int id, string document)
        {
            var report = await _reportRepository.GetByIdAsync(id, document, User.GetOrganizationId());
            if (report == null)
            {
                return ResourceNotFound();
            }

            // TODO: Remove only when empty

            await _reportRepository.DeleteAsync(report);

            return NoContent();
        }
    }
}
