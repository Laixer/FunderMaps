using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/user
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/user
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
