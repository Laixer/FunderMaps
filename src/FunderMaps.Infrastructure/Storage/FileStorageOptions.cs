using System.Collections.Generic;

namespace FunderMaps.Infrastructure.Storage
{
    /// <summary>
    /// Options for the file storage service.
    /// </summary>
    public sealed class FileStorageOptions
    {
        /// <summary>
        /// Name per storage container.
        /// </summary>
        public IDictionary<string, string> StorageContainers { get; set; }
    }
}
