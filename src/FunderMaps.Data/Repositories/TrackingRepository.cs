using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    // TODO: Rename to TelemetryRepository
    /// <summary>
    ///     User tracking repository. This has a minimum item count of 1, as
    ///     we also want to track requests that return nothing.
    /// </summary>
    internal class TrackingRepository : DataBase, ITrackingRepository
    {
        /// <summary>
        ///     Process analysis product usage. The minimum item count is 1,
        ///     as we also want to track requests that return nothing.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="itemCount">Result set item count.</param>
        public Task ProcessAnalysisUsageAsync(Guid userId, AnalysisProductType product, uint itemCount = 1)
            => ProcessUsageAsync(userId, MapAnalysisProduct(product), itemCount);

        /// <summary>
        ///     Process statistics product usage. The minimum item count is 1,
        ///     as we also want to track requests that return nothing.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="itemCount">Result set item count.</param>
        public Task ProcessStatisticsUsageAsync(Guid userId, StatisticsProductType product, uint itemCount = 1)
            => ProcessUsageAsync(userId, MapStatisticsProduct(product), itemCount);

        /// <summary>
        ///     Processes a user tracking request.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="itemCount"/> will be set to one if it's zero.
        ///     Map the <see cref="AnalysisProduct"/> or <see cref="StatisticsProduct"/>
        ///     using <see cref="MapAnalysisProduct(AnalysisProductType)"/> or
        ///     <see cref="MapStatisticsProduct(StatisticsProductType)"/>.
        /// </remarks>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productMapped">Textual enum representation</param>
        /// <param name="itemCount">Amount of items returned by product</param>
        protected async Task ProcessUsageAsync(Guid userId, string productMapped, uint itemCount)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            productMapped.ThrowIfNullOrEmpty();

            // If a product returned 0 items, we still queried the DB. Mark this as 1.
            itemCount = itemCount == 0U ? 1U : itemCount;

            // Build sql.
            var sql = @"
                INSERT INTO application.product_usage AS pu (
                    user_id,
                    product,
                    count)
                VALUES (                    
                    @user_id,
                    @product::application.product,
                    @count)
                ON CONFLICT (user_id, product)
                DO UPDATE SET
                    count = pu.count + EXCLUDED.count";

            // Execute sql.
            await using var connection = await DbProvider.OpenConnectionScopeAsync(AppContext.CancellationToken);
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            cmd.AddParameterWithValue("user_id", userId);
            cmd.AddParameterWithValue("product", productMapped);
            cmd.AddParameterWithValue("count", (int)itemCount);

            await cmd.ExecuteNonQueryEnsureAffectedAsync(AppContext.CancellationToken);
        }

        /// <summary>
        ///     Maps a <see cref="AnalysisProductType"/> to the textual
        ///     representation of the database enum.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProductType"/></param>
        /// <returns>Textual enum representation.</returns>
        private static string MapAnalysisProduct(AnalysisProductType product)
            => product switch
            {
                AnalysisProductType.BuildingData => "analysis_building_data",
                AnalysisProductType.Foundation => "analysis_foundation",
                AnalysisProductType.FoundationPlus => "analysis_foundation_plus",
                AnalysisProductType.Costs => "analysis_costs",
                AnalysisProductType.Complete => "analysis_complete",
                AnalysisProductType.Risk => "analysis_risk",
                _ => throw new InvalidOperationException(nameof(product))
            };

        /// <summary>
        ///     Maps a <see cref="StatisticsProductType"/> to the textual
        ///     representation of the database enum.
        /// </summary>
        /// <param name="product"><see cref="StatisticsProductType"/></param>
        /// <returns>Textual enum representation.</returns>
        private static string MapStatisticsProduct(StatisticsProductType product)
            => product switch
            {
                StatisticsProductType.FoundationRatio => "statistics_foundation_ratio",
                StatisticsProductType.ConstructionYears => "statistics_construction_years",
                StatisticsProductType.FoundationRisk => "statistics_foundation_risk",
                StatisticsProductType.DataCollected => "statistics_data_collected",
                StatisticsProductType.BuildingsRestored => "statistics_buildings_restored",
                StatisticsProductType.Incidents => "statistics_incidents",
                StatisticsProductType.Reports => "statistics_reports",
                _ => throw new InvalidOperationException(nameof(product))
            };
    }
}
