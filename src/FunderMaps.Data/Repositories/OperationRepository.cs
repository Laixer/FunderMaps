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
        {
            var sql = @"
                REFRESH MATERIALIZED VIEW CONCURRENTLY data.building_sample;
                REFRESH MATERIALIZED VIEW CONCURRENTLY data.cluster_sample;
                REFRESH MATERIALIZED VIEW CONCURRENTLY data.supercluster_sample;

                CALL data.model_risk_manifest();";

            await using var connection = DbContextFactory.DbProvider.ConnectionScope();
            await connection.ExecuteAsync(sql, commandTimeout: 10800);
        }

        {
            var sql = @"REINDEX TABLE CONCURRENTLY data.model_risk_static;";

            await using var connection = DbContextFactory.DbProvider.ConnectionScope();
            await connection.ExecuteAsync(sql, commandTimeout: 3600);
        }
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
    ///   Cleanup BAG data.
    /// </summary>
    public async Task CleanupBAGAsync()
    {
        var sql = @"
            DROP TABLE IF EXISTS public.woonplaats;
            DROP TABLE IF EXISTS public.verblijfsobject;
            DROP TABLE IF EXISTS public.pand;
            DROP TABLE IF EXISTS public.ligplaats;
            DROP TABLE IF EXISTS public.standplaats;";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();
        await connection.ExecuteAsync(sql, commandTimeout: 10800);
    }

    /// <summary>
    ///     Load building data from BAG.
    /// </summary>
    public async Task LoadBuildingAsync()
    {
        // TODO: Move this into a prepare method
        {
            var sql = @"
                UPDATE public.pand SET identificatie = concat('NL.IMBAG.PAND.', identificatie);
                CREATE INDEX pand_identificatie_idx ON public.pand USING btree (identificatie);
                
                UPDATE public.ligplaats SET identificatie = concat('NL.IMBAG.LIGPLAATS.', identificatie);
                CREATE INDEX ligplaats_identificatie_idx ON public.ligplaats USING btree (identificatie);

                UPDATE public.standplaats SET identificatie = concat('NL.IMBAG.STANDPLAATS.', identificatie);
                CREATE INDEX standplaats_identificatie_idx ON public.standplaats USING btree (identificatie);
                
                --
                
                UPDATE public.verblijfsobject SET nummeraanduiding_hoofdadres_identificatie = concat('NL.IMBAG.NUMMERAANDUIDING.', nummeraanduiding_hoofdadres_identificatie);
                CREATE INDEX verblijfsobject_nummeraanduiding_hoofdadres_identificatie_idx ON public.verblijfsobject USING btree (nummeraanduiding_hoofdadres_identificatie);

                UPDATE public.verblijfsobject SET pand_identificatie = concat('NL.IMBAG.PAND.', pand_identificatie);
                CREATE INDEX verblijfsobject_pand_identificatie_idx ON public.verblijfsobject USING btree (pand_identificatie);

                UPDATE public.verblijfsobject SET identificatie = concat('NL.IMBAG.VERBLIJFSOBJECT.', identificatie);
                CREATE INDEX verblijfsobject_identificatie_idx ON public.verblijfsobject USING btree (identificatie);";

            await using var connection = DbContextFactory.DbProvider.ConnectionScope();
            await connection.ExecuteAsync(sql, commandTimeout: 10800);
        }

        {
            var sql = @"
                INSERT INTO geocoder.building(built_year, is_active, geom, external_id, building_type, neighborhood_id, zone_function)
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
                    p.identificatie,
                    'house',
                    null,
                    (
                        SELECT
                            array_agg(
                                CASE zone_function
                                    when 'bijeenkomstfunctie' then 'assembly'::geocoder.zone_function
                                    when 'sportfunctie' then 'sport'::geocoder.zone_function
                                    when 'celfunctie' then 'prison'::geocoder.zone_function
                                    when 'gezondheidszorgfunctie' then 'medical'::geocoder.zone_function
                                    when 'industriefunctie' then 'industry'::geocoder.zone_function
                                    when 'kantoorfunctie' then 'office'::geocoder.zone_function
                                    when 'logiesfunctie' then 'accommodation'::geocoder.zone_function
                                    when 'onderwijsfunctie' then 'education'::geocoder.zone_function
                                    when 'winkelfunctie' then 'retail'::geocoder.zone_function
                                    when 'woonfunctie' then 'residential'::geocoder.zone_function
                                    else 'other'::geocoder.zone_function
                                END
                            )
                        FROM (
                            SELECT pa.identificatie, unnest(string_to_array(pa.gebruiksdoel, ',')) AS zone_function
                            FROM public.pand pa
                            WHERE pa.identificatie = p.identificatie
                        )
                    )
                FROM public.pand p
                ON CONFLICT (external_id)
                DO UPDATE
                    SET built_year = excluded.built_year,
                    is_active = excluded.is_active,
                    geom = excluded.geom,
                    zone_function = excluded.zone_function;

                INSERT INTO geocoder.building(built_year, is_active, geom, external_id, building_type, neighborhood_id)
                SELECT
                    null,
                    true,
                    ST_Multi(ST_Transform(l.geom, 4326)),
                    l.identificatie,
                    'houseboat',
                    null
                FROM public.ligplaats l
                ON CONFLICT (external_id)
                DO UPDATE
                    SET geom = excluded.geom;
                    
                INSERT INTO geocoder.building(built_year, is_active, geom, external_id, building_type, neighborhood_id)
                SELECT
                    null,
                    true,
                    ST_Multi(ST_Transform(s.geom, 4326)),
                    s.identificatie,
                    'mobile_home',
                    null
                FROM public.standplaats s
                ON CONFLICT (external_id)
                DO UPDATE
                    SET geom = excluded.geom;";

            await using var connection = DbContextFactory.DbProvider.ConnectionScope();
            await connection.ExecuteAsync(sql, commandTimeout: 10800);
        }

        {
            var sql = @"
                UPDATE geocoder.building
                SET is_active = false
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM public.pand
                    WHERE public.pand.identificatie = geocoder.building.external_id
                )
                AND geocoder.building.is_active = true
                AND geocoder.building.external_id like 'NL.IMBAG.PAND.%';";

            await using var connection = DbContextFactory.DbProvider.ConnectionScope();
            await connection.ExecuteAsync(sql, commandTimeout: 10800);
        }

        {
            var sql = @"
                UPDATE geocoder.building
                SET neighborhood_id = n.id
                FROM geocoder.neighborhood n
                WHERE ST_Contains(n.geom, geocoder.building.geom)
                AND neighborhood_id IS NULL;";

            await using var connection = DbContextFactory.DbProvider.ConnectionScope();
            await connection.ExecuteAsync(sql, commandTimeout: 1800);
        }

        {
            var sql = @"REINDEX TABLE CONCURRENTLY geocoder.building;";

            await using var connection = DbContextFactory.DbProvider.ConnectionScope();
            await connection.ExecuteAsync(sql, commandTimeout: 3600);
        }
    }

    /// <summary>
    ///     Load address data from BAG.
    /// </summary>
    public async Task LoadAddressAsync()
    {
        {
            var sql = @"
                INSERT INTO geocoder.address(building_number, postal_code, street, external_id, city, building_id)
                SELECT
                    concat(v.huisnummer, v.huisletter, v.toevoeging),
                    v.postcode,
                    v.openbare_ruimte_naam,
                    v.nummeraanduiding_hoofdadres_identificatie,
                    v.woonplaats_naam,
                    b.id
                FROM public.verblijfsobject v
                JOIN geocoder.building b ON b.external_id = v.pand_identificatie
                ON CONFLICT (external_id)
                DO UPDATE
                    SET building_number = excluded.building_number,
                    postal_code = excluded.postal_code,
                    street = excluded.street,
                    city = excluded.city;
                    
                INSERT INTO geocoder.address(building_number, postal_code, street, external_id, city, building_id)
                SELECT
                    concat(l.huisnummer, l.huisletter, l.toevoeging),
                    l.postcode,
                    l.openbare_ruimte_naam,
                    concat('NL.IMBAG.NUMMERAANDUIDING.', l.nummeraanduiding_hoofdadres_identificatie),
                    l.woonplaats_naam,
                    b.id
                FROM public.ligplaats l
                JOIN geocoder.building b ON b.external_id = l.identificatie
                ON CONFLICT (external_id)
                DO UPDATE
                    SET building_number = excluded.building_number,
                    postal_code = excluded.postal_code,
                    street = excluded.street,
                    city = excluded.city;
   
                INSERT INTO geocoder.address(building_number, postal_code, street, external_id, city, building_id)
                SELECT
                    concat(s.huisnummer, s.huisletter, s.toevoeging),
                    s.postcode,
                    s.openbare_ruimte_naam,
                    concat('NL.IMBAG.NUMMERAANDUIDING.', s.nummeraanduiding_hoofdadres_identificatie),
                    s.woonplaats_naam,
                    b.id
                FROM public.standplaats s
                JOIN geocoder.building b ON b.external_id = s.identificatie
                ON CONFLICT (external_id)
                DO UPDATE
                    SET building_number = excluded.building_number,
                    postal_code = excluded.postal_code,
                    street = excluded.street,
                    city = excluded.city;";

            await using var connection = DbContextFactory.DbProvider.ConnectionScope();
            await connection.ExecuteAsync(sql, commandTimeout: 10800);
        }

        {
            var sql = @"REINDEX TABLE CONCURRENTLY geocoder.address;";

            await using var connection = DbContextFactory.DbProvider.ConnectionScope();
            await connection.ExecuteAsync(sql, commandTimeout: 3600);
        }
    }

    /// <summary>
    ///     Load residence data from BAG.
    /// </summary>
    public async Task LoadResidenceAsync()
    {
        {
            var sql = @"
                INSERT INTO geocoder.residence(id, address_id, building_id, geom)
                SELECT
                    v.identificatie,
                    a.external_id,
                    b.external_id,
                    ST_Transform(v.geom, 4326)
                FROM public.verblijfsobject v
                join geocoder.address a on a.external_id = v.nummeraanduiding_hoofdadres_identificatie
                join geocoder.building b on b.external_id = v.pand_identificatie
                ON CONFLICT (id, address_id, building_id)
                DO NOTHING;";

            await using var connection = DbContextFactory.DbProvider.ConnectionScope();
            await connection.ExecuteAsync(sql, commandTimeout: 10800);
        }

        {
            var sql = @"REINDEX TABLE CONCURRENTLY geocoder.residence;";

            await using var connection = DbContextFactory.DbProvider.ConnectionScope();
            await connection.ExecuteAsync(sql, commandTimeout: 3600);
        }
    }
}
