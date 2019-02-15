using System.Threading.Tasks;

namespace FunderMaps.Data.Seed
{
    public class FunderMapsSeed
    {
        public static Task SeedAsync(FunderMapsDbContext catalogContext)
        {
            // FUTURE: Seed the application here

            return Task.CompletedTask;
        }
    }
}
