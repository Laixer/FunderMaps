using System;
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
        Task<bool> FileExistsAsync(string containerName, string fileName);

        /// <summary>
        ///     Retrieve file access link as uri.
        /// </summary>
        /// <param name="containerName">Storage container.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="hoursValid">How long the link is valid in hours.</param>
        /// <returns>The generated link.</returns>
        Task<Uri> GetAccessLinkAsync(string containerName, string fileName, double hoursValid);

        /// <summary>
        ///     Store the file in the data store.
        /// </summary>
        /// <param name="containerName">Storage container.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="stream">Content stream.</param>
        Task StoreFileAsync(string containerName, string fileName, Stream stream);

        /// <summary>
        ///     Store the file in the data store.
        /// </summary>
        /// <param name="containerName">Storage container.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="contentType">Blob content type.</param>
        /// <param name="stream">Content stream.</param>
        Task StoreFileAsync(string containerName, string fileName, string contentType, Stream stream);

        /// <summary>
        ///     Test the Amazon S3 service backend.
        /// </summary>
        Task TestService();
    }
}
