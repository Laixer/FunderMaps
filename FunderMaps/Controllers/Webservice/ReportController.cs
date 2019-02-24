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
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var organization = await _context.OrganizationUsers
                .Include(s => s.Organization)
                .Select(s => new { s.UserId, s.Organization.AttestationOrganizationId })
                .SingleOrDefaultAsync(q => q.UserId == user.Id);

            if (organization == null || organization.AttestationOrganizationId == 0)
            {
                return ResourceNotFound();
            }

            var query = _fisContext.Report
                .AsNoTracking()
                .Include(s => s.TypeNavigation)
                .Include(s => s.OwnerNavigation)
                .Where(s => s.Owner == organization.AttestationOrganizationId)
                .Select(s => new
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
                })
                .Take(limit);

            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            return Ok(await query.ToListAsync());
        }

        public sealed class InputModel
        {
            public int? Project { get; set; }

            [Required]
            public string DocumentId { get; set; }

            public bool Inspection { get; set; }
            public bool JointMeasurement { get; set; }
            public bool FloorMeasurement { get; set; }
            public bool ConformF3o { get; set; }
            public string Note { get; set; }
            public string Type { get; set; }
            public int? Reviewer { get; set; }

            [Required]
            public int? Contractor { get; set; }

            [Required]
            public DateTime DocumentDate { get; set; }
        }

        // GET: api/report/{id}/{document}
        [HttpGet("{id}/{document}")]
        public async Task<IActionResult> GetAsync(int id, string document)
        {
            var report = await _fisContext.Report.FindAsync(id, document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            // Public data is accessible to any clients
            if (report.IsPublic())
            {
                return Ok(report);
            }

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var organizationUser = await _context.OrganizationUsers
                .Include(s => s.Organization)
                .Select(s => new { s.UserId, s.Organization })
                .SingleOrDefaultAsync(q => q.UserId == user.Id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organizationUser.Organization, OperationsRequirement.Read);
            if (authorizationResult.Succeeded)
            {
                return Ok(report);
            }

            return ResourceForbid();
        }

        // POST: api/report
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] InputModel input)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var organizationUser = await _context.OrganizationUsers
                .Include(s => s.Organization)
                .Select(s => new { s.UserId, s.Organization })
                .SingleOrDefaultAsync(q => q.UserId == user.Id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organizationUser.Organization, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                var report = new Report
                {
                    Project = input.Project,
                    DocumentId = input.DocumentId,
                    Inspection = input.Inspection,
                    JointMeasurement = input.JointMeasurement,
                    FloorMeasurement = input.FloorMeasurement,
                    ConformF3o = input.ConformF3o,
                    Note = input.Note,
                    Status = "todo",
                    Type = input.Type ?? "unknown",
                    Reviewer = input.Reviewer,
                    Contractor = input.Contractor.Value,
                    DocumentDate = input.DocumentDate,
                    Creator = user.AttestationPrincipalId,
                    Owner = organizationUser.Organization.AttestationOrganizationId,
                };

                await _fisContext.Report.AddAsync(report);
                await _fisContext.SaveChangesAsync();

                return NoContent();
            }

            return ResourceForbid();
        }

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
        [HttpPut("{id}/{document}")]
        public async Task<IActionResult> PutAsync(int id, string document, [FromBody] Report report)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var organizationUser = await _context.OrganizationUsers
                .Include(s => s.Organization)
                .Select(s => new { s.UserId, s.Organization })
                .SingleOrDefaultAsync(q => q.UserId == user.Id);

            if (id != report.Id || document != report.DocumentId)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organizationUser.Organization, OperationsRequirement.Update);
            if (authorizationResult.Succeeded)
            {
                _fisContext.Report.Update(report);
                await _fisContext.SaveChangesAsync();

                return NoContent();
            }

            return ResourceForbid();
        }

        // DELETE: api/report/{id}/{document}
        [HttpDelete("{id}/{document}")]
        public async Task<IActionResult> DeleteAsync(int id, string document)
        {
            var report = await _fisContext.Report.FindAsync(id, document);
            if (report == null)
            {
                return ResourceNotFound();
            }

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var organizationUser = await _context.OrganizationUsers
                .Include(s => s.Organization)
                .Select(s => new { s.UserId, s.Organization })
                .SingleOrDefaultAsync(q => q.UserId == user.Id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, organizationUser.Organization, OperationsRequirement.Delete);
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
