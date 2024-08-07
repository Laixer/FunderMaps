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

        // TODO: Refactor to use a view.
        var sql = @"
            SELECT  -- Mapset
                    m.id::text,
                    m.name,
                    LOWER(REGEXP_REPLACE(m.name, '\s+', '-', 'g')) AS slug,
                    m.style,
                    m.layers,
                    m.options,
                    m.public,
                    m.consent,
                    m.note,
                    m.icon,
                    NULL AS fence_neighborhood,
                    NULL AS fence_district,
                    NULL as fence_municipality,
                    (
                        SELECT jsonb_agg(maplayers.layer)
                        FROM (
                            SELECT l AS layer
                            FROM maplayer.layer l
                            WHERE l.id IN (
                                SELECT  unnest(m2.layers)
                                FROM    maplayer.mapset m2
                                WHERE   m2.id = m.id
                            )
                            ORDER BY l.order ASC
                        ) AS maplayers
                    ) AS layerset
            FROM    maplayer.mapset AS m
            WHERE   m.id::text = @id
            AND     m.public = true
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

        // TODO: Refactor to use a view.
        var sql = @"
            SELECT  -- Mapset
                    m.id::text,
                    m.name,
                    LOWER(REGEXP_REPLACE(m.name, '\s+', '-', 'g')) AS slug,
                    m.style,
                    m.layers,
                    m.options,
                    m.public,
                    m.consent,
                    m.note,
                    m.icon,
                    NULL AS fence_neighborhood,
                    NULL AS fence_district,
                    NULL AS fence_municipality,
                    (
                        SELECT jsonb_agg(maplayers.layer)
                        FROM (
                            SELECT l AS layer
                            FROM maplayer.layer l
                            WHERE l.id IN (
                                SELECT  unnest(m2.layers)
                                FROM    maplayer.mapset m2
                                WHERE   m2.id = m.id
                            )
                            ORDER BY l.order ASC
                        ) AS maplayers
                    ) AS layerset
            FROM    maplayer.mapset AS m
            WHERE   LOWER(REGEXP_REPLACE(m.name, '\s+', '-', 'g')) = LOWER(@name)
            AND     m.public = true
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
        // TODO: Refactor to use a view.
        var sql = @"
            SELECT  -- Mapset
                    m.id::text,
                    m.name,
                    LOWER(REGEXP_REPLACE(m.name, '\s+', '-', 'g')) AS slug,
                    m.style,
                    m.layers,
                    COALESCE(mo.options, m.options),
                    m.public,
                    m.consent,
                    m.note,
                    m.icon,
                    o.fence_neighborhood,
                    o.fence_district,
                    o.fence_municipality,
                    (
                        SELECT jsonb_agg(maplayers.layer)
                        FROM (
                            SELECT l AS layer
                            FROM maplayer.layer l
                            WHERE l.id IN (
                                SELECT  unnest(m2.layers)
                                FROM    maplayer.mapset m2
                                WHERE   m2.id = m.id
                            )
                            ORDER BY l.order ASC
                        ) AS maplayers
                    ) AS layerset
            FROM    maplayer.map_organization mo
            JOIN    maplayer.mapset AS m on m.id = mo.map_id
            JOIN    application.organization o on o.id = mo.organization_id 
            WHERE   mo.organization_id = @id
            AND     m.public = false
            ORDER BY m.order ASC";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Mapset>(sql, new { id }))
        {
            yield return item;
        }
    }
}
