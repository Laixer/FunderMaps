using Microsoft.EntityFrameworkCore;

namespace FunderMaps.Tests.Helpers
{
    internal static class DatabaseWrapper
    {
        public static DbContextOptions<TContext> GenerateDatabase<TContext>(string name)
            where TContext : DbContext
        {
            return new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase(databaseName: name)
                .Options;
        }
    }
}
