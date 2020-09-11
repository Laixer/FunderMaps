using FunderMaps.AspNetCore.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FunderMaps.Webservice.Controllers
{
    /// <summary>
    ///  Version controller.
    /// </summary>
    [AllowAnonymous]
    public class VersionController : ControllerBase
    {
        /// <summary>
        ///     Return application versioning information.
        /// </summary>
        /// <remarks>
        ///     Cache response for 24 hours. Version will not change
        ///     often and this call is primarily used to check if the
        ///     API is responding.
        /// </remarks>
        [HttpGet("version"), ResponseCache(Duration = 60 * 60 * 24)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AppVersionDto))]
        public IActionResult Get()
            => Ok(new AppVersionDto
            {
                Name = Constants.ApplicationName,
                Version = Constants.ApplicationVersion,
            });
    }
}
