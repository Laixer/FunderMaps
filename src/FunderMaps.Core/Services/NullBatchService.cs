using FunderMaps.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Dummy batch service.
    /// </summary>
    internal class NullBatchService : IBatchService
    {
        public Task<Guid> EnqueueAsync(string name, object value, CancellationToken token = default)
        {
            return Task.FromResult(Guid.NewGuid());
        }

        public Task TestService()
        {
            return Task.CompletedTask;
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
