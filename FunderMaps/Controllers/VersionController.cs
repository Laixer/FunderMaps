using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FunderMaps.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        public sealed class VersionOutputModel
        {
            public string Name { get; set; }
            public Version ApplicationVersion { get; set; }
            public string ApplicationVersionString { get; set; }
        }

        // GET: api/version
        [HttpGet]
        public VersionOutputModel Get()
        {
            AssemblyName assembly = Assembly.GetEntryAssembly().GetName();

            return new VersionOutputModel
            {
                Name = assembly.Name,
                ApplicationVersion = assembly.Version,
                ApplicationVersionString = assembly.Version.ToString(),
            };
        }
    }
}
