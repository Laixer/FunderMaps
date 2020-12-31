using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Data.Repositories
{
    // TODO: Move most if not all logic to db.
    // FUTURE: This casts uint? to uint (and similar). In theory this is safe
    ///     but this is VERY bug sensitive.
    /// <summary>
    ///     Repository for statistics.
    /// </summary>
    internal sealed class StatisticsRepository : DbContextBase, IStatisticsRepository
    {
        /// <summary>
        ///     Built-in enum to indicate our id method.
        /// </summary>
        private enum IdMethod
        {
            /// <summary>
            ///     Use the code.
            /// </summary>
            NeighborhoodCode = 0,

            /// <summary>
            ///     Use the id.
            /// </summary>
            NeighborhoodId = 1,
        }

        /// <summary>
        ///     Fill all statistic fields if required.
        /// </summary>
        public async Task<StatisticsProduct> GetStatisticsProductByIdAsync(string id)
            => new()
            {
                FoundationTypeDistribution = await GetFoundationTypeDistributionByIdAsync(id),
                ConstructionYearDistribution = await GetConstructionYearDistributionByIdAsync(id),
                FoundationRiskDistribution = await GetFoundationRiskDistributionByIdAsync(id),
                DataCollectedPercentage = await GetDataCollectedPercentageByIdAsync(id),
                TotalBuildingRestoredCount = await GetTotalBuildingRestoredCountByIdAsync(id),
                TotalIncidentCount = await GetTotalIncidentCountByIdAsync(id),
                TotalReportCount = await GetTotalReportCountByIdAsync(id),
            };

        /// <summary>
        ///     Fill all statistic fields if required.
        /// </summary>
        public async Task<StatisticsProduct> GetStatisticsProductByExternalIdAsync(string id)
            => new()
            {
                FoundationTypeDistribution = await GetFoundationTypeDistributionByExternalIdAsync(id),
                ConstructionYearDistribution = await GetConstructionYearDistributionByExternalIdAsync(id),
                FoundationRiskDistribution = await GetFoundationRiskDistributionByExternalIdAsync(id),
                DataCollectedPercentage = await GetDataCollectedPercentageByExternalIdAsync(id),
                TotalBuildingRestoredCount = await GetTotalBuildingRestoredCountByExternalIdAsync(id),
                TotalIncidentCount = await GetTotalIncidentCountByExternalIdAsync(id),
                TotalReportCount = await GetTotalReportCountByExternalIdAsync(id),
            };

        public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByExternalIdAsync(string neighborhoodCode)
            => (await ProcessAsync(StatisticsProductType.ConstructionYears, IdMethod.NeighborhoodCode, neighborhoodCode)).ConstructionYearDistribution;

        public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(string neighborhoodId)
            => (await ProcessAsync(StatisticsProductType.ConstructionYears, IdMethod.NeighborhoodId, neighborhoodId)).ConstructionYearDistribution;

        public async Task<double> GetDataCollectedPercentageByExternalIdAsync(string neighborhoodCode)
            => (double)(await ProcessAsync(StatisticsProductType.DataCollected, IdMethod.NeighborhoodCode, neighborhoodCode)).DataCollectedPercentage;

        public async Task<double> GetDataCollectedPercentageByIdAsync(string neighborhoodId)
            => (double)(await ProcessAsync(StatisticsProductType.DataCollected, IdMethod.NeighborhoodId, neighborhoodId)).DataCollectedPercentage;

        public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByExternalIdAsync(string neighborhoodCode)
            => (await ProcessAsync(StatisticsProductType.FoundationRisk, IdMethod.NeighborhoodCode, neighborhoodCode)).FoundationRiskDistribution;

        public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(string neighborhoodId)
            => (await ProcessAsync(StatisticsProductType.FoundationRisk, IdMethod.NeighborhoodId, neighborhoodId)).FoundationRiskDistribution;

        public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByExternalIdAsync(string neighborhoodCode)
            => (await ProcessAsync(StatisticsProductType.FoundationRatio, IdMethod.NeighborhoodCode, neighborhoodCode)).FoundationTypeDistribution;

        public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(string neighborhoodId)
            => (await ProcessAsync(StatisticsProductType.FoundationRatio, IdMethod.NeighborhoodId, neighborhoodId)).FoundationTypeDistribution;

        public async Task<uint> GetTotalBuildingRestoredCountByExternalIdAsync(string neighborhoodCode)
            => (uint)(await ProcessAsync(StatisticsProductType.BuildingsRestored, IdMethod.NeighborhoodCode, neighborhoodCode)).TotalBuildingRestoredCount;

        public async Task<uint> GetTotalBuildingRestoredCountByIdAsync(string neighborhoodId)
            => (uint)(await ProcessAsync(StatisticsProductType.BuildingsRestored, IdMethod.NeighborhoodId, neighborhoodId)).TotalBuildingRestoredCount;

        public async Task<uint> GetTotalIncidentCountByExternalIdAsync(string neighborhoodCode)
            => (uint)(await ProcessAsync(StatisticsProductType.Incidents, IdMethod.NeighborhoodCode, neighborhoodCode)).TotalIncidentCount;

        public async Task<uint> GetTotalIncidentCountByIdAsync(string neighborhoodId)
            => (uint)(await ProcessAsync(StatisticsProductType.Incidents, IdMethod.NeighborhoodId, neighborhoodId)).TotalIncidentCount;

        public async Task<uint> GetTotalReportCountByExternalIdAsync(string neighborhoodCode)
            => (uint)(await ProcessAsync(StatisticsProductType.Reports, IdMethod.NeighborhoodCode, neighborhoodCode)).TotalReportCount;

        public async Task<uint> GetTotalReportCountByIdAsync(string neighborhoodId)
            => (uint)(await ProcessAsync(StatisticsProductType.Reports, IdMethod.NeighborhoodId, neighborhoodId)).TotalReportCount;

        // TODO This seems a bit hacky. The format might change in the future when we decide to change the process of getting the data.
        /// <summary>
        ///     Wrapper function to handle every case we have.
        /// </summary>
        /// <remarks>
        ///     The returned <see cref="StatisticsProduct"/> has null values
        ///     for all fields that were not requested.
        /// </remarks>
        /// <param name="product">Product type.</param>
        /// <param name="method">Query method.</param>
        /// <param name="identifier">Neighborhood id or code.</param>
        private async Task<StatisticsProduct> ProcessAsync(StatisticsProductType product, IdMethod method, string identifier)
        {
            static string GetTable(StatisticsProductType product)
                => product switch
                {
                    StatisticsProductType.FoundationRatio => "statistics_product_foundation_type",
                    StatisticsProductType.ConstructionYears => "statistics_product_construction_years",
                    StatisticsProductType.FoundationRisk => "statistics_product_foundation_risk",
                    StatisticsProductType.DataCollected => "statistics_product_data_collected",
                    StatisticsProductType.BuildingsRestored => "statistics_product_buildings_restored",
                    StatisticsProductType.Incidents => "statistics_product_incidents",
                    StatisticsProductType.Reports => "statistics_product_inquiries",
                    _ => throw new InvalidOperationException(nameof(product))
                };

            static string GetIdColumnName(IdMethod method)
                => method switch
                {
                    IdMethod.NeighborhoodCode => "external_id",
                    IdMethod.NeighborhoodId => "id",
                    _ => throw new InvalidOperationException(nameof(method))
                };

            var sql1 = "application.is_geometry_in_fence(@user_id, geom)";
            var sql2 = "1=1";

            // TODO: Write out columns in the select.
            // Note: This uses a CTE for proper index usage. 
            var sql = $@"
                WITH id_cte AS (
                    SELECT id
                    FROM geocoder.neighborhood
                    WHERE {GetIdColumnName(method)} = @identifier
                    AND  {(AppContext.HasIdentity ? sql1 : sql2)}
                    LIMIT 1
                )
                SELECT 
                    s.*
                FROM data.{GetTable(product)} AS s
                WHERE s.neighborhood_id = (SELECT * FROM id_cte)";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("identifier", identifier);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            // await using var reader = await context.ReaderAsync(readAhead: true, hasRowsGuard: false);

            // Map and return.
            return new()
            {
                ConstructionYearDistribution = product == StatisticsProductType.ConstructionYears ? await MapConstructionYearDistributionAsync(context) : null,
                DataCollectedPercentage = product == StatisticsProductType.DataCollected ? await MapDataCollectedAsync(context) : (double?)null,
                FoundationRiskDistribution = product == StatisticsProductType.FoundationRisk ? await MapFoundationRiskDistributionAsync(context) : null,
                FoundationTypeDistribution = product == StatisticsProductType.FoundationRatio ? await MapFoundationTypeDistributionAsync(context) : null,
                TotalBuildingRestoredCount = product == StatisticsProductType.BuildingsRestored ? await MapBuildingsRestoredAsync(context) : (uint?)null,
                TotalIncidentCount = product == StatisticsProductType.Incidents ? await MapIncidentsAsync(context) : (uint?)null,
                TotalReportCount = product == StatisticsProductType.Reports ? await MapInquiriesAsync(context) : (uint?)null,
            };
        }

        #region Mapping

        /// <summary>
        ///     Gets a <see cref="ConstructionYearDistribution"/>.
        /// </summary>
        private async Task<ConstructionYearDistribution> MapConstructionYearDistributionAsync(DbContext context)
        {
            // FUTURE: Make functional
            var pairs = new List<ConstructionYearPair>();

            await using var reader = await context.ReaderAsync(readAhead: false, hasRowsGuard: false);
            while (await reader.ReadAsync(AppContext.CancellationToken))
            {
                pairs.Add(new ConstructionYearPair
                {
                    Decade = Years.FromDecade(reader.GetInt(1)),
                    TotalCount = reader.GetUInt(2)
                });
            }

            return new ConstructionYearDistribution
            {
                Decades = pairs
            };
        }

        /// <summary>
        ///     Gets the amount of buildings restored.
        /// </summary>
        /// <returns>Amount of buildings restored.</returns>
        private static async Task<uint> MapBuildingsRestoredAsync(DbContext context)
        {
            await using var reader = await context.ReaderAsync();
            return reader.GetUInt(1);
        }

        /// <summary>
        ///     Gets the amount of data collected.
        /// </summary>
        /// <returns>Percentage of data collected.</returns>
        private static async Task<double> MapDataCollectedAsync(DbContext context)
        {
            await using var reader = await context.ReaderAsync();
            return reader.GetDouble(1);
        }

        // FUTURE: Clean up, this is ugly.
        /// <summary>
        ///     Gets a <see cref="FoundationRiskDistribution"/>.
        /// </summary>
        private async Task<FoundationRiskDistribution> MapFoundationRiskDistributionAsync(DbContext context)
        {
            Dictionary<FoundationRisk, double> map = new()
            {
                { FoundationRisk.A, 0 },
                { FoundationRisk.B, 0 },
                { FoundationRisk.C, 0 },
                { FoundationRisk.D, 0 },
                { FoundationRisk.E, 0 }
            };

            await using var reader = await context.ReaderAsync(readAhead: false, hasRowsGuard: false);
            while (await reader.ReadAsync(AppContext.CancellationToken))
            {
                map[reader.GetFieldValue<FoundationRisk>(1)] = reader.GetDouble(2);
            }

            return new FoundationRiskDistribution
            {
                PercentageA = map[FoundationRisk.A],
                PercentageB = map[FoundationRisk.B],
                PercentageC = map[FoundationRisk.C],
                PercentageD = map[FoundationRisk.D],
                PercentageE = map[FoundationRisk.E]
            };
        }

        /// <summary>
        ///     Gets a <see cref="FoundationTypeDistribution"/>.
        /// </summary>
        private async Task<FoundationTypeDistribution> MapFoundationTypeDistributionAsync(DbContext context)
        {
            // FUTURE: Make functional
            var pairs = new List<FoundationTypePair>();

            await using var reader = await context.ReaderAsync(readAhead: false, hasRowsGuard: false);
            while (await reader.ReadAsync(AppContext.CancellationToken))
            {
                pairs.Add(new FoundationTypePair
                {
                    FoundationType = reader.GetFieldValue<FoundationType>(1),
                    Percentage = reader.GetDouble(2)
                });
            }

            return new()
            {
                FoundationTypes = pairs
            };
        }

        /// <summary>
        ///     Gets the amount of incidents.
        /// </summary>
        /// <returns>Amount of incidents.</returns>
        private static async Task<uint> MapIncidentsAsync(DbContext context)
        {
            await using var reader = await context.ReaderAsync();
            return reader.GetUInt(1);
        }

        /// <summary>
        ///     Gets the amount of inquiries.
        /// </summary>
        /// <returns>Amount of inquiries.</returns>
        private static async Task<uint> MapInquiriesAsync(DbContext context)
        {
            await using var reader = await context.ReaderAsync();
            return reader.GetUInt(1);
        }

        #endregion Mapping
    }
}
#pragma warning disable CA1812 // Internal class is never instantiated
