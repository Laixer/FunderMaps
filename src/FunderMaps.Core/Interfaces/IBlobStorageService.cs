using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Store a file contents in a data store.
    /// </summary>
    public interface IBlobStorageService
    {
        /// <summary>
        ///     Check if a file exist in storage.
        /// </summary>
        /// <param name="containerName">Storage container.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>True if file exist, false otherwise.</returns>
        ValueTask<bool> FileExistsAsync(string containerName, string fileName);

        /// <summary>
        ///     Retrieve file access link as uri.
        /// </summary>
        /// <param name="containerName">Storage container.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="hoursValid">How long the link is valid in hours.</param>
        /// <param name="accessType">Indicates what we want to do with the link.</param>
        /// <returns>The generated link.</returns>
        ValueTask<Uri> GetAccessLinkAsync(string containerName, string fileName, double hoursValid, AccessType accessType);
        
        /// <summary>
        ///     List the names of all subcontainers in a given container.
        /// </summary>
        /// <remarks>
        ///     This should return name, not the full path.
        /// </remarks>
        /// <param name="containerName">Container name to check.</param>
        /// <returns>Collection of subcontainer names.</returns>
        ValueTask<IEnumerable<string>> ListSubcontainerNamesAsync(string containerName);

        /// <summary>
        ///     Store the file in the data store.
        /// </summary>
        /// <param name="containerName">Storage container.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="stream">Content stream.</param>
        ValueTask StoreFileAsync(string containerName, string fileName, Stream stream);

        /// <summary>
        ///     Store the file in the data store.
        /// </summary>
        /// <param name="containerName">Storage container.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="contentType">Blob content type.</param>
        /// <param name="stream">Content stream.</param>
        ValueTask StoreFileAsync(string containerName, string fileName, string contentType, Stream stream);
    }
}
