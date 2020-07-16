using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    internal class NullFileStorageService : IFileStorageService
    {
        public ValueTask<bool> FileExists(string store, string name)
        {
            return new ValueTask<bool>(false);
        }

        public Uri GetAccessLink(string store, string name, double hoursValid)
        {
            return new Uri("/");
        }

        public ValueTask<string> GetStorageNameAsync()
        {
            return new ValueTask<string>("NullFileStorageService");
        }

        public ValueTask StoreFileAsync(string store, string name, byte[] content)
        {
            return new ValueTask();
        }

        public ValueTask StoreFileAsync(string store, FileWrapper file, byte[] content)
        {
            return new ValueTask();
        }

        public ValueTask StoreFileAsync(string store, string name, Stream stream)
        {
            return new ValueTask();
        }

        public ValueTask StoreFileAsync(string store, FileWrapper file, Stream stream)
        {
            return new ValueTask();
        }
    }
}
