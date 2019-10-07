using FunderMaps.Helpers;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet]
        [ResponseCache(Duration = 24 * 60 * 60)]
        [ProducesResponseType(typeof(ApplicationVersionModel), 200)]
        public IActionResult Get()
            => Ok(new ApplicationVersionModel
            {
                Name = Constants.ApplicationName,
                Version = Constants.ApplicationVersion,
                VersionString = Constants.ApplicationVersion.ToString(),
            });
    }
}
