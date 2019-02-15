using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Store the file in the data store.
        /// </summary>
        /// <param name="name">File name.</param>
        /// <param name="content">Content array.</param>
        Task StoreFileAsync(string name, byte[] content);

        /// <summary>
        /// Store the file in the data store.
        /// </summary>
        /// <param name="name">File name.</param>
        /// <param name="stream">Content stream.</param>
        Task StoreFileAsync(string name, Stream stream);
    }
}
