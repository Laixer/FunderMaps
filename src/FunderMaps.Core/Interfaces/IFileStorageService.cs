using System;
using System.IO;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    // FUTURE: Rename to IBlobStorageService
    /// <summary>
    ///     Store a file contents in a data store.
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        ///     Check if a file exist in storage.
        /// </summary>
        /// <param name="store">Storage container.</param>
        /// <param name="name">File name.</param>
        /// <returns>True if file exist, false otherwise.</returns>
        ValueTask<bool> FileExistsAsync(string store, string name);

        /// <summary>
        ///     Retrieve file access link as uri.
        /// </summary>
        /// <param name="store">Storage container.</param>
        /// <param name="name">File name.</param>
        /// <param name="hoursValid">How long the link is valid in hours.</param>
        /// <returns>The generated link.</returns>
        ValueTask<Uri> GetAccessLinkAsync(string store, string name, double hoursValid);

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
