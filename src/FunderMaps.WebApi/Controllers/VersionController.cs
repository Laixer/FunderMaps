using FunderMaps.Core;
using FunderMaps.Core.Controllers;
using FunderMaps.Core.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Return application versioning information.
/// </summary>
[AllowAnonymous]
public class VersionController : FunderMapsController
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
    [HttpGet("api/version")] // FUTURE: Remove api prefix
    [HttpGet("version"), ResponseCache(Duration = 60 * 60 * 24)]
    public ActionResult<AppVersionDto> Get()
        => Ok(new AppVersionDto
        {
            Name = Constants.ApplicationName,
            Version = Constants.ApplicationVersion,
            Commit = Constants.ApplicationCommit,
        });
}
