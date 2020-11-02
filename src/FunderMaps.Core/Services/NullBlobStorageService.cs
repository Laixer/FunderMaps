using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;
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
        public ValueTask<bool> FileExistsAsync(string store, string name)
        {
            return new ValueTask<bool>(false);
        }

        public ValueTask<Uri> GetAccessLinkAsync(string store, string name, double hoursValid, AccessType accessType = AccessType.Read)
        {
            return new ValueTask<Uri>(new Uri("https://localhost/blob"));
        }

        public ValueTask<IEnumerable<string>> ListSubcontainerNamesAsync(string containerName) 
            => throw new NotImplementedException();

        public ValueTask StoreFileAsync(string store, string name, Stream stream)
        {
            return new ValueTask();
        }

        public ValueTask StoreFileAsync(string containerName, string fileName, string contentType, Stream stream)
        {
            return new ValueTask();
        }

        public Task TestService()
        {
            return Task.CompletedTask;
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
