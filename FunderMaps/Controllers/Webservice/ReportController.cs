using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Interfaces;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Data.Authorization;
using FunderMaps.Extensions;
using FunderMaps.Models;

namespace FunderMaps.Controllers.Webservice
{
    /// <summary>
    /// Endpoint controller for report operations.
    /// </summary>
    [Authorize]
    [Route("api/report")]
    [ApiController]
    public class ReportController : BaseApiController
    {
        private readonly FisDbContext _fisContext;
        private readonly IAuthorizationService _authorizationService;
        private readonly IReportRepository _reportRepository;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public ReportController(
            FisDbContext fisContext,
            IAuthorizationService authorizationService,
            IReportRepository reportRepository)
        {
            _fisContext = fisContext;
            _authorizationService = authorizationService;
            _reportRepository = reportRepository;
        }

        // GET: api/report
        /// <summary>
        /// Get a chunk of reports either by organization or as public data.
        /// </summary>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of reports.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Report>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int offset = 0, [FromQuery] int limit = 25)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // TODO: attestationOrganizationId can be null
            // TODO: administrator can query anything

            return Ok(await _reportRepository.ListAllAsync(int.Parse(attestationOrganizationId), new Navigation(offset, limit)));
        }

        public sealed class EntityStatsOutputModel
        {
            public long Count { get; set; }
        }

        // GET: api/report/stats
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

            return Ok(new EntityStatsOutputModel
            {
                Count = await _reportRepository.CountAsync(int.Parse(attestationOrganizationId)),
            });
        }

        // POST: api/report
        /// <summary>
        /// Create a new report.
        /// </summary>
        /// <param name="input">Report data.</param>
        /// <returns>Report.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PostAsync([FromBody] Report input)
        {
            var attestationPrincipalId = User.GetClaim(FisClaimTypes.UserAttestationIdentifier);
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            if (attestationPrincipalId == null || attestationOrganizationId == null)
            {
                return ResourceForbid();
            }

            var report = new Report
            {
                DocumentId = input.DocumentId,
                Inspection = input.Inspection,
                JointMeasurement = input.JointMeasurement,
                FloorMeasurement = input.FloorMeasurement,
                Note = input.Note,
                Norm = input.Norm,
                Status = ReportStatus.Todo,
                Type = input.Type,
                DocumentDate = input.DocumentDate,
                AccessPolicy = AccessPolicy.Private,
                Attribution = new Attribution
                {
                    Project = input.Attribution.Project,
                    Reviewer = await _fisContext.Principal.GetOrAddAsync(input.Attribution.Reviewer, s => s.NickName == input.Attribution.Reviewer.NickName || s.Email == input.Attribution.Reviewer.Email),
                    Contractor = await _fisContext.Organization.GetOrAddAsync(input.Attribution.Contractor, s => s.Name == input.Attribution.Contractor.Name),
                    Creator = await _fisContext.Principal.FindAsync(int.Parse(attestationPrincipalId)),
                    Owner = await _fisContext.Organization.FindAsync(int.Parse(attestationOrganizationId)),
                },
            };

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution.Owner.Id, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                await _reportRepository.AddAsync(report);

                return Ok(report);
            }

            return ResourceForbid();
        }

        // GET: api/report/{id}/{document}
        /// <summary>
        /// Retrieve the report by identifier. The report is returned
        /// if the the record is public or if the organization user has
        /// access to the record.
        /// </summary>
        /// <param name="id">Report identifier.</param>
        /// <param name="document">Report identifier.</param>
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

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Read);
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

            report.Inspection = input.Inspection;
            report.JointMeasurement = input.JointMeasurement;
            report.FloorMeasurement = input.FloorMeasurement;
            report.Note = input.Note;
            report.Norm = input.Norm;

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Update);
            if (authorizationResult.Succeeded)
            {
                _fisContext.Entry(report.Norm).State = EntityState.Modified;

                await _reportRepository.UpdateAsync(report);

                return NoContent();
            }

            return ResourceForbid();
        }

        // TODO: Remove and be replaced by /validate
        // PUT: api/report/{id}/{document}/done
        /// <summary>
        /// Mark the report for as done and prepare for verification.
        /// </summary>
        /// <param name="id">Report identifier.</param>
        /// <param name="document">Report identifier.</param>
        [HttpPut("{id}/{document}/done")]
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

            if (report.Status != ReportStatus.Todo && report.Status != ReportStatus.Pending)
            {
                return Forbid(0, "Resource modification forbidden with current status");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                report.Status = ReportStatus.Done;
                await _reportRepository.UpdateAsync(report);

                return NoContent();
            }

            return ResourceForbid();
        }

        public sealed class VerificationInputModel
        {
            public enum VerificationResult
            {
                Verified,
                Rejected
            }

            public VerificationResult Result { get; set; }
        }

        // PUT: api/report/{id}/{document}/validate
        /// <summary>
        /// Save the verification result to the report.
        /// </summary>
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

            if (report.Status != ReportStatus.Done)
            {
                return Forbid(0, "Resource modification forbidden with current status");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Validate);
            if (authorizationResult.Succeeded)
            {
                switch (input.Result)
                {
                    case VerificationInputModel.VerificationResult.Verified:
                        report.Status = ReportStatus.Verified;
                        break;
                    case VerificationInputModel.VerificationResult.Rejected:
                        report.Status = ReportStatus.Rejected;
                        break;
                }

                await _reportRepository.UpdateAsync(report);

                return NoContent();
            }

            return ResourceForbid();
        }

        // TODO: Only allow removal if there a no samples.
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

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Delete);
            if (authorizationResult.Succeeded)
            {
                await _reportRepository.DeleteAsync(report);

                return NoContent();
            }

            return ResourceForbid();
        }
    }
}
