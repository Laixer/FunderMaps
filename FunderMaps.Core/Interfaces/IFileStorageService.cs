using System.IO;
using System.Threading.Tasks;
using FunderMaps.Core.Helpers;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    /// Store a file contents in a data store.
    /// </summary>
    public interface IFileStorageService
    {
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
        /// <param name="name">Application file.</param>
        /// <param name="stream">Content stream.</param>
        Task StoreFileAsync(string store, ApplicationFile file, Stream stream);
    }
}
