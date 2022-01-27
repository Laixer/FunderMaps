using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Extensions;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Log product hit.
/// </summary>
internal class TelemetryRepository : DbServiceBase, ITelemetryRepository
{
    /// <summary>
    ///     Retrieve all product telemetrics.
    /// </summary>
    public async IAsyncEnumerable<ProductTelemetry2> ListAllAsync()
    {
        var sql = @"
            SELECT  -- ProductTracker
                    o.id,
                    o.name,
                    pt.product,
                    count(pt.organization_id)
            FROM    application.product_tracker AS pt
            JOIN	application.organization AS o ON o.id::uuid = pt.organization_id::uuid
            GROUP BY o.id, pt.product";

        await using var context = await DbContextFactory.CreateAsync(sql);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return new()
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Product = reader.GetString(2),
                Count = reader.GetInt(3),
            };
        }
    }

    // TODO: Rename
    /// <summary>
    ///     Retrieve all product telemetrics.
    /// </summary>
    public async IAsyncEnumerable<ProductTelemetry3> ListRecentByTenantAsync(Guid tenant)
    {
        var sql = @"
            SELECT  -- ProductTracker
                    pt.product,
                    pt.building_id,
                    pt.create_date
            FROM    application.product_tracker AS pt
            WHERE   pt.organization_id = @tenant
            ORDER BY pt.create_date desc
            LIMIT 10";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("tenant", tenant);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return new()
            {
                Product = reader.GetString(0),
                BuildingId = reader.GetString(1),
                CreateDate = reader.GetDateTime(2),
            };
        }
    }

    // TODO: Rename
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
            yield return new()
            {
                Product = reader.GetString(0),
                Count = reader.GetInt(1),
            };
        }
    }
}
