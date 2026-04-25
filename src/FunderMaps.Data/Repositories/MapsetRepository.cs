using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

internal sealed class MapsetRepository : DbServiceBase, IMapsetRepository
{
    public async Task<Mapset> GetPublicAsync(string id)
    {
        var sql = @"
            SELECT  c.id,
                    c.name,
                    c.slug,
                    c.style,
                    c.layers,
                    c.metadata,
                    c.public,
                    c.consent,
                    c.note,
                    c.icon,
                    NULL::text AS fence_neighborhood,
                    NULL::text AS fence_district,
                    NULL::text AS fence_municipality,
                    c.order,
                    c.layerset
            FROM    application.mapset_collection AS c
            WHERE   c.id = @id
            AND     c.public = true
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Mapset>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(Mapset));
    }

    public async Task<Mapset> GetPublicByNameAsync(string name)
    {
        var sql = @"
            SELECT  c.id,
                    c.name,
                    c.slug,
                    c.style,
                    c.layers,
                    c.metadata,
                    c.public,
                    c.consent,
                    c.note,
                    c.icon,
                    NULL::text AS fence_neighborhood,
                    NULL::text AS fence_district,
                    NULL::text AS fence_municipality,
                    c.order,
                    c.layerset
            FROM    application.mapset_collection AS c
            WHERE   LOWER(REGEXP_REPLACE(c.name, '\s+', '-', 'g')) = LOWER(@name)
            AND     c.public = true
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Mapset>(sql, new { name })
            ?? throw new EntityNotFoundException(nameof(Mapset));
    }

    public async IAsyncEnumerable<Mapset> GetByOrganizationIdAsync(Guid id)
    {
        var sql = @"
            SELECT  c.id,
                    c.name,
                    c.slug,
                    c.style,
                    c.layers,
                    c.metadata,
                    c.public,
                    c.consent,
                    c.note,
                    c.icon,
                    (
                        SELECT array_agg(neighborhood_id)
                        FROM   application.organization_geolock_neighborhood
                        WHERE  organization_id = om.organization_id
                    ) AS fence_neighborhood,
                    (
                        SELECT array_agg(district_id)
                        FROM   application.organization_geolock_district
                        WHERE  organization_id = om.organization_id
                    ) AS fence_district,
                    (
                        SELECT array_agg(municipality_id)
                        FROM   application.organization_geolock_municipality
                        WHERE  organization_id = om.organization_id
                    ) AS fence_municipality,
                    c.order,
                    c.layerset
            FROM    application.mapset_collection AS c
            JOIN    application.organization_mapset om ON om.mapset_id = c.id
            WHERE   om.organization_id = @id
            AND     c.public = false
            ORDER BY c.order ASC";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Mapset>(sql, new { id }))
        {
            yield return item;
        }
    }
}
