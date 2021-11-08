using FunderMaps.AspNetCore.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.AspNetCore.Controllers
{
    /// <summary>
    ///     Return application versioning information.
    /// </summary>
    [AllowAnonymous]
    public class VersionController : ControllerBase
    {
        // GET: version
        /// <summary>
        ///     Return application versioning information.
        /// </summary>
        /// <remarks>
        ///     Cache response for 24 hours. Version will not change
        ///     often and this call is primarily used to check if the
        ///     backend is responding.
        /// </remarks>
        [HttpGet("version"), ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<AppVersionDto> Get()
            => Ok(new AppVersionDto
            {
                Name = Constants.ApplicationName,
                Version = Constants.ApplicationVersion,
                Commit = Constants.ApplicationCommit,
            });
    }
}
