using System;

namespace FunderMaps.WebApi.ViewModels
{
    /// <summary>
    /// Version output model.
    /// </summary>
    public sealed class ApplicationVersionModel
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
}
