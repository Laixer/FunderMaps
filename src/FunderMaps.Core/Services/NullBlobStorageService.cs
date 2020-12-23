using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Dummy file storage service.
    /// </summary>
    internal class NullBlobStorageService : IBlobStorageService
    {
        public Task<bool> FileExistsAsync(string store, string name)
        {
            return Task.FromResult(false);
        }

        public Task<Uri> GetAccessLinkAsync(string store, string name, double hoursValid)
        {
            return Task.FromResult(new Uri("https://localhost/blob"));
        }

        public Task RemoveDirectoryAsync(string directoryPath)
        {
            return Task.CompletedTask;
        }

        public Task StoreDirectoryAsync(string directoryName, string directoryPath, StorageObject storageObject)
        {
            return Task.CompletedTask;
        }

        public Task StoreFileAsync(string store, string name, Stream stream)
        {
            return Task.CompletedTask;
        }

        public Task StoreFileAsync(string containerName, string fileName, string contentType, Stream stream, StorageObject storageObject)
        {
            return Task.CompletedTask;
        }

        public Task TestService()
        {
            return Task.CompletedTask;
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
