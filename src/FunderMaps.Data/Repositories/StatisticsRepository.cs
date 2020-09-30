using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Data.Repositories
{
    // FUTURE: This casts uint? to uint (and similar). In theory this is safe
    ///     but this is VERY bug sensitive.
    /// <summary>
    ///     Repository for statistics.
    /// </summary>
    internal sealed class StatisticsRepository : DataBase, IStatisticsRepository
    {
        /// <summary>
        ///     Built-in enum to indicate our id method.
        /// </summary>
        private enum IdMethod
        {
            /// <summary>
            ///     Use the code.
            /// </summary>
            NeighborhoodCode,

            /// <summary>
            ///     Use the id.
            /// </summary>
            NeighborhoodId
        }

        public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByExternalIdAsync(Guid userId, string neighborhoodCode)
            => (await ProcessAsync(userId, StatisticsProductType.ConstructionYears, IdMethod.NeighborhoodCode, neighborhoodCode)).ConstructionYearDistribution;

        public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(Guid userId, string neighborhoodId)
            => (await ProcessAsync(userId, StatisticsProductType.ConstructionYears, IdMethod.NeighborhoodId, neighborhoodId)).ConstructionYearDistribution;

        public async Task<double> GetDataCollectedPercentageByExternalIdAsync(Guid userId, string neighborhoodCode)
            => (double)(await ProcessAsync(userId, StatisticsProductType.DataCollected, IdMethod.NeighborhoodCode, neighborhoodCode)).DataCollectedPercentage;

        public async Task<double> GetDataCollectedPercentageByIdAsync(Guid userId, string neighborhoodId)
            => (double)(await ProcessAsync(userId, StatisticsProductType.DataCollected, IdMethod.NeighborhoodId, neighborhoodId)).DataCollectedPercentage;

        public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByExternalIdAsync(Guid userId, string neighborhoodCode)
            => (await ProcessAsync(userId, StatisticsProductType.FoundationRisk, IdMethod.NeighborhoodCode, neighborhoodCode)).FoundationRiskDistribution;

        public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(Guid userId, string neighborhoodId)
            => (await ProcessAsync(userId, StatisticsProductType.FoundationRisk, IdMethod.NeighborhoodId, neighborhoodId)).FoundationRiskDistribution;

        public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByExternalIdAsync(Guid userId, string neighborhoodCode)
            => (await ProcessAsync(userId, StatisticsProductType.FoundationRatio, IdMethod.NeighborhoodCode, neighborhoodCode)).FoundationTypeDistribution;

        public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(Guid userId, string neighborhoodId)
            => (await ProcessAsync(userId, StatisticsProductType.FoundationRatio, IdMethod.NeighborhoodId, neighborhoodId)).FoundationTypeDistribution;

        public async Task<uint> GetTotalBuildingRestoredCountByExternalIdAsync(Guid userId, string neighborhoodCode)
            => (uint)(await ProcessAsync(userId, StatisticsProductType.BuildingsRestored, IdMethod.NeighborhoodCode, neighborhoodCode)).TotalBuildingRestoredCount;

        public async Task<uint> GetTotalBuildingRestoredCountByIdAsync(Guid userId, string neighborhoodId)
            => (uint)(await ProcessAsync(userId, StatisticsProductType.BuildingsRestored, IdMethod.NeighborhoodId, neighborhoodId)).TotalBuildingRestoredCount;

        public async Task<uint> GetTotalIncidentCountByExternalIdAsync(Guid userId, string neighborhoodCode)
            => (uint)(await ProcessAsync(userId, StatisticsProductType.Incidents, IdMethod.NeighborhoodCode, neighborhoodCode)).TotalIncidentCount;

        public async Task<uint> GetTotalIncidentCountByIdAsync(Guid userId, string neighborhoodId)
            => (uint)(await ProcessAsync(userId, StatisticsProductType.Incidents, IdMethod.NeighborhoodId, neighborhoodId)).TotalIncidentCount;

        public async Task<uint> GetTotalReportCountByExternalIdAsync(Guid userId, string neighborhoodCode)
            => (uint)(await ProcessAsync(userId, StatisticsProductType.Reports, IdMethod.NeighborhoodCode, neighborhoodCode)).TotalReportCount;

        public async Task<uint> GetTotalReportCountByIdAsync(Guid userId, string neighborhoodId)
            => (uint)(await ProcessAsync(userId, StatisticsProductType.Reports, IdMethod.NeighborhoodId, neighborhoodId)).TotalReportCount;


        // TODO This seems a bit hacky. The format might change in the future when we decide to change the process of getting the data.
        /// <summary>
        ///     Wrapper function to handle every case we have.
        /// </summary>
        /// <remarks>
        ///     The returned <see cref="StatisticsProduct"/> has null values
        ///     for all fields that were not requested.
        /// </remarks>
        /// <param name="identifier">Neighborhood id or code</param>
        private async Task<StatisticsProduct> ProcessAsync(Guid userId, StatisticsProductType product, IdMethod method, string identifier)
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

            // TODO: Write out columns in the select.
            // Note: This uses a CTE for proper index usage. 
            var sql = $@"
                WITH id_cte AS (
                    SELECT id
                    FROM geocoder.neighborhood
                    WHERE {GetIdColumnName(method)} = @identifier
                    AND application.is_geometry_in_fence(@user_id, geom)
                    LIMIT 1
                )
                SELECT 
                    s.*
                FROM data.{GetTable(product)} AS s
                WHERE s.neighborhood_id = (SELECT * FROM id_cte)";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("identifier", identifier);
            context.AddParameterWithValue("user_id", userId);

            await using var reader = await context.ReaderAsync(readAhead: true, hasRowsGuard: false);

            // Map and return.
            return new StatisticsProduct
            {
                ConstructionYearDistribution = product == StatisticsProductType.ConstructionYears ? await MapConstructionYearDistributionAsync(reader) : null,
                DataCollectedPercentage = product == StatisticsProductType.DataCollected ? await MapDataCollectedAsync(reader) : (double?)null,
                FoundationRiskDistribution = product == StatisticsProductType.FoundationRisk ? await MapFoundationRiskDistributionAsync(reader) : null,
                FoundationTypeDistribution = product == StatisticsProductType.FoundationRatio ? await MapFoundationTypeDistributionAsync(reader) : null,
                NeighborhoodCode = method == IdMethod.NeighborhoodCode ? identifier : null,
                NeighborhoodId = method == IdMethod.NeighborhoodId ? identifier : null,
                TotalBuildingRestoredCount = product == StatisticsProductType.BuildingsRestored ? await MapBuildingsRestoredAsync(reader) : (uint?)null,
                TotalIncidentCount = product == StatisticsProductType.Incidents ? await MapIncidentsAsync(reader) : (uint?)null,
                TotalReportCount = product == StatisticsProductType.Reports ? await MapInquiriesAsync(reader) : (uint?)null,
            };
        }

        #region Mapping

        /// <summary>
        ///     Gets a <see cref="ConstructionYearDistribution"/> from a <paramref name="reader"/>.
        /// </summary>
        private async Task<ConstructionYearDistribution> MapConstructionYearDistributionAsync(DbDataReader reader)
        {
            // FUTURE: Make functional
            var pairs = new List<ConstructionYearPair>();

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
        ///     Gets the amount of buildings restored from a <paramref name="reader"/>.
        /// </summary>
        /// <returns>Amount of buildings restored.</returns>
        private static async Task<uint> MapBuildingsRestoredAsync(DbDataReader reader, CancellationToken token = default)
        {
            await reader.ReadAsync(token);
            return reader.GetUInt(1);
        }

        /// <summary>
        ///     Gets the amount of data collected from a <paramref name="reader"/>.
        /// </summary>
        /// <returns>Percentage of data collected.</returns>
        private async Task<double> MapDataCollectedAsync(DbDataReader reader)
        {
            await reader.ReadAsync(AppContext.CancellationToken);
            return reader.GetDouble(1);
        }

        // FUTURE: Clean up, this is ugly.
        /// <summary>
        ///     Gets a <see cref="FoundationRiskDistribution"/> from a <paramref name="reader"/>.
        /// </summary>
        private async Task<FoundationRiskDistribution> MapFoundationRiskDistributionAsync(DbDataReader reader)
        {
            var map = new Dictionary<FoundationRisk, double>
            {
                { FoundationRisk.A, 0 },
                { FoundationRisk.B, 0 },
                { FoundationRisk.C, 0 },
                { FoundationRisk.D, 0 },
                { FoundationRisk.E, 0 }
            };

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
        ///     Gets a <see cref="FoundationTypeDistribution"/> from a <paramref name="reader"/>.
        /// </summary>
        private async Task<FoundationTypeDistribution> MapFoundationTypeDistributionAsync(DbDataReader reader)
        {
            // FUTURE: Make functional
            var pairs = new List<FoundationTypePair>();

            while (await reader.ReadAsync(AppContext.CancellationToken))
            {
                pairs.Add(new FoundationTypePair
                {
                    FoundationType = reader.GetFieldValue<FoundationType>(1),
                    Percentage = reader.GetDouble(2)
                });
            }

            return new FoundationTypeDistribution
            {
                FoundationTypes = pairs
            };
        }

        /// <summary>
        ///     Gets the amount of incidents from a <paramref name="reader"/>.
        /// </summary>
        /// <returns>Amount of incidents.</returns>
        private async Task<uint> MapIncidentsAsync(DbDataReader reader)
        {
            await reader.ReadAsync(AppContext.CancellationToken);
            return reader.GetUInt(1);
        }

        /// <summary>
        ///     Gets the amount of inquiries from a <paramref name="reader"/>.
        /// </summary>
        /// <returns>Amount of inquiries.</returns>
        private async Task<uint> MapInquiriesAsync(DbDataReader reader)
        {
            await reader.ReadAsync(AppContext.CancellationToken);
            return reader.GetUInt(1);
        }

        #endregion Mapping
    }
}
#pragma warning disable CA1812 // Internal class is never instantiated
