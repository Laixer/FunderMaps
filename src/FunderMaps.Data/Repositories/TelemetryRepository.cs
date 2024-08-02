using Dapper;
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

    public async IAsyncEnumerable<Guid> ListLastMonthOrganizationaAsync()
    {
        var sql = @"
            SELECT  -- ProductTracker
                    pt.organization_id
            FROM    application.product_tracker AS pt
            WHERE   pt.create_date >= date_trunc('month', CURRENT_DATE) - interval '1 month'
            AND     pt.create_date < date_trunc('month', CURRENT_DATE)
            GROUP BY pt.organization_id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Guid>(sql))
        {
            yield return item;
        }
    }

    public async IAsyncEnumerable<ProductCall> ListLastMonthByOrganizationIdAsync(Guid id)
    {
        var sql = @"
            SELECT  -- ProductTracker
                    pt.organization_id,
                    pt.product,
                    pt.building_id,
                    b.external_id,
                    pt.create_date,
                    pt.identifier AS request
            FROM    application.product_tracker AS pt
            JOIN    geocoder.building AS b ON b.id = pt.building_id
            WHERE   pt.organization_id = @id
            AND     pt.create_date >= date_trunc('month', CURRENT_DATE) - interval '1 month'
            AND     pt.create_date < date_trunc('month', CURRENT_DATE)";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<ProductCall>(sql, new { id }))
        {
            yield return item;
        }
    }
}
