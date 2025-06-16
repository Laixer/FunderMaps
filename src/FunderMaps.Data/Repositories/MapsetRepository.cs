using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace FunderMaps.Data.Repositories;

internal sealed class MapsetRepository : DbServiceBase, IMapsetRepository
{
    public async Task<Mapset> GetPublicAsync(string id)
    {
        if (Cache.TryGetValue(id, out Mapset? value))
        {
            return value ?? throw new EntityNotFoundException(nameof(Mapset));
        }

        var sql = @"
            SELECT  c.id,
                    c.name,
                    c.slug,
                    c.style,
                    c.layers,
                    c.options,
                    c.public,
                    c.consent,
                    c.note,
                    c.icon,
                    NULL::text AS fence_neighborhood,
                    NULL::text AS fence_district,
                    NULL::text AS fence_municipality,
                    c.order,
                    c.layerset
            FROM    maplayer.mapset_collection AS c
            WHERE   c.id = @id
            AND     c.public = true
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var mapset = await connection.QuerySingleOrDefaultAsync<Mapset>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(Mapset));

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(2))
            .SetAbsoluteExpiration(TimeSpan.FromHours(12));

        return Cache.Set(mapset.Id, mapset, options);
    }

    public async Task<Mapset> GetPublicByNameAsync(string name)
    {
        if (Cache.TryGetValue(name, out Mapset? value))
        {
            return value ?? throw new EntityNotFoundException(nameof(Mapset));
        }

        var sql = @"
            SELECT  c.id,
                    c.name,
                    c.slug,
                    c.style,
                    c.layers,
                    c.options,
                    c.public,
                    c.consent,
                    c.note,
                    c.icon,
                    NULL::text AS fence_neighborhood,
                    NULL::text AS fence_district,
                    NULL::text AS fence_municipality,
                    c.order,
                    c.layerset
            FROM    maplayer.mapset_collection AS c
            WHERE   LOWER(REGEXP_REPLACE(c.name, '\s+', '-', 'g')) = LOWER(@name)
            AND     c.public = true
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var mapset = await connection.QuerySingleOrDefaultAsync<Mapset>(sql, new { name })
            ?? throw new EntityNotFoundException(nameof(Mapset));

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(2))
            .SetAbsoluteExpiration(TimeSpan.FromHours(12));

        Cache.Set(mapset.Name, mapset, options);

        return Cache.Set(mapset.Id, mapset, options);
    }

    public async IAsyncEnumerable<Mapset> GetByOrganizationIdAsync(Guid id)
    {
        var sql = @"
            SELECT  c.id,
                    c.name,
                    c.slug,
                    c.style,
                    c.layers,
                    c.options,
                    c.public,
                    c.consent,
                    c.note,
                    c.icon,
                    (
                        SELECT array_agg(neighborhood_id)
                        FROM   application.organization_geolock_neighborhood
                        WHERE  organization_id = mo.organization_id
                    ) AS fence_neighborhood,
                    (
                        SELECT array_agg(district_id)
                        FROM   application.organization_geolock_district
                        WHERE  organization_id = mo.organization_id
                    ) AS fence_district,
                    (
                        SELECT array_agg(municipality_id)
                        FROM   application.organization_geolock_municipality
                        WHERE  organization_id = mo.organization_id
                    ) AS fence_municipality,
                    c.order,
                    c.layerset
            FROM    maplayer.mapset_collection AS c
            JOIN    maplayer.map_organization mo ON mo.map_id = c.id
            WHERE   mo.organization_id = @id
            AND     c.public = false
            ORDER BY c.order ASC";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Mapset>(sql, new { id }))
        {
            yield return item;
        }
    }
}
