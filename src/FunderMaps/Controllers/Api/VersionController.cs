using FunderMaps.Helpers;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Return application versioning information.
    /// </summary>
    [AllowAnonymous]
    [Route("api/version")]
    [ApiController]
    public class VersionController : BaseApiController
    {
        // GET: api/version
        /// <summary>
        /// Return application versioning information.
        /// </summary>
        /// <remarks>Cache response for 24 hours.</remarks>
        [HttpGet]
        [ResponseCache(Duration = 60 * 60 * 24)]
        [ProducesResponseType(typeof(ApplicationVersionModel), StatusCodes.Status200OK)]
        public IActionResult Get()
            => Ok(new ApplicationVersionModel
            {
                Name = Constants.ApplicationName,
                Version = Constants.ApplicationVersion,
                VersionString = Constants.ApplicationVersion.ToString(),
            });
    }
}
