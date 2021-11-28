﻿using FunderMaps.Core.Entities;
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
