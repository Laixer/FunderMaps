using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Log product hit.
    /// </summary>
    internal class TelemetryRepository : DbServiceBase, ITelemetryRepository
    {
        /// <summary>
        ///     Log a product hit.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The <paramref name="hitCount"/> has a lower bound of 1.
        ///     </para>
        ///     <para>
        ///         This method is and should be fault-tolerant. If one of the
        ///         necessary parameters is not passed, then behavior could
        ///         be different.
        ///     </para>
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

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("user", AppContext.UserId);
            context.AddParameterWithValue("tenant", AppContext.TenantId);
            context.AddParameterWithValue("product", productName);
            context.AddParameterWithValue("count", hitCount == 0 ? 1 : hitCount);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Retrieve all product telemetrics.
        /// </summary>
        public async IAsyncEnumerable<ProductTelemetry> ListAllUsageAsync()
        {
            var sql = @"
                SELECT  -- ProductTracker
                        pt.product,
                        count(pt.organization_id)
                FROM    application.product_tracker AS pt
                WHERE   pt.organization_id = @tenant
                GROUP BY pt.organization_id, pt.product";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("tenant", AppContext.TenantId);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        public static ProductTelemetry MapFromReader(DbDataReader reader, int offset = 0)
            => new()
            {
                Product = reader.GetString(offset++),
                Count = reader.GetInt(offset++),
            };
    }
}
