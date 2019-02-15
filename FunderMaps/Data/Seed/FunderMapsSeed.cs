using System.Threading.Tasks;

namespace FunderMaps.Data.Seed
{
    public class FunderMapsSeed
    {
        public static Task SeedAsync(FunderMapsDbContext catalogContext)
        {
            return Task.CompletedTask;
        }
    }
}
