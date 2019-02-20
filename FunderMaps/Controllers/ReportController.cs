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
        private readonly IReportService _reportService;

        public ReportController(
            FisDbContext fixContext,
            FunderMapsDbContext context,
            UserManager<FunderMapsUser> userManager,
            IFileStorageService fileStorageService,
            IReportService reportService)
        {
            _fisContext = fixContext;
            _context = context;
            _userManager = userManager;
            _fileStorageService = fileStorageService;
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
            public string Status { get; set; }
            public string Type { get; set; }
            public int? Reviewer { get; set; }
            public int? Creator { get; set; }

            [Required]
            public int? Owner { get; set; }

            [Required]
            public int? Contractor { get; set; }

            public DateTime DocumentDate { get; set; }
            public string DocumentName { get; set; }
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
                Status = input.Status,
                Type = input.Type,
                Reviewer = input.Reviewer,
                Creator = input.Creator,
                Owner = input.Owner.Value,
                Contractor = input.Contractor.Value,
                DocumentDate = input.DocumentDate,
                DocumentName = input.DocumentName
            };
        }

        // GET: api/report/{id}/{document}
        [HttpGet("{id}/{document}")]
        public async Task<Report> Get(int id, string document)
        {
            return await _fisContext.Report.FindAsync(id, document);
        }

        // POST: api/report
        [HttpPost]
        public async Task<Report> PostAsync([FromBody] InputModel input)
        {
            var report = Map(input);

            _fisContext.Report.Add(report);
            await _fisContext.SaveChangesAsync();

            return report;
        }

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

        // PUT: api/report
        [HttpPut]
        public async Task<Report> PutAsync([FromBody] Report report)
        {
            report.DeleteDate = DateTime.Now;

            _fisContext.Report.Update(report);
            await _fisContext.SaveChangesAsync();

            return report;
        }

        // DELETE: api/report
        [HttpDelete]
        public async Task DeleteAsync([FromBody] Report report)
        {
            report.DeleteDate = DateTime.Now;

            _fisContext.Report.Update(report);
            await _fisContext.SaveChangesAsync();
        }
    }
}
