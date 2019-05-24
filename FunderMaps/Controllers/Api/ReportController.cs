using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FunderMaps.Interfaces;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Data.Authorization;
using FunderMaps.Extensions;
using FunderMaps.ViewModels;
using FunderMaps.Helpers;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Endpoint controller for report operations.
    /// </summary>
    [Authorize]
    [Route("api/report")]
    [ApiController]
    public class ReportController : BaseApiController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IReportRepository _reportRepository;
        private readonly IPrincipalRepository _principalRepository;
        private readonly ISampleRepository _sampleRepository;
        private readonly IOrganizationRepository _organizationRepository;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public ReportController(
            IAuthorizationService authorizationService,
            IReportRepository reportRepository,
            ISampleRepository sampleRepository,
            IPrincipalRepository principalRepository,
            IOrganizationRepository organizationRepository)
        {
            _authorizationService = authorizationService;
            _reportRepository = reportRepository;
            _sampleRepository = sampleRepository;
            _principalRepository = principalRepository;
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
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // Administrator can query anything
            if (User.IsInRole(Constants.AdministratorRole))
            {
                return Ok(await _reportRepository.ListAllAsync(new Navigation(offset, limit)));
            }

            if (attestationOrganizationId == null)
            {
                return ResourceForbid();
            }

            return Ok(await _reportRepository.ListAllAsync(int.Parse(attestationOrganizationId), new Navigation(offset, limit)));
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

            // Administrator can query anything
            if (User.IsInRole(Constants.AdministratorRole))
            {
                return Ok(new EntityStatsOutputModel
                {
                    Count = await _reportRepository.CountAsync()
                });
            }

            if (attestationOrganizationId == null)
            {
                return ResourceForbid();
            }

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

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, int.Parse(attestationOrganizationId), OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
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
                    AccessPolicy = input.AccessPolicy,
                    Attribution = new Attribution
                    {
                        Project = input.Attribution.Project,
                        Reviewer = await _principalRepository.GetOrAddAsync(input.Attribution.Reviewer),
                        Contractor = await _organizationRepository.GetOrAddAsync(input.Attribution.Contractor),
                        _Creator = int.Parse(attestationPrincipalId),
                        _Owner = int.Parse(attestationOrganizationId),
                    },
                };

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

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Update);
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
                    if (report.Norm != null)
                    {
                        report.Norm.ConformF3o = input.Norm.ConformF3o;
                    }
                    else
                    {
                        report.Norm = input.Norm;
                    }
                }

                await _reportRepository.UpdateAsync(report);

                return NoContent();
            }

            return ResourceForbid();
        }

        // PUT: api/report/{id}/{document}/done
        /// <summary>
        /// Mark the report as done and prepare for verification.
        /// </summary>
        /// <remarks>
        /// This operation can be done by anyone with the create permissions
        /// which can be different than validation.
        /// </remarks>
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

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                if (report.Status != ReportStatus.Todo && report.Status != ReportStatus.Pending)
                {
                    return Forbid(0, "Resource modification forbidden with current status");
                }

                // Report cannot be altered anymore
                report.Status = ReportStatus.Done;

                await _reportRepository.UpdateAsync(report);

                return NoContent();
            }

            return ResourceForbid();
        }

        /// <summary>
        /// Report verification input model.
        /// </summary>
        public sealed class VerificationInputModel
        {
            /// <summary>
            /// Posible verification results.
            /// </summary>
            public enum VerificationResult
            {
                /// <summary>
                /// Report is in order and can be marked as verified.
                /// </summary>
                Verified,

                /// <summary>
                /// Report is rejected.
                /// </summary>
                Rejected,
            }

            /// <summary>
            /// Verification result.
            /// </summary>
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

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Validate);
            if (authorizationResult.Succeeded)
            {
                if (report.Status != ReportStatus.Done)
                {
                    return Forbid(0, "Resource modification forbidden with current status");
                }

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
                // TODO: Remove only when empty

                await _reportRepository.DeleteAsync(report);

                return NoContent();
            }

            return ResourceForbid();
        }
    }
}
