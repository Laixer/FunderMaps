using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Extensions;
using System.Data.Common;
using System.Text.Json;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Repository for map sets.
/// </summary>
internal sealed class MapsetRepository : DbServiceBase, IMapsetRepository
{
    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<Mapset> GetPublicAsync(Guid id)
    {
        var sql = @"
            SELECT  -- Mapset
                    m.id,
                    m.name,
                    m.style,
                    m.layers,
                    m.options,
                    m.public,
                    m.consent
            FROM    maplayer.mapset AS m
            WHERE   m.id = @id
            AND     m.public = true
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await using var reader = await context.ReaderAsync();

        return MapFromReader(reader);
    }

    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async IAsyncEnumerable<Mapset> GetByOrganizationIdAsync(Guid id)
    {
        var sql = @"
            SELECT  -- Mapset
                    m.id,
                    m.name,
                    m.style,
                    m.layers,
                    m.options,
                    m.public,
                    m.consent
            FROM    maplayer.map_organization mo
            JOIN    maplayer.mapset AS m on m.id = mo.map_id
            WHERE   mo.organization_id = @id
            AND     m.public = false";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return MapFromReader(reader);
        }
    }

    /// <summary>
    ///     Maps a reader to an <see cref="Mapset"/>.
    /// </summary>
    private static Mapset MapFromReader(DbDataReader reader)
        => new()
        {
            Id = reader.GetGuid(0),
            Name = reader.GetString(1),
            Style = reader.GetString(2),
            Layers = reader.GetSafeStringArray(3),
            Options = reader.GetFieldValue<object>(4),
            Public = reader.GetBoolean(5),
            Consent = reader.GetSafeString(6),
        };

    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<Mapset> GetPublicAsync2(Guid id)
    {
        var sql = @"
            SELECT  -- Mapset
                    m.id,
                    m.name,
                    m.style,
                    m.layers,
                    m.options,
                    m.public,
                    m.consent,
                    NULL as fence_municipality,
                    (
                        SELECT jsonb_agg(maplayers.layer)
                        FROM (
                            SELECT l AS layer
                            FROM maplayer.layer l
                            WHERE l.id IN (
                                SELECT  unnest(m.layers)
                                FROM    maplayer.mapset m2
                                WHERE   m2.id = m.id
                            )
                        ) AS maplayers
                    ) AS layerset
            FROM    maplayer.mapset AS m
            WHERE   m.id = @id
            AND     m.public = true
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await using var reader = await context.ReaderAsync();

        return MapFromReader2(reader);
    }

    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async IAsyncEnumerable<Mapset> GetByOrganizationIdAsync2(Guid id)
    {
        var sql = @"
            SELECT  -- Mapset
                    m.id,
                    m.name,
                    m.style,
                    m.layers,
                    m.options,
                    m.public,
                    m.consent,
                    o.fence_municipality,
                    (
                        SELECT jsonb_agg(maplayers.layer)
                        FROM (
                            SELECT l AS layer
                            FROM maplayer.layer l
                            WHERE l.id IN (
                                SELECT  unnest(m.layers)
                                FROM    maplayer.mapset m2
                                WHERE   m2.id = m.id
                            )
                        ) AS maplayers
                    ) AS layerset
            FROM    maplayer.map_organization mo
            JOIN    maplayer.mapset AS m on m.id = mo.map_id
            JOIN    application.organization o on o.id = mo.organization_id 
            WHERE   mo.organization_id = @id
            AND     m.public = false";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return MapFromReader2(reader);
        }
    }

    /// <summary>
    ///     Maps a reader to an <see cref="Mapset"/>.
    /// </summary>
    private static Mapset MapFromReader2(DbDataReader reader)
        => new()
        {
            Id = reader.GetGuid(0),
            Name = reader.GetString(1),
            Style = reader.GetString(2),
            Layers = reader.GetSafeStringArray(3),
            Options = reader.GetFieldValue<object>(4),
            Public = reader.GetBoolean(5),
            Consent = reader.GetSafeString(6),
            FenceMunicipality = reader.GetSafeString(7),
            LayerSet = JsonSerializer.Deserialize<object>(reader.GetString(8)),
        };
}
