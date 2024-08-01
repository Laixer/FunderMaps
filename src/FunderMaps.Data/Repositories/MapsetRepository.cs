using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Repository for map sets.
/// </summary>
internal sealed class MapsetRepository : DbServiceBase, IMapsetRepository
{
    // FUTURE: Add caching.
    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<Mapset> GetPublicAsync(Guid id)
    {
        // TODO: Refactor to use a view.
        var sql = @"
            SELECT  -- Mapset
                    m.id,
                    m.name,
                    LOWER(REGEXP_REPLACE(m.name, '\s+', '-', 'g')) AS slug,
                    m.style,
                    m.layers,
                    m.options,
                    m.public,
                    m.consent,
                    m.note,
                    m.icon,
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
            WHERE   m.id = @id
            AND     m.public = true
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var mapset = await connection.QuerySingleOrDefaultAsync<Mapset>(sql, new { id });
        return mapset is null ? throw new EntityNotFoundException(nameof(mapset)) : mapset;
    }

    /// <summary>
    ///    Gets an analysis product by its name.
    /// </summary>
    public async Task<Mapset> GetPublicByNameAsync(string name)
    {
        // TODO: Refactor to use a view.
        var sql = @"
            SELECT  -- Mapset
                    m.id,
                    m.name,
                    LOWER(REGEXP_REPLACE(m.name, '\s+', '-', 'g')) AS slug,
                    m.style,
                    m.layers,
                    m.options,
                    m.public,
                    m.consent,
                    m.note,
                    m.icon,
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
            WHERE   LOWER(REGEXP_REPLACE(m.name, '\s+', '-', 'g')) = LOWER(@name)
            AND     m.public = true
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var mapset = await connection.QuerySingleOrDefaultAsync<Mapset>(sql, new { name });
        return mapset is null ? throw new EntityNotFoundException(nameof(mapset)) : mapset;
    }

    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async IAsyncEnumerable<Mapset> GetByOrganizationIdAsync(Guid id)
    {
        // TODO: Refactor to use a view.
        var sql = @"
            SELECT  -- Mapset
                    m.id,
                    m.name,
                    LOWER(REGEXP_REPLACE(m.name, '\s+', '-', 'g')) AS slug,
                    m.style,
                    m.layers,
                    COALESCE(mo.options, m.options),
                    m.public,
                    m.consent,
                    m.note,
                    m.icon,
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
