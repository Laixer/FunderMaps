using System;
using System.Collections.Generic;
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

namespace FunderMaps.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
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
                return NotFound();
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

        private Report Map(InputModel input)
        {
            return new Report
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
            };
        }

        // GET: api/report/{id}/{document}
        [HttpGet("{id}/{document}")]
        public async Task<IActionResult> Get(int id, string document)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var organization = await _context.OrganizationUsers
                .Include(s => s.Organization)
                .Select(s => new { s.UserId, s.Organization.AttestationOrganizationId })
                .SingleOrDefaultAsync(q => q.UserId == user.Id);

            var report = await _fisContext.Report.FindAsync(id, document);
            if (report == null)
            {
                return NotFound();
            }

            //var authorizationResult = await _authorizationService.AuthorizeAsync(User, report, "OrganizationMemberPolicy");
            //if (authorizationResult.Succeeded)
            //{
            return Ok(report);
            //}

            return Forbid();
        }

        // POST: api/report
        [HttpPost]
        public async Task<Report> PostAsync([FromBody] InputModel input)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var organization = await _context.OrganizationUsers
                .Include(s => s.Organization)
                .Select(s => new { s.UserId, s.Organization.AttestationOrganizationId })
                .SingleOrDefaultAsync(q => q.UserId == user.Id);

            var report = Map(input);

            report.Creator = user.AttestationPrincipalId;
            report.Owner = organization.AttestationOrganizationId;

            await _fisContext.Report.AddAsync(report);
            await _fisContext.SaveChangesAsync();

            return report;
        }

        // TODO: Upload files via form
        // POST: api/report/attach_document
        [HttpPost("attach_document")]
        public async Task<IActionResult> AttachDocument()
        {
            if (Request.ContentLength == 0)
            {
                return BadRequest();
            }

            // Store the report
            await _fileStorageService.StoreFileAsync("report", "kaas.pak", Request.Body);

            return Ok();
        }

        // TODO:
        // PUT: api/report/{id}/{document}
        [HttpPut("{id}/{document}")]
        public async Task<IActionResult> PutAsync(int id, string document, [FromBody] Report report)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var organization = await _context.OrganizationUsers
                .Include(s => s.Organization)
                .Select(s => new { s.UserId, s.Organization.AttestationOrganizationId })
                .SingleOrDefaultAsync(q => q.UserId == user.Id);

            if (id != report.Id || document != report.DocumentId)
            {
                return BadRequest();
            }

            _fisContext.Report.Update(report);
            await _fisContext.SaveChangesAsync();

            return NoContent();
        }

        // TODO:
        // DELETE: api/report/{id}/{document}
        [HttpDelete("{id}/{document}")]
        public async Task<IActionResult> DeleteAsync(int id, string document)
        {
            var report = await _fisContext.Report.FindAsync(id, document);
            if (report == null)
            {
                return NotFound();
            }

            _fisContext.Report.Remove(report);
            await _fisContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
