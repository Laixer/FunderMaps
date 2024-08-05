using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Log product hit.
/// </summary>
internal class TelemetryRepository : DbServiceBase, ITelemetryRepository
{
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
