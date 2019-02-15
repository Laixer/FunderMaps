using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Models.Fis;

namespace FunderMaps.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly FisDbContext _context;

        public ReportController(FisDbContext context)
        {
            _context = context;
        }

        // GET: api/report
        [HttpGet]
        public async Task<IEnumerable<Report>> Get([FromQuery] uint? offset = null, [FromQuery] uint? limit = null)
        {
            var query = _context.Report
                .AsNoTracking()
                .AsQueryable();

            if (offset.HasValue)
            {
                query = query.Skip((int)offset.Value);
            }
            if (limit.HasValue)
            {
                query = query.Take((int)limit.Value);
            }

            return await query.ToListAsync();
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
            return await _context.Report.FindAsync(id, document);
        }

        // POST: api/report
        [HttpPost]
        public async Task<Report> Post([FromBody] InputModel input)
        {
            var report = Map(input);

            _context.Report.Add(report);
            await _context.SaveChangesAsync();

            return report;
        }

        // POST: api/report/attach_document
        [HttpPost("attach_document"), DisableRequestSizeLimit]
        public async Task AttachDocument([FromBody] Report report)
        {
            await _context.SaveChangesAsync();
        }

        // PUT: api/report
        [HttpPut]
        public async Task<Report> Put([FromBody] Report report)
        {
            report.DeleteDate = DateTime.Now;

            _context.Report.Update(report);
            await _context.SaveChangesAsync();

            return report;
        }

        // DELETE: api/report
        [HttpDelete]
        public async Task Delete([FromBody] Report report)
        {
            report.DeleteDate = DateTime.Now;

            _context.Report.Update(report);
            await _context.SaveChangesAsync();
        }
    }
}
