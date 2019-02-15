using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FunderMaps.Data;
using FunderMaps.Models.Fis;

namespace FunderMaps.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly FisDbContext _context;

        public SampleController(FisDbContext context)
        {
            _context = context;
        }

        // GET: api/sample
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/sample/{id}/{report}
        [HttpGet("{id}/{report}")]
        public async Task<Sample> Get(int id, int report)
        {
            return await _context.Sample.FindAsync(id, report);
        }

        // POST: api/sample
        [HttpPost]
        public async Task<Sample> Post([FromBody] Sample sample)
        {
            _context.Sample.Add(sample);
            await _context.SaveChangesAsync();

            return sample;
        }

        // PUT: api/sample
        [HttpPut]
        public async Task<Sample> Put([FromBody] Sample sample)
        {
            _context.Sample.Update(sample);
            await _context.SaveChangesAsync();

            return sample;
        }

        // DELETE: api/sample/{id}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
