using FunderMaps.Core.Helpers;
using System.IO;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    /// Store a file contents in a data store.
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Retrieve account name for the storage service.
        /// </summary>
        /// <returns>Account name.</returns>
        Task<string> StorageAccountAsync();

        /// <summary>
        /// Retrieve file access link.
        /// </summary>
        /// <param name="store">Storage container.</param>
        /// <param name="name">File name.</param>
        /// <param name="hoursValid">How long the link is valid in hours.</param>
        /// <returns>The generated link.</returns>
        string GetAccessLink(string store, string name, double hoursValid);

        /// <summary>
        /// Store the file in the data store.
        /// </summary>
        /// <param name="store">Storage container.</param>
        /// <param name="name">File name.</param>
        /// <param name="content">Content array.</param>
        Task StoreFileAsync(string store, string name, byte[] content);

        /// <summary>
        /// Store the file in the data store.
        /// </summary>
        /// /// <param name="store">Storage container.</param>
        /// <param name="name">File name.</param>
        /// <param name="stream">Content stream.</param>
        Task StoreFileAsync(string store, string name, Stream stream);

        /// <summary>
        /// Store the file in the data store.
        /// </summary>
        /// /// <param name="store">Storage container.</param>
        /// <param name="file">Application file.</param>
        /// <param name="stream">Content stream.</param>
        Task StoreFileAsync(string store, ApplicationFile file, Stream stream);
    }
}
