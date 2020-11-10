using FunderMaps.Core.Interfaces;
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
        public Task EnqueueAsync(string name, object value, CancellationToken token = default)
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
