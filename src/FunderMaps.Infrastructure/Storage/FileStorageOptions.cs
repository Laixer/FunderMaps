using System.Collections.Generic;

namespace FunderMaps.Infrastructure.Storage
{
    // TODO Remove this later.
    /// <summary>
    ///     Options for the file storage service.
    /// </summary>
    /// <remarks>
    ///     This exists for backwards compatibility with the Azure Storage service,
    ///     we have since switched to the Digital Ocean Storage service. The latter
    ///     requires a completely different form of configuration.
    /// </remarks>
    public sealed class FileStorageOptions
    {
        /// <summary>
        ///     Configuration section key.
        /// </summary>
        public const string Section = "FileStorageContainers";

        /// <summary>
        ///     Name per storage container.
        /// </summary>
        public IDictionary<string, string> StorageContainers { get; set; } = new Dictionary<string, string>();
    }
}