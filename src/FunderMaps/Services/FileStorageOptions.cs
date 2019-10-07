using System.Collections.Generic;

namespace FunderMaps.Services
{
    /// <summary>
    /// Options for the file storage service.
    /// </summary>
    public class FileStorageOptions
    {
        /// <summary>
        /// Name per storage container.
        /// </summary>
        public IDictionary<string, string> StorageContainers { get; }
    }
}
