using Dapper;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Various data operations.
/// </summary>
internal sealed class OperationRepository : DbServiceBase, IOperationRepository
{
    /// <summary>
    ///     Check if backend is online.
    /// </summary>
    /// <remarks>
    ///     Explicit check on result, not all commands are submitted
    ///     to the database.
    /// </remarks>
    public async Task<bool> IsAliveAsync()
    {
        var sql = @"SELECT 1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<int>(sql) == 1;
    }

    /// <summary>
    ///    Refresh data models.
    /// </summary>
    public async Task RefreshModelAsync()
    {
        var sql = @"
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.building_sample;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.cluster_sample;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.supercluster_sample;

            CALL data.model_risk_manifest();";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, commandTimeout: 10800);
    }

    /// <summary>
    ///   Refresh statistics.
    /// </summary>
    public async Task RefreshStatisticsAsync()
    {
        var sql = @"
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_inquiries;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_inquiry_municipality;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_incidents;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_incident_municipality;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_foundation_type;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_foundation_risk;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_data_collected;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_construction_years;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_buildings_restored;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_postal_code_foundation_type;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_postal_code_foundation_risk;";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, commandTimeout: 3600);
    }

    /// <summary>
    ///   Copy BAG data to building table.
    /// </summary>
    public async Task CopyPandToBuildingAsync()
    {
        var sql = @"
            DROP TABLE IF EXISTS public.woonplaats;
            DROP TABLE IF EXISTS public.verblijfsobject;

            INSERT INTO geocoder.building(built_year, is_active, geom, external_id, external_source, building_type, neighborhood_id, mutation_date)
            SELECT
                case
                    when p.bouwjaar > 2099 then null
                    when p.bouwjaar < 899 then null
                    else to_date(p.bouwjaar::text, 'YYYY')
                end,
                case lower(p.status)
                    when 'bouwvergunning verleend' then false
                    else true
                end,
                ST_Multi(ST_Transform(p.geom, 4326)),
                concat('NL.IMBAG.PAND.', p.identificatie),
                'nl_bag',
                'house',
                null,
                null
            FROM public.pand p
            ON CONFLICT (external_id)
            DO UPDATE
                SET built_year = excluded.built_year,
                is_active = excluded.is_active,
                geom = excluded.geom;
            DROP TABLE IF EXISTS public.pand;

            INSERT INTO geocoder.building(built_year, is_active, geom, external_id, external_source, building_type, neighborhood_id, mutation_date)
            SELECT
                null,
                true,
                ST_Multi(ST_Transform(l.geom, 4326)),
                concat('NL.IMBAG.LIGPLAATS.', l.identificatie),
                'nl_bag',
                'houseboat',
                null,
                null
            FROM public.ligplaats l
            ON CONFLICT (external_id)
            DO UPDATE
                SET geom = excluded.geom;
            DROP TABLE IF EXISTS public.ligplaats;
                
            INSERT INTO geocoder.building(built_year, is_active, geom, external_id, external_source, building_type, neighborhood_id, mutation_date)
            SELECT
                null,
                true,
                ST_Multi(ST_Transform(s.geom, 4326)),
                concat('NL.IMBAG.STANDPLAATS.', s.identificatie),
                'nl_bag',
                'mobile_home',
                null,
                null
            FROM public.standplaats s
            ON CONFLICT (external_id)
            DO UPDATE
                SET geom = excluded.geom;
            DROP TABLE IF EXISTS public.standplaats;";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, commandTimeout: 10800);
    }
}
