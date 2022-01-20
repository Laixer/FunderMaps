﻿using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Extensions;
using System.Data.Common;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Repository for analysis products.
/// </summary>
internal sealed class AnalysisRepository : DbServiceBase, IAnalysisRepository
{
    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<AnalysisProduct> GetByIdAsync(string id)
    {
        var sql = @"
                WITH tracker AS (
		            INSERT INTO application.product_tracker AS pt (organization_id, product, building_id)
		            SELECT
			            @tenant,
			            'analysis',
			            aa.id
		            FROM    data.analysis_address AS aa
		            WHERE   aa.id = @id
		            LIMIT 1
		            returning pt.building_id
                )
                SELECT -- AnalysisAddress
				        aa.id,
				        aa.external_id,
				        aa.external_source,
				        aa.construction_year,
				        aa.construction_year_source,
				        aa.address_id,
				        aa.address_external_id,
				        aa.postal_code,
				        aa.neighborhood_id,
				        aa.groundwater_level,
				        aa.soil,
				        aa.building_height,
				        aa.ground_level,
				        aa.cpt,
				        aa.monitoring_well,
				        aa.recovery_advised,
				        aa.damage_cause,
				        aa.substructure,
				        aa.document_name,
				        aa.document_date,
				        aa.inquiry_type,
				        aa.recovery_type,
				        aa.recovery_status,
				        aa.surface_area,
				        aa.living_area,
				        aa.foundation_bearing_layer,
				        aa.restoration_costs,
				        aa.foundation_type,
				        aa.foundation_type_reliability,
				        aa.drystand,
				        aa.drystand_reliability,
				        aa.drystand_risk,
				        aa.dewatering_depth,
				        aa.dewatering_depth_reliability,
				        aa.dewatering_depth_risk,
				        aa.bio_infection,
				        aa.bio_infection_reliability,
				        aa.bio_infection_risk
                FROM    data.analysis_address AS aa, tracker
                WHERE   aa.id = tracker.building_id
                LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await using var reader = await context.ReaderAsync();

        return MapFromReader(reader);
    }

    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<AnalysisProduct2> GetById2Async(string id)
    {
        var sql = @"
                WITH tracker AS (
                    INSERT INTO application.product_tracker AS pt (organization_id, product, building_id)
                    SELECT
                        @tenant,
	                    'analysis2',
	                    ac.building_id
                    FROM data.analysis_complete ac
                    WHERE ac.building_id = @id
                    LIMIT 1
                    returning pt.building_id
                )
                SELECT -- AnalysisComplete
                        ac.building_id,
                        ac.external_building_id,
                        ac.address_id,
                        ac.address_external_id,
                        ac.neighborhood_id,
                        ac.construction_year,
                        ac.foundation_type,
                        ac.foundation_type_reliability,
                        ac.restoration_costs,
                        ac.drystand_risk,
                        ac.drystand_risk_reliability,
                        ac.bio_infection_risk,
                        ac.bio_infection_risk_reliability,
                        ac.dewatering_depth_risk,
                        ac.dewatering_depth_risk_reliability,
                        ac.unclassified_risk,
                        ac.recovery_type
                FROM    data.analysis_complete ac, tracker
                WHERE   ac.building_id = tracker.building_id
                LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await using var reader = await context.ReaderAsync();

        return MapFromReader2(reader);
    }

    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<AnalysisProduct3> Get3Async(string id)
    {
        var sql = @"
            WITH identifier AS (
                SELECT
                        type,
                        id
                FROM    geocoder.id_parser(@id)
                LIMIT	1
            ),
            tracker AS (
                INSERT INTO application.product_tracker AS pt (organization_id, product, building_id)
                SELECT
                        @tenant,
                        'analysis3',
                        CASE
                            WHEN identifier.type = 'fundermaps' THEN (
                                SELECT-- AnalysisComplete
                                        ac.building_id
                                FROM    data.analysis_complete ac
                                WHERE   ac.building_id = identifier.id
                                LIMIT   1
                            )
                            WHEN identifier.type = 'nl_bag_building' OR identifier.type = 'nl_bag_berth' OR identifier.type = 'nl_bag_posting' THEN (
                                SELECT-- AnalysisComplete
                                        ac.building_id
                                FROM    data.analysis_complete ac
                                WHERE   ac.external_building_id = identifier.id
                                LIMIT   1
                            )
                            WHEN identifier.type = 'nl_bag_address' THEN  (
                                SELECT-- AnalysisComplete
                                        ac.building_id
                                FROM    data.analysis_complete ac
                                WHERE   ac.address_external_id = identifier.id
                                LIMIT   1
                            )
                        END
                FROM    identifier
                LIMIT   1
                RETURNING building_id
            )
            SELECT-- AnalysisComplete
                    ac.building_id,
                    ac.external_building_id,
                    ac.address_id,
                    ac.address_external_id,
                    ac.neighborhood_id,
                    ac.construction_year,
                    ac.construction_year_reliability,
                    ac.foundation_type,
                    ac.foundation_type_reliability,
                    ac.restoration_costs,
                    ac.height,
                    ac.velocity,
                    ac.ground_water_level,
                    ac.ground_level,
                    ac.soil,
                    ac.surface_area,
                    ac.damage_cause,
                    ac.enforcement_term,
                    ac.overall_quality,
                    ac.inquiry_type,
                    ac.drystand,
                    ac.drystand_risk,
                    ac.drystand_risk_reliability,
                    ac.bio_infection_risk,
                    ac.bio_infection_risk_reliability,
                    ac.dewatering_depth,
                    ac.dewatering_depth_risk,
                    ac.dewatering_depth_risk_reliability,
                    ac.unclassified_risk,
                    ac.recovery_type
            FROM    data.analysis_complete ac, tracker
            WHERE   ac.building_id = tracker.building_id
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await using var reader = await context.ReaderAsync();

        return MapFromReader3(reader);
    }

