using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Interfaces;
using FunderMaps.Models.Fis;
using FunderMaps.Models.Identity;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Extensions;
using FunderMaps.Data.Authorization;
using FunderMaps.Models;

namespace FunderMaps.Controllers.Webservice
{
    [Authorize]
    [Route("api/sample")]
    [ApiController]
    public class SampleController : BaseApiController
    {
        private readonly FisDbContext _fisContext;
        private readonly FunderMapsDbContext _context;
        private readonly UserManager<FunderMapsUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        //private readonly IReportService _reportService;
        private readonly ISampleRepository _sampleRepository;

        public SampleController(
            FisDbContext fisContext,
            FunderMapsDbContext context,
            UserManager<FunderMapsUser> userManager,
            IAuthorizationService authorizationService,
            IReportService reportService,
            ISampleRepository sampleRepository)
        {
            _fisContext = fisContext;
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
            //_reportService = reportService;
            _sampleRepository = sampleRepository;
        }

        // GET: api/sample
        /// <summary>
        /// Get a chunk of samples either by organization or as public data.
        /// </summary>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of samples.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Sample>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int offset = 0, [FromQuery] int limit = 25)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // TODO: attestationOrganizationId can be null
            // TODO: administrator can query anything

            // FUTURE: The 'where' could be improved
            var samples = await _fisContext.Sample
                .AsNoTracking()
                .Include(s => s.ReportNavigation)
                    .ThenInclude(si => si.Attribution)
                .Include(s => s.AccessPolicy)
                .Where(s => s.ReportNavigation.Attribution._Owner == int.Parse(attestationOrganizationId) || s._AccessPolicy == AccessControl.Public)
                .OrderByDescending(s => s.CreateDate)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return Ok(samples);
        }

        // POST: api/sample
        /// <summary>
        /// Create a new sample for a report.
        /// </summary>
        /// <param name="input">Sample data.</param>
        /// <returns>Report.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Sample), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PostAsync([FromBody] Sample input)
        {
            var report = await _fisContext.Report
                .Include(s => s.Attribution)
                .Include(s => s.Status)
                .FirstOrDefaultAsync(s => s.Id == input.Report);
            if (report == null)
            {
                return ResourceNotFound();
            }

            // Check if we can add new samples to the report
            if (!report.CanHaveNewSamples())
            {
                return Forbid(0, "Resource modification forbidden with current status");
            }

            var sample = new Sample
            {
                ReportNavigation = report,
                MonitoringWell = input.MonitoringWell,
                Cpt = input.Cpt,
                Note = input.Note,
                WoodLevel = input.WoodLevel,
                GroundwaterLevel = input.GroundwaterLevel,
                GroundLevel = input.GroundLevel,
                FoundationRecoveryAdviced = input.FoundationRecoveryAdviced,
                BuiltYear = input.BuiltYear,
                Address = input.Address,
                BaseMeasurementLevel = await _fisContext.BaseLevel.FindAsync("NAP"),
                FoundationDamageCause = await _fisContext.FoundationDamageCause.FindAsync(input.FoundationDamageCause != null ? input.FoundationDamageCause.Id : "unknown"),
                AccessPolicy = await _fisContext.AccessPolicy.FindAsync(AccessControl.Private),
            };

            // Set the report status to 'pending'
            if (report.Status.Id != "pending")
            {
                report.Status = await _fisContext.ReportStatus.FindAsync("pending");
            }

            if (input.EnforcementTerm != null)
            {
                sample.EnforcementTerm = await _fisContext.EnforcementTerm.FindAsync(input.EnforcementTerm.Id);
            }
            if (input.FoundationQuality != null)
            {
                sample.FoundationQuality = await _fisContext.FoundationQuality.FindAsync(input.FoundationQuality.Id);
            }
            if (input.FoundationType != null)
            {
                sample.FoundationType = await _fisContext.FoundationType.FindAsync(input.FoundationType.Id);
            }
            if (input.Substructure != null)
            {
                sample.Substructure = await _fisContext.Substructure.FindAsync(input.Substructure.Id);
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation.Attribution._Owner, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                await _fisContext.Sample.AddAsync(sample);
                await _fisContext.SaveChangesAsync();

                return Ok(sample);
            }

            return ResourceForbid();
        }

        // GET: api/sample/{id}
        /// <summary>
        /// Retrieve the sample by identifier. The sample is returned
        /// if the the record is public or if the organization user has
        /// access to the record.
        /// </summary>
        /// <param name="id">Report identifier.</param>
        /// <returns>Report.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Sample), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var sample = await _sampleRepository.GetByIdWithItemsAsync(id);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            // Public data is accessible to anyone
            if (sample.IsPublic() && sample.ReportNavigation.IsPublic())
            {
                return Ok(sample);
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation.Attribution._Owner, OperationsRequirement.Read);
            if (authorizationResult.Succeeded)
            {
                return Ok(sample);
            }

            return ResourceForbid();
        }

        // PUT: api/sample/{id}
        /// <summary>
        /// Update sample if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Sample identifier.</param>
        /// <param name="input">Sample data.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Sample input)
        {
            var sample = await _sampleRepository.GetByIdAsync(id);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            if (id != input.Id)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            sample.MonitoringWell = input.MonitoringWell;
            sample.Cpt = input.Cpt;
            sample.WoodLevel = input.WoodLevel;
            sample.GroundLevel = input.GroundLevel;
            sample.GroundwaterLevel = input.GroundwaterLevel;
            sample.BuiltYear = input.BuiltYear;
            sample.Note = input.Note;

            if (input.EnforcementTerm != null)
            {
                sample.EnforcementTerm = await _fisContext.EnforcementTerm.FindAsync(input.EnforcementTerm.Id);
            }
            if (input.FoundationQuality != null)
            {
                sample.FoundationQuality = await _fisContext.FoundationQuality.FindAsync(input.FoundationQuality.Id);
            }
            if (input.FoundationType != null)
            {
                sample.FoundationType = await _fisContext.FoundationType.FindAsync(input.FoundationType.Id);
            }
            if (input.Substructure != null)
            {
                sample.Substructure = await _fisContext.Substructure.FindAsync(input.Substructure.Id);
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation.Attribution._Owner, OperationsRequirement.Update);
            if (authorizationResult.Succeeded)
            {
                await _sampleRepository.UpdateAsync(sample);

                return NoContent();
            }

            return ResourceForbid();
        }

        // DELETE: api/sample/{id}
        /// <summary>
        /// Soft delete the sample if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Sample identifier.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var sample = await _sampleRepository.GetByIdAsync(id);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation.Attribution._Owner, OperationsRequirement.Delete);
            if (authorizationResult.Succeeded)
            {
                await _sampleRepository.DeleteAsync(sample);

                return NoContent();
            }

            return ResourceForbid();
        }
    }
}
