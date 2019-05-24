using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FunderMaps.Helpers;

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
        /// <summary>
        /// Version output model.
        /// </summary>
        public sealed class VersionOutputModel
        {
            /// <summary>
            /// Application name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Application version structure.
            /// </summary>
            public Version Version { get; set; }

            /// <summary>
            /// Application version as string.
            /// </summary>
            public string VersionString { get; set; }
        }

        // GET: api/version
        /// <summary>
        /// Return application versioning information.
        /// </summary>
        [HttpGet]
        [ResponseCache(Duration = 86400)]
        [ProducesResponseType(typeof(VersionOutputModel), 200)]
        public IActionResult Get()
        {
            return Ok(new VersionOutputModel
            {
                Name = Constants.ApplicationName,
                Version = Constants.ApplicationVersion,
                VersionString = Constants.ApplicationVersion.ToString(),
            });
        }
    }
}
