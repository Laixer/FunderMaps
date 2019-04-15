using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Interfaces;
using FunderMaps.Models.Fis;
using FunderMaps.Models.Identity;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Data.Authorization;
using FunderMaps.Extensions;
using FunderMaps.Helpers;

namespace FunderMaps.Controllers.Webservice
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : AbstractMicroController
    {
        private readonly FisDbContext _fisContext;
        private readonly FunderMapsDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly UserManager<FunderMapsUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IReportService _reportService;

        private static readonly string[] allowedReportFileTypes =
        {
            "application/pdf",
        };

        public ReportController(
            FisDbContext fisContext,
            FunderMapsDbContext context,
            UserManager<FunderMapsUser> userManager,
            IAuthorizationService authorizationService,
            IFileStorageService fileStorageService,
            IReportService reportService)
        {
            _fisContext = fisContext;
            _context = context;
            _userManager = userManager;
            _fileStorageService = fileStorageService;
            _authorizationService = authorizationService;
            _reportService = reportService;
        }

        // GET: api/report
        /// <summary>
        /// Get a chunk of reports either by organization or as public data.
        /// </summary>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of reports.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int offset = 0, [FromQuery] int limit = 25)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // FUTURE: The 'where' could be improved
            var reports = await _fisContext.Report
                .AsNoTracking()
                .Include(s => s.Attribution)
                    .ThenInclude(si => si.Reviewer)
                .Include(s => s.Attribution)
                    .ThenInclude(si => si.Contractor)
                .Include(s => s.Attribution)
                    .ThenInclude(si => si.Creator)
                .Include(s => s.Attribution)
                    .ThenInclude(si => si.Owner)
                .Include(s => s.Type)
                .Include(s => s.Status)
                .Include(s => s.Norm)
                .Include(s => s.AccessPolicy)
                .Where(s => s.Attribution._Owner == int.Parse(attestationOrganizationId) || s._AccessPolicy == AccessControl.Public)
                .OrderByDescending(s => s.CreateDate)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return Ok(reports);
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
        public async Task<IActionResult> GetStatsAsync()
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            long count = await _fisContext.Report
                .AsNoTracking()
                .Include(s => s.Attribution)
                .Where(s => s.Attribution._Owner == int.Parse(attestationOrganizationId) || s._AccessPolicy == AccessControl.Public)
                .CountAsync();

            return Ok(new EntityStatsOutputModel
            {
                Count = count,
            });
        }

        // POST: api/report
        /// <summary>
        /// Create a new report.
        /// </summary>
        /// <param name="input">Report data.</param>
        /// <returns>Report.</returns>
        [HttpPost]
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
                Status = await _fisContext.ReportStatus.FindAsync("todo"),
                Type = await _fisContext.ReportType.FindAsync(input.Type != null ? input.Type.Id : "unknown"),
                DocumentDate = input.DocumentDate,
                Attribution = new Attribution
                {
                    Project = input.Attribution.Project,
                    Reviewer = await _fisContext.Principal.GetOrAddAsync(input.Attribution.Reviewer, s => s.NickName == input.Attribution.Reviewer.NickName || s.Email == input.Attribution.Reviewer.Email),
                    Contractor = await _fisContext.Organization.GetOrAddAsync(input.Attribution.Contractor, s => s.Name == input.Attribution.Contractor.Name),
                    Creator = await _fisContext.Principal.FindAsync(int.Parse(attestationPrincipalId)),
                    Owner = await _fisContext.Organization.FindAsync(int.Parse(attestationOrganizationId)),
                },
                AccessPolicy = await _fisContext.AccessPolicy.FindAsync(AccessControl.Private),
            };

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution.Owner.Id, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                await _fisContext.Report.AddAsync(report);
                await _fisContext.SaveChangesAsync();

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
        public async Task<IActionResult> GetAsync(int id, string document)
        {
            var report = await _fisContext.Report
                .AsNoTracking()
                .Include(s => s.Attribution)
                    .ThenInclude(si => si.Reviewer)
                .Include(s => s.Attribution)
                    .ThenInclude(si => si.Contractor)
                .Include(s => s.Attribution)
                    .ThenInclude(si => si.Creator)
                .Include(s => s.Attribution)
                    .ThenInclude(si => si.Owner)
                .Include(s => s.Type)
                .Include(s => s.Status)
                .Include(s => s.Norm)
                .Include(s => s.AccessPolicy)
                .FirstOrDefaultAsync(s => s.Id == id && s.DocumentId == document);
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

        // POST: api/report/attach_document
        [HttpPost("attach_document")]
        public async Task<IActionResult> AttachDocument(IFormFile file)
        {
            var reportFile = new ApplicationFile(file);

            if (!allowedReportFileTypes.Contains(reportFile.ContentType))
            {
                return BadRequest(0, "file content type is not allowed");
            }

            if (reportFile.Empty())
            {
                return BadRequest(0, "file content is empty");
            }

            // Store the report
            await _fileStorageService.StoreFileAsync("report", reportFile, file.OpenReadStream());

            return Ok(reportFile);
        }

        // PUT: api/report/{id}/{document}
        /// <summary>
        /// Update report if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Report identifier.</param>
        /// <param name="document">Report identifier.</param>
        /// <param name="input">Report data.</param>
        [HttpPut("{id}/{document}")]
        public async Task<IActionResult> PutAsync(int id, string document, [FromBody] Report input)
        {
            var report = await _fisContext.Report
                .Include(s => s.Attribution)
                .FirstOrDefaultAsync(s => s.Id == id && s.DocumentId == document);
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

            _fisContext.Entry(report.Norm).State = EntityState.Modified;

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Update);
            if (authorizationResult.Succeeded)
            {
                _fisContext.Report.Update(report);
                await _fisContext.SaveChangesAsync();

                return NoContent();
            }

            return ResourceForbid();
        }

        // PUT: api/report/{id}/{document}/done
        /// <summary>
        /// Mark the report for as done and prepare for verification.
        /// </summary>
        /// <param name="id">Report identifier.</param>
        /// <param name="document">Report identifier.</param>
        [HttpPut("{id}/{document}/done")]
        public async Task<IActionResult> PutSignalStatusDoneAsync(int id, string document)
        {
            var report = await _fisContext.Report
                .Include(s => s.Attribution)
                .Include(s => s.Status)
                .FirstOrDefaultAsync(s => s.Id == id && s.DocumentId == document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            if (!report.CanHaveNewSamples())
            {
                return Forbid(0, "Resource modification forbidden with current status");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                report.Status = await _fisContext.ReportStatus.FindAsync("done");
                await _fisContext.SaveChangesAsync();

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
        public async Task<IActionResult> PutValidateRequestAsync(int id, string document, [FromBody] VerificationInputModel input)
        {
            var report = await _fisContext.Report
                .Include(s => s.Attribution)
                .Include(s => s.Status)
                .FirstOrDefaultAsync(s => s.Id == id && s.DocumentId == document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            if (report.Status.Id != "done")
            {
                return Forbid(0, "Resource modification forbidden with current status");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Validate);
            if (authorizationResult.Succeeded)
            {
                switch (input.Result)
                {
                    case VerificationInputModel.VerificationResult.Verified:
                        report.Status = await _fisContext.ReportStatus.FindAsync("verified");
                        break;
                    case VerificationInputModel.VerificationResult.Rejected:
                        report.Status = await _fisContext.ReportStatus.FindAsync("rejected");
                        break;
                }

                await _fisContext.SaveChangesAsync();

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
        public async Task<IActionResult> DeleteAsync(int id, string document)
        {
            var report = await _fisContext.Report
                .Include(s => s.Attribution)
                .FirstOrDefaultAsync(s => s.Id == id && s.DocumentId == document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Delete);
            if (authorizationResult.Succeeded)
            {
                // TODO: Only allow removal if there a no samples.

                _fisContext.Report.Remove(report);
                await _fisContext.SaveChangesAsync();

                return NoContent();
            }

            return ResourceForbid();
        }
    }
}
