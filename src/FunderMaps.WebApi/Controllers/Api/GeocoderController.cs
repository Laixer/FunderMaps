using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Geocoder endpoint.
    /// </summary>
    [Authorize]
    [Route("api/geocoder")]
    [ApiController]
    public class GeocoderController : BaseApiController
    {
        // GET: api/geocoder/suggest
        /// <summary>
        /// Return addresses by suggestion query.
        /// </summary>
        /// <param name="query">Search query.</param>
        [HttpGet("suggest")]
        public async Task<IActionResult> GetSuggestionsAsync(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest();
            }

            await Task.CompletedTask;

            throw new NotImplementedException();
        }
    }
}
