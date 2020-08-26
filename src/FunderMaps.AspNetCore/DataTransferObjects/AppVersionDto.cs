using System;
using System.Collections.Generic;
using System.Text;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Application version DTO.
    /// </summary>
    public sealed class AppVersionDto
    {
        /// <summary>
        ///     Application name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Application version structure.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        ///     Application version as string.
        /// </summary>
        public string VersionString { get; set; }
    }
}