    /// <summary>
    ///     Gets an analysis product by its external building id and source.
    /// </summary>
    /// <param name="id">External building id.</param>
    public async Task<AnalysisProduct> GetByExternalIdAsync(string id)
    {
        var sql = @"
                WITH tracker AS (
		            INSERT INTO application.product_tracker AS pt (organization_id, product, building_id)
		            SELECT
			            @tenant,
			            'analysis',
			            aa.id
		            FROM    data.analysis_address AS aa
		            WHERE   aa.external_id = upper(@external_id)
		            LIMIT 1
		            returning pt.building_id
                )
                SELECT -- AnalysisAddress
				        aa.id,
				        aa.external_id,
				        aa.external_source,
				        aa.construction_year,
				        aa.construction_year_source,
				        aa.address_id,
				        aa.address_external_id,
				        aa.postal_code,
				        aa.neighborhood_id,
				        aa.groundwater_level,
				        aa.soil,
				        aa.building_height,
				        aa.ground_level,
				        aa.cpt,
				        aa.monitoring_well,
				        aa.recovery_advised,
				        aa.damage_cause,
				        aa.substructure,
				        aa.document_name,
				        aa.document_date,
				        aa.inquiry_type,
				        aa.recovery_type,
				        aa.recovery_status,
				        aa.surface_area,
				        aa.living_area,
				        aa.foundation_bearing_layer,
				        aa.restoration_costs,
				        aa.foundation_type,
				        aa.foundation_type_reliability,
				        aa.drystand,
				        aa.drystand_reliability,
				        aa.drystand_risk,
				        aa.dewatering_depth,
				        aa.dewatering_depth_reliability,
				        aa.dewatering_depth_risk,
				        aa.bio_infection,
				        aa.bio_infection_reliability,
				        aa.bio_infection_risk
                FROM    data.analysis_address AS aa, tracker
                WHERE   aa.id = tracker.building_id
                LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("external_id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await using var reader = await context.ReaderAsync();

        return MapFromReader(reader);
    }

    /// <summary>
    ///     Gets an analysis product by its external building id and source.
    /// </summary>
    /// <param name="id">External building id.</param>
    public async Task<AnalysisProduct2> GetByExternalId2Async(string id)
    {
        var sql = @"
                WITH tracker AS (
                    INSERT INTO application.product_tracker AS pt (organization_id, product, building_id)
                    SELECT
                        @tenant,
	                    'analysis2',
	                    ac.building_id
                    FROM data.analysis_complete ac
                    WHERE ac.external_building_id = upper(@external_id)
                    LIMIT 1
                    returning pt.building_id
                )
                SELECT -- AnalysisComplete
                        ac.building_id,
                        ac.external_building_id,
                        ac.address_id,
                        ac.address_external_id,
                        ac.neighborhood_id,
                        ac.construction_year,
                        ac.foundation_type,
                        ac.foundation_type_reliability,
                        ac.restoration_costs,
                        ac.drystand_risk,
                        ac.drystand_risk_reliability,
                        ac.bio_infection_risk,
                        ac.bio_infection_risk_reliability,
                        ac.dewatering_depth_risk,
                        ac.dewatering_depth_risk_reliability,
                        ac.unclassified_risk,
                        ac.recovery_type
                FROM    data.analysis_complete ac, tracker
                WHERE   ac.building_id = tracker.building_id
                LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("external_id", id);
        context.AddParameterWithValue("user", AppContext.UserId);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await using var reader = await context.ReaderAsync();

        return MapFromReader2(reader);
    }

    /// <summary>
    ///     Gets an analysis product by its external address id and source.
    /// </summary>
    /// <param name="id">External address id.</param>
    public async Task<AnalysisProduct> GetByAddressExternalIdAsync(string id)
    {
        var sql = @"
                WITH tracker AS (
		            INSERT INTO application.product_tracker AS pt (organization_id, product, building_id)
		            SELECT
			            @tenant,
			            'analysis',
			            aa.id
		            FROM    data.analysis_address AS aa
		            WHERE   aa.address_external_id = upper(@external_id)
		            LIMIT 1
		            returning pt.building_id
                )
                SELECT -- AnalysisAddress
				        aa.id,
				        aa.external_id,
				        aa.external_source,
				        aa.construction_year,
				        aa.construction_year_source,
				        aa.address_id,
				        aa.address_external_id,
				        aa.postal_code,
				        aa.neighborhood_id,
				        aa.groundwater_level,
				        aa.soil,
				        aa.building_height,
				        aa.ground_level,
				        aa.cpt,
				        aa.monitoring_well,
				        aa.recovery_advised,
				        aa.damage_cause,
				        aa.substructure,
				        aa.document_name,
				        aa.document_date,
				        aa.inquiry_type,
				        aa.recovery_type,
				        aa.recovery_status,
				        aa.surface_area,
				        aa.living_area,
				        aa.foundation_bearing_layer,
				        aa.restoration_costs,
				        aa.foundation_type,
				        aa.foundation_type_reliability,
				        aa.drystand,
				        aa.drystand_reliability,
				        aa.drystand_risk,
				        aa.dewatering_depth,
				        aa.dewatering_depth_reliability,
				        aa.dewatering_depth_risk,
				        aa.bio_infection,
				        aa.bio_infection_reliability,
				        aa.bio_infection_risk
                FROM    data.analysis_address AS aa, tracker
                WHERE   aa.id = tracker.building_id
                LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("external_id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await using var reader = await context.ReaderAsync();

        return MapFromReader(reader);
    }

    /// <summary>
    ///     Gets an analysis product by its external address id and source.
    /// </summary>
    /// <param name="id">External address id.</param>
    public async Task<AnalysisProduct2> GetByAddressExternalId2Async(string id)
    {
        var sql = @"
                WITH tracker AS (
                    INSERT INTO application.product_tracker AS pt (organization_id, product, building_id)
                    SELECT
                        @tenant,
	                    'analysis2',
	                    ac.building_id
                    FROM data.analysis_complete ac
                    WHERE   ac.address_external_id = upper(@external_id)
                    LIMIT 1
                    returning pt.building_id
                )
                SELECT -- AnalysisComplete
                        ac.building_id,
                        ac.external_building_id,
                        ac.address_id,
                        ac.address_external_id,
                        ac.neighborhood_id,
                        ac.construction_year,
                        ac.foundation_type,
                        ac.foundation_type_reliability,
                        ac.restoration_costs,
                        ac.drystand_risk,
                        ac.drystand_risk_reliability,
                        ac.bio_infection_risk,
                        ac.bio_infection_risk_reliability,
                        ac.dewatering_depth_risk,
                        ac.dewatering_depth_risk_reliability,
                        ac.unclassified_risk,
                        ac.recovery_type
                FROM    data.analysis_complete ac, tracker
                WHERE   ac.building_id = tracker.building_id
                LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("external_id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await using var reader = await context.ReaderAsync();

        return MapFromReader2(reader);
    }

    /// <summary>
    ///     Gets the risk index by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<bool> GetRiskIndexAsync(string id)
    {
        var sql = @"
            WITH identifier AS (
	            SELECT
                        type,
                        id
                FROM    geocoder.id_parser(@id)
            ),
            tracker AS (
                INSERT INTO application.product_tracker AS pt (organization_id, product, building_id)
                SELECT
                        @tenant,
	                    'riskindex',
	                    ac.building_id
                FROM    data.analysis_complete ac, identifier
                WHERE
                    CASE
                        WHEN identifier.type = 'fundermaps' THEN ac.building_id = identifier.id
                        WHEN identifier.type = 'nl_bag_address' THEN ac.address_external_id = identifier.id
                        WHEN identifier.type = 'nl_bag_building' THEN ac.external_building_id = identifier.id
                        WHEN identifier.type = 'nl_bag_berth' THEN ac.external_building_id = identifier.id
                        WHEN identifier.type = 'nl_bag_posting' THEN ac.external_building_id = identifier.id
                    END
                LIMIT 1
                RETURNING pt.building_id
            )
            SELECT -- AnalysisComplete
				    'a'::data.foundation_risk_indication <> ANY (ARRAY[
				    CASE
						    WHEN ac.drystand_risk IS NULL THEN 'a'::data.foundation_risk_indication
						    ELSE ac.drystand_risk
				    END,
				    CASE
						    WHEN ac.bio_infection_risk IS NULL THEN 'a'::data.foundation_risk_indication
						    ELSE ac.bio_infection_risk
				    END,
				    CASE
						    WHEN ac.dewatering_depth_risk IS NULL THEN 'a'::data.foundation_risk_indication
						    ELSE ac.dewatering_depth_risk
				    END,
				    CASE
						    WHEN ac.unclassified_risk IS NULL THEN 'a'::data.foundation_risk_indication
						    ELSE ac.unclassified_risk
				    END]) AS has_risk
            FROM    data.analysis_complete ac, tracker
            WHERE   ac.building_id = tracker.building_id
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        return await context.ScalarAsync<bool>();
    }

    /// <summary>
    ///     Maps a reader to an <see cref="AnalysisProduct"/>.
    /// </summary>
    public static AnalysisProduct MapFromReader(DbDataReader reader, int offset = 0)
        => new()
        {
            Id = reader.GetSafeString(offset),
            ExternalId = reader.GetSafeString(offset + 1),
            ExternalSource = reader.GetFieldValue<ExternalDataSource>(offset + 2),
            ConstructionYear = reader.GetDateTime(offset + 3),
            ConstructionYearSource = reader.GetFieldValue<BuiltYearSource>(offset + 4),
            AddressId = reader.GetSafeString(offset + 5),
            AddressExternalId = reader.GetSafeString(offset + 6),
            PostalCode = reader.GetSafeString(offset + 7),
            NeighborhoodId = reader.GetSafeString(offset + 8),
            GroundWaterLevel = reader.GetSafeDouble(offset + 9),
            Soil = reader.GetSafeString(offset + 10),
            BuildingHeight = reader.GetSafeDouble(offset + 11),
            GroundLevel = reader.GetSafeDouble(offset + 12),
            Cpt = reader.GetSafeString(offset + 13),
            MonitoringWell = reader.GetSafeString(offset + 14),
            RecoveryAdvised = reader.GetSafeBoolean(offset + 15),
            DamageCause = reader.GetFieldValue<FoundationDamageCause?>(offset + 16),
            Substructure = reader.GetFieldValue<Substructure?>(offset + 17),
            DocumentName = reader.GetSafeString(offset + 18),
            DocumentDate = reader.GetSafeDateTime(offset + 19),
            InquiryType = reader.GetFieldValue<InquiryType?>(offset + 20),
            RecoveryType = reader.GetFieldValue<RecoveryType?>(offset + 21),
            RecoveryStatus = reader.GetFieldValue<RecoveryStatus?>(offset + 22),
            SurfaceArea = reader.GetSafeDouble(offset + 23),
            LivingArea = reader.GetSafeDouble(offset + 24),
            FoundationBearingLayer = reader.GetSafeDouble(offset + 25),
            RestorationCosts = reader.GetSafeDouble(offset + 26),
            FoundationType = reader.GetFieldValue<FoundationType>(offset + 27),
            FoundationTypeReliability = reader.GetFieldValue<Reliability>(offset + 28),
            Drystand = reader.GetSafeDouble(offset + 29),
            DrystandReliability = reader.GetFieldValue<Reliability>(offset + 30),
            DrystandRisk = reader.GetFieldValue<FoundationRisk>(offset + 31),
            DewateringDepth = reader.GetSafeDouble(offset + 32),
            DewateringDepthReliability = reader.GetFieldValue<Reliability>(offset + 33),
            DewateringDepthRisk = reader.GetFieldValue<FoundationRisk>(offset + 34),
            BioInfection = reader.GetSafeString(offset + 35),
            BioInfectionReliability = reader.GetFieldValue<Reliability>(offset + 36),
            BioInfectionRisk = reader.GetFieldValue<FoundationRisk>(offset + 37),
        };

    /// <summary>
    ///     Maps a reader to an <see cref="AnalysisProduct2"/>.
    /// </summary>
    public static AnalysisProduct2 MapFromReader2(DbDataReader reader, int offset = 0)
        => new()
        {
            BuildingId = reader.GetSafeString(offset++),
            ExternalBuildingId = reader.GetSafeString(offset++),
            AddressId = reader.GetSafeString(offset++),
            ExternalAddressId = reader.GetSafeString(offset++),
            NeighborhoodId = reader.GetSafeString(offset++),
            ConstructionYear = reader.GetSafeInt(offset++),
            FoundationType = reader.GetFieldValue<FoundationType>(offset++),
            FoundationTypeReliability = reader.GetFieldValue<Reliability>(offset++),
            RestorationCosts = reader.GetSafeInt(offset++),
            DrystandRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
            DrystandReliability = reader.GetFieldValue<Reliability>(offset++),
            BioInfectionRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
            BioInfectionReliability = reader.GetFieldValue<Reliability>(offset++),
            DewateringDepthRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
            DewateringDepthReliability = reader.GetFieldValue<Reliability>(offset++),
            UnclassifiedRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
            RecoveryType = reader.GetFieldValue<RecoveryType?>(offset++),
        };

    /// <summary>
    ///     Maps a reader to an <see cref="AnalysisProduct3"/>.
    /// </summary>
    public static AnalysisProduct3 MapFromReader3(DbDataReader reader)
        => new()
        {
            BuildingId = reader.GetSafeString(0),
            ExternalBuildingId = reader.GetSafeString(1),
            AddressId = reader.GetSafeString(2),
            ExternalAddressId = reader.GetSafeString(3),
            NeighborhoodId = reader.GetSafeString(4),
            ConstructionYear = reader.GetSafeInt(5),
            ConstructionYearReliability = reader.GetFieldValue<Reliability>(6),
            FoundationType = reader.GetFieldValue<FoundationType>(7),
            FoundationTypeReliability = reader.GetFieldValue<Reliability>(8),
            RestorationCosts = reader.GetSafeInt(9),
            Height = reader.GetSafeDouble(10),
            Velocity = reader.GetSafeDouble(11),
            GroundWaterLevel = reader.GetSafeDouble(12),
            GroundLevel = reader.GetSafeDouble(13),
            Soil = reader.GetSafeString(14),
            SurfaceArea = reader.GetSafeDouble(15),
            DamageCause = reader.GetFieldValue<FoundationDamageCause?>(16),
            EnforcementTerm = reader.GetFieldValue<EnforcementTerm?>(17),
            OverallQuality = reader.GetFieldValue<Quality?>(18),
            InquiryType = reader.GetFieldValue<InquiryType?>(19),
            Drystand = reader.GetSafeDouble(20),
            DrystandRisk = reader.GetFieldValue<FoundationRisk?>(21),
            DrystandReliability = reader.GetFieldValue<Reliability>(22),
            BioInfectionRisk = reader.GetFieldValue<FoundationRisk?>(23),
            BioInfectionReliability = reader.GetFieldValue<Reliability>(24),
            DewateringDepth = reader.GetSafeDouble(25),
            DewateringDepthRisk = reader.GetFieldValue<FoundationRisk?>(26),
            DewateringDepthReliability = reader.GetFieldValue<Reliability>(27),
            UnclassifiedRisk = reader.GetFieldValue<FoundationRisk?>(28),
            RecoveryType = reader.GetFieldValue<RecoveryType?>(29),
        };
}
