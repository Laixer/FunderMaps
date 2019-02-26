using System.Linq;
using System.Threading.Tasks;
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

namespace FunderMaps.Controllers.Webservice
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : AbstractMicroController
    {
        private readonly FisDbContext _fisContext;
        private readonly FunderMapsDbContext _context;
        private readonly UserManager<FunderMapsUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IReportService _reportService;

        public SampleController(
            FisDbContext fixContext,
            FunderMapsDbContext context,
            UserManager<FunderMapsUser> userManager,
            IAuthorizationService authorizationService,
            IReportService reportService)
        {
            _fisContext = fixContext;
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _reportService = reportService;
        }

        // GET: api/sample
        /// <summary>
        /// Get a chunk of samples either by organization or as public data.
        /// </summary>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of samples.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int offset = 0, [FromQuery] int limit = 25)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // FUTURE: The 'where' could be improved
            var samples = await _fisContext.Sample
                .AsNoTracking()
                .Include(s => s.ReportNavigation)
                .Where(s => attestationOrganizationId == null
                    ? s.AccessPolicy == AccessPolicy.Public
                    : s.ReportNavigation.Owner == int.Parse(attestationOrganizationId) || s.AccessPolicy == AccessPolicy.Public)
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
        public async Task<IActionResult> PostAsync([FromBody] Sample input)
        {
            var report = await _fisContext.Report
                .SingleAsync(s => s.Id == input.Report);
            if (report == null)
            {
                return ResourceNotFound();
            }

            var sample = input;
            sample.FoundationDamageCause = "unknown"; // NOTE: HACK: The default value for database does not work, hence the default here
            sample.AccessPolicy = AccessPolicy.Private; // NOTE: HACK: The default value for database does not work, hence the default here

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                await _fisContext.Sample.AddAsync(sample);
                await _fisContext.SaveChangesAsync();

                return Ok(sample);
            }

            return ResourceForbid();
        }

        // GET: api/sample/{id}/{report}
        /// <summary>
        /// Retrieve the sample by identifier. The sample is returned
        /// if the the record is public or if the organization user has
        /// access to the record.
        /// </summary>
        /// <param name="id">Report identifier.</param>
        /// <param name="document">Report identifier.</param>
        /// <returns>Report.</returns>
        [HttpGet("{id}/{report}")]
        public async Task<IActionResult> GetAsync(int id, int report)
        {
            var sample = await _fisContext.Sample
                .AsNoTracking()
                .Include(s => s.ReportNavigation)
                .SingleOrDefaultAsync(s => s.Id == id && s.Report == report);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            // Public data is accessible to anyone
            if (sample.IsPublic() && sample.ReportNavigation.IsPublic())
            {
                return Ok(sample);
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation, OperationsRequirement.Read);
            if (authorizationResult.Succeeded)
            {
                return Ok(sample);
            }

            return ResourceForbid();
        }

        // PUT: api/sample/{id}/{report}
        /// <summary>
        /// Update sample if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Sample identifier.</param>
        /// <param name="document">Sample identifier.</param>
        /// <param name="input">Sample data.</param>
        [HttpPut("{id}/{report}")]
        public async Task<IActionResult> PutAsync(int id, int report, [FromBody] Sample input)
        {
            var sample = await _fisContext.Sample
                .AsNoTracking()
                .Include(s => s.ReportNavigation)
                .SingleOrDefaultAsync(s => s.Id == id && s.Report == report);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            if (id != input.Id || report != input.Report)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            sample.StreetName = input.StreetName;
            sample.BuildingNumber = input.BuildingNumber;
            sample.BuildingNumberSuffix = input.BuildingNumberSuffix;
            sample.MonitoringWell = input.MonitoringWell;
            sample.Cpt = input.Cpt;
            sample.WoodLevel = input.WoodLevel;
            sample.GroudLevel = input.GroudLevel;
            sample.GroundwaterLevel = input.GroundwaterLevel;
            sample.BuiltYear = input.BuiltYear;
            sample.Note = input.Note;
            sample.AccessPolicy = input.AccessPolicy;

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation, OperationsRequirement.Update);
            if (authorizationResult.Succeeded)
            {
                _fisContext.Sample.Update(sample);
                await _fisContext.SaveChangesAsync();

                return NoContent();
            }

            return ResourceForbid();
        }

        // DELETE: api/sample/{id}/{report}
        /// <summary>
        /// Soft delete the sample if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Sample identifier.</param>
        /// <param name="document">Sample identifier.</param>
        [HttpDelete("{id}/{report}")]
        public async Task<IActionResult> DeleteAsync(int id, int report)
        {
            var sample = await _fisContext.Sample
                .AsNoTracking()
                .Include(s => s.ReportNavigation)
                .SingleOrDefaultAsync(s => s.Id == id && s.Report == report);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation, OperationsRequirement.Delete);
            if (authorizationResult.Succeeded)
            {
                _fisContext.Sample.Remove(sample);
                await _fisContext.SaveChangesAsync();

                return NoContent();
            }

            return ResourceForbid();
        }
    }
}
