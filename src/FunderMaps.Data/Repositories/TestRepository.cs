using FunderMaps.Core.Interfaces.Repositories;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Repository for testing.
    /// </summary>
    internal sealed class TestRepository : DbContextBase, ITestRepository
    {
        /// <summary>
        ///     Check if backend is online.
        /// </summary>
        /// <remarks>
        ///     Explicit check on result, not all commands are submitted
        ///     to the database.
        /// </remarks>
        public async Task<bool> IsAlive()
        {
            var sql = @"SELECT 1";

            await using var context = await DbContextFactory(sql);

            return await context.ScalarAsync<int>() == 1;
        }
    }
}
