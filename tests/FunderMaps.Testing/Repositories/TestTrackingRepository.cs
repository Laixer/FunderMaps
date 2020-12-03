using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class TestTrackingRepository : ITelemetryRepository
    {
        public Task ProductHitAsync(string productName, int hitCount = 1) => Task.CompletedTask;
    }
}
