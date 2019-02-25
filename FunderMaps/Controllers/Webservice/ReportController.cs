using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Interfaces;
using FunderMaps.Models.Fis;
using FunderMaps.Models.Identity;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Data.Authorization;

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

        public ReportController(
            FisDbContext fixContext,
            FunderMapsDbContext context,
            UserManager<FunderMapsUser> userManager,
            IAuthorizationService authorizationService,
            IFileStorageService fileStorageService,
            IReportService reportService)
        {
            _fisContext = fixContext;
            _context = context;
            _userManager = userManager;
            _fileStorageService = fileStorageService;
            _authorizationService = authorizationService;
            _reportService = reportService;
        }

        // GET: api/report
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] int? offset = null, [FromQuery] int limit = 100)
        {
            var attestationOrganizationId = User.Claims.Where(s => s.Type == FisClaimTypes.OrganizationAttestationIdentifier).First();

            var query = _fisContext.Report
                .AsNoTracking()
                .Include(s => s.TypeNavigation)
                .Include(s => s.OwnerNavigation)
                .Take(limit);
            //.WhereAccessLevel(AccessPolicy.Public);

            if (attestationOrganizationId == null)
            {
                query = query.Where(s => s.AccessPolicy == AccessPolicy.Public);
            }
            else
            {
                query = query.Where(s => s.Owner == int.Parse(attestationOrganizationId.Value) || s.AccessPolicy == AccessPolicy.Public);
            }

            var query2 = query.Select(s => new
            {
                s.Id,
                s.DocumentId,
                s.Inspection,
                s.JointMeasurement,
                s.FloorMeasurement,
                s.ConformF3o,
                s.CreateDate,
                s.UpdateDate,
                s.Note,
                s.Status,
                s.Type,
                TypeNavigationNameNl = s.TypeNavigation.NameNl
            });

            if (offset.HasValue)
            {
                query2 = query2.Skip(offset.Value);
            }

            return Ok(await query2.ToListAsync());
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
            var attestationPrincipalId = User.Claims.Where(s => s.Type == FisClaimTypes.UserAttestationIdentifier).First();
            var attestationOrganizationId = User.Claims.Where(s => s.Type == FisClaimTypes.OrganizationAttestationIdentifier).First();

            if (attestationPrincipalId == null || attestationOrganizationId == null)
            {
                return ResourceForbid();
            }

            var report = new Report
            {
                Project = input.Project,
                DocumentId = input.DocumentId,
                Inspection = input.Inspection,
                JointMeasurement = input.JointMeasurement,
                FloorMeasurement = input.FloorMeasurement,
                ConformF3o = input.ConformF3o,
                Note = input.Note,
                Status = "todo", // NOTE: HACK: The default value for database does not work, hence the default here
                Type = input.Type ?? "unknown", // NOTE: HACK: The default value for database does not work, hence the default here
                Reviewer = input.Reviewer,
                Contractor = input.Contractor,
                DocumentDate = input.DocumentDate,
                Creator = int.Parse(attestationPrincipalId.Value),
                Owner = int.Parse(attestationOrganizationId.Value),
                AccessPolicy = AccessPolicy.Private, // NOTE: HACK: The default value for database does not work, hence the default here
            };

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report, OperationsRequirement.Create);
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
            var report = await _fisContext.Report.FindAsync(id, document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            // Public data is accessible to anyone
            if (report.IsPublic())
            {
                return Ok(report);
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report, OperationsRequirement.Read);
            if (authorizationResult.Succeeded)
            {
                return Ok(report);
            }

            return ResourceForbid();
        }

        // TODO: Authorization
        // TODO: Upload files via form
        // POST: api/report/attach_document
        [HttpPost("attach_document")]
        public async Task<IActionResult> AttachDocument()
        {
            if (Request.ContentLength == 0)
            {
                return BadRequest(0, "Content is empty");
            }

            // Store the report
            await _fileStorageService.StoreFileAsync("report", "kaas.pak", Request.Body);

            return NoContent();
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
            var report = await _fisContext.Report.FindAsync(id, document);
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
            report.ConformF3o = input.ConformF3o;
            report.Note = input.Note;
            report.AccessPolicy = input.AccessPolicy;

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report, OperationsRequirement.Update);
            if (authorizationResult.Succeeded)
            {
                _fisContext.Report.Update(report);
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
            var report = await _fisContext.Report.FindAsync(id, document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report, OperationsRequirement.Delete);
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
