using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Log product hit.
    /// </summary>
    internal class TelemetryRepository : DbContextBase, ITelemetryRepository
    {
        /// <summary>
        ///     Log a product hit.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="hitCount"/> has a lower bound of 1.
        /// </remarks>
        /// <param name="productName">Product name.</param>
        /// <param name="hitCount">Number of hits to log.</param>
        public async Task ProductHitAsync(string productName, int hitCount = 1)
        {
            if (string.IsNullOrEmpty(productName) || !AppContext.HasIdentity)
            {
                return;
            }

            var sql = @"
                INSERT INTO application.product_telemetry AS pu (
                    user_id,
                    organization_id,
                    product,
                    count)
                VALUES (
                    @user,
                    @tenant,
                    lower(trim(@product)),
                    @count)
                ON CONFLICT (organization_id, product)
                DO UPDATE SET
                    count = pu.count + EXCLUDED.count";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("user", AppContext.UserId);
            context.AddParameterWithValue("tenant", AppContext.TenantId);
            context.AddParameterWithValue("product", productName);
            context.AddParameterWithValue("count", hitCount == 0 ? 1 : hitCount);

            await context.NonQueryAsync();
        }
    }
}
