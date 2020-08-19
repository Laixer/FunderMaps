using FunderMaps.Core.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    internal class NullFileStorageService : IFileStorageService
    {
        public ValueTask<bool> FileExistsAsync(string store, string name)
        {
            return new ValueTask<bool>(false);
        }

        public ValueTask<Uri> GetAccessLinkAsync(string store, string name, double hoursValid)
        {
            return new ValueTask<Uri>(new Uri("http://localhost/blob"));
        }

        public ValueTask StoreFileAsync(string store, string name, Stream stream)
        {
            return new ValueTask();
        }

        public ValueTask StoreFileAsync(string containerName, string fileName, string contentType, Stream stream)
        {
            return new ValueTask();
        }
    }
}
