using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FunderMaps.Controllers.Webservice
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : AbstractMicroController
    {
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
        [ProducesResponseType(typeof(VersionOutputModel), 200)]
        public IActionResult Get()
        {
            AssemblyName assembly = Assembly.GetEntryAssembly().GetName();

            return Ok(new VersionOutputModel
            {
                Name = assembly.Name,
                Version = assembly.Version,
                VersionString = assembly.Version.ToString(),
            });
        }
    }
}
