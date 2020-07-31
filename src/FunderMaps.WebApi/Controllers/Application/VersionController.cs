using FunderMaps.Controllers;
using FunderMaps.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Return application versioning information.
    /// </summary>
    [AllowAnonymous, ApiController, Route("api/version")]
    public class VersionController : BaseApiController
    {
        // GET: api/version
        /// <summary>
        ///     Return application versioning information.
        /// </summary>
        /// <remarks>
        ///     Cache response for 24 hours. Version will not change
        ///     often and this call is primarily used to check if the
        ///     API is responding.
        /// </remarks>
        [HttpGet, ResponseCache(Duration = 60 * 60 * 24)]
        public IActionResult Get()
            => Ok(new ApplicationVersionModel
            {
                Name = Constants.ApplicationName,
                Version = Constants.ApplicationVersion,
                VersionString = Constants.ApplicationVersion.ToString(),
            });
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods