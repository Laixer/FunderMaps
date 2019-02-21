using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FunderMaps.Data;
using FunderMaps.Interfaces;
using FunderMaps.Models.Fis;
using FunderMaps.Models.Identity;

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
        [HttpGet]
        public IEnumerable<string> Get()
        {
            // TODO: ...

            return new string[] { "value1", "value2" };
        }

        // GET: api/sample/{id}/{report}
        [HttpGet("{id}/{report}")]
        public async Task<Sample> GetAsync(int id, int report)
        {
            return await _fisContext.Sample.FindAsync(id, report);
        }

        // POST: api/sample
        [HttpPost]
        public async Task<Sample> PostAsync([FromBody] Sample sample)
        {
            await _fisContext.Sample.AddAsync(sample);
            await _fisContext.SaveChangesAsync();

            return sample;
        }

        // PUT: api/sample/{id}/{report}
        [HttpPut("{id}/{report}")]
        public async Task<IActionResult> PutAsync(int id, int report, [FromBody] Sample sample)
        {
            if (id != sample.Id || report != sample.Report)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            _fisContext.Sample.Update(sample);
            await _fisContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/sample/{id}/{report}
        [HttpDelete("{id}/{report}")]
        public async Task<IActionResult> DeleteAsync(int id, int report)
        {
            var sample = await _fisContext.Sample.FindAsync(id, report);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            _fisContext.Sample.Remove(sample);
            await _fisContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
