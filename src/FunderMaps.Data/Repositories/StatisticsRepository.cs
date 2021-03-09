using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Data.Repositories
{
    // FUTURE: Change function names and view names.

    /// <summary>
    ///     Repository for statistics.
    /// </summary>
    internal sealed class StatisticsRepository : DbServiceBase, IStatisticsRepository
    {
        /// <summary>
        ///     Get foundation type distribution by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(string id)
        {
            var sql = @"
                SELECT  -- FoundationTypeDistribution
                        spft.foundation_type,
                        round(spft.percentage::numeric, 2)
                FROM    data.statistics_product_foundation_type AS spft
                JOIN    geocoder.neighborhood n ON n.id = spft.neighborhood_id 
                WHERE   n.id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            List<FoundationTypePair> pairs = new();
            await foreach (var reader in context.EnumerableReaderAsync())
            {
                pairs.Add(new()
                {
                    FoundationType = reader.GetFieldValue<FoundationType>(0),
                    Percentage = reader.GetDecimal(1)
                });
            }

            return new()
            {
                FoundationTypes = pairs
            };
        }

        /// <summary>
        ///     Get foundation type distribution by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByExternalIdAsync(string id)
        {
            var sql = @"
                SELECT  -- FoundationTypeDistribution
                        spft.foundation_type,
                        round(spft.percentage::numeric, 2)
                FROM    data.statistics_product_foundation_type AS spft
                JOIN    geocoder.neighborhood n ON n.id = spft.neighborhood_id 
                WHERE   n.external_id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            List<FoundationTypePair> pairs = new();
            await foreach (var reader in context.EnumerableReaderAsync())
            {
                pairs.Add(new()
                {
                    FoundationType = reader.GetFieldValue<FoundationType>(0),
                    Percentage = reader.GetDecimal(1)
                });
            }

            return new()
            {
                FoundationTypes = pairs
            };
        }

        /// <summary>
        ///     Get construction year distribution by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(string id)
        {
            var sql = @"
                SELECT  -- ConstructionYearDistribution
                        spcy.year_from,
                        spcy.count
                FROM    data.statistics_product_construction_years AS spcy
                JOIN    geocoder.neighborhood n ON n.id = spcy.neighborhood_id 
                WHERE   n.id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            List<ConstructionYearPair> pairs = new();
            await foreach (var reader in context.EnumerableReaderAsync())
            {
                pairs.Add(new()
                {
                    Decade = Years.FromDecade(reader.GetInt(0)),
                    TotalCount = reader.GetInt(1)
                });
            }

            return new()
            {
                Decades = pairs
            };
        }

        /// <summary>
        ///     Get construction year distribution by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByExternalIdAsync(string id)
        {
            var sql = @"
                SELECT  -- ConstructionYearDistribution
                        spcy.year_from,
                        spcy.count
                FROM    data.statistics_product_construction_years AS spcy
                JOIN    geocoder.neighborhood n ON n.id = spcy.neighborhood_id 
                WHERE   n.external_id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            List<ConstructionYearPair> pairs = new();
            await foreach (var reader in context.EnumerableReaderAsync())
            {
                pairs.Add(new()
                {
                    Decade = Years.FromDecade(reader.GetInt(0)),
                    TotalCount = reader.GetInt(1)
                });
            }

            return new()
            {
                Decades = pairs
            };
        }

        /// <summary>
        ///     Get data collection percentage by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<decimal> GetDataCollectedPercentageByIdAsync(string id)
        {
            var sql = @"
                SELECT  -- DataCollected
                        round(spdc.percentage::numeric, 2)
                FROM    data.statistics_product_data_collected AS spdc
                JOIN    geocoder.neighborhood n ON n.id = spdc.neighborhood_id
                WHERE   n.id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            sql += $"\r\n LIMIT 1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            return await context.ScalarAsync<decimal>(resultGuard: false);
        }

        /// <summary>
        ///     Get data collection percentage by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<decimal> GetDataCollectedPercentageByExternalIdAsync(string id)
        {
            var sql = @"
                SELECT  -- DataCollected
                        round(spdc.percentage::numeric, 2)
                FROM    data.statistics_product_data_collected AS spdc
                JOIN    geocoder.neighborhood n ON n.id = spdc.neighborhood_id
                WHERE   n.external_id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            sql += $"\r\n LIMIT 1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            return await context.ScalarAsync<decimal>(resultGuard: false);
        }

        /// <summary>
        ///     Get foundation risk distribution by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(string id)
        {
            var sql = @"
                SELECT  -- FoundationRiskDistribution
                        spfr.foundation_risk,
                        round(spfr.percentage::numeric, 2)
                FROM    data.statistics_product_foundation_risk AS spfr
                JOIN    geocoder.neighborhood n ON n.id = spfr.neighborhood_id 
                WHERE   n.id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            Dictionary<FoundationRisk, decimal> map = new()
            {
                { FoundationRisk.A, 0 },
                { FoundationRisk.B, 0 },
                { FoundationRisk.C, 0 },
                { FoundationRisk.D, 0 },
                { FoundationRisk.E, 0 }
            };

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                map[reader.GetFieldValue<FoundationRisk>(0)] = reader.GetDecimal(1);
            }

            return new()
            {
                PercentageA = map[FoundationRisk.A],
                PercentageB = map[FoundationRisk.B],
                PercentageC = map[FoundationRisk.C],
                PercentageD = map[FoundationRisk.D],
                PercentageE = map[FoundationRisk.E]
            };
        }

        /// <summary>
        ///     Get foundation risk distribution by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByExternalIdAsync(string id)
        {
            var sql = @"
                SELECT  -- FoundationRiskDistribution
                        spfr.foundation_risk,
                        round(spfr.percentage::numeric, 2)
                FROM    data.statistics_product_foundation_risk AS spfr
                JOIN    geocoder.neighborhood n ON n.id = spfr.neighborhood_id 
                WHERE   n.external_id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            Dictionary<FoundationRisk, decimal> map = new()
            {
                { FoundationRisk.A, 0 },
                { FoundationRisk.B, 0 },
                { FoundationRisk.C, 0 },
                { FoundationRisk.D, 0 },
                { FoundationRisk.E, 0 }
            };

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                map[reader.GetFieldValue<FoundationRisk>(0)] = reader.GetDecimal(1);
            }

            return new()
            {
                PercentageA = map[FoundationRisk.A],
                PercentageB = map[FoundationRisk.B],
                PercentageC = map[FoundationRisk.C],
                PercentageD = map[FoundationRisk.D],
                PercentageE = map[FoundationRisk.E]
            };
        }

        /// <summary>
        ///     Get total building restored count by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<long> GetTotalBuildingRestoredCountByIdAsync(string id)
        {
            var sql = @"
                SELECT  -- BuildingRestoredCount
                        spbr.count
                FROM    data.statistics_product_buildings_restored AS spbr
                JOIN    geocoder.neighborhood n ON n.id = spbr.neighborhood_id
                WHERE   n.id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            sql += $"\r\n LIMIT 1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            return await context.ScalarAsync<long>(resultGuard: false);
        }

        /// <summary>
        ///     Get total building restored count by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<long> GetTotalBuildingRestoredCountByExternalIdAsync(string id)
        {
            var sql = @"
                SELECT  -- BuildingRestoredCount
                        spbr.count
                FROM    data.statistics_product_buildings_restored AS spbr
                JOIN    geocoder.neighborhood n ON n.id = spbr.neighborhood_id
                WHERE   n.external_id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            sql += $"\r\n LIMIT 1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            return await context.ScalarAsync<long>(resultGuard: false);
        }

        /// <summary>
        ///     Get total incident count by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<IEnumerable<IncidentYearPair>> GetTotalIncidentCountByIdAsync(string id)
        {
            var sql = @"
                SELECT  -- IncidentCount
                        spi.year,
                        spi.count
                FROM    data.statistics_product_incidents AS spi
                JOIN    geocoder.neighborhood n ON n.id = spi.neighborhood_id
                WHERE   n.id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            List<IncidentYearPair> pairs = new();
            await foreach (var reader in context.EnumerableReaderAsync())
            {
                pairs.Add(new()
                {
                    Year = reader.GetInt(0),
                    TotalCount = reader.GetInt(1)
                });
            }

            return pairs;
        }

        /// <summary>
        ///     Get total incident count by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<IEnumerable<IncidentYearPair>> GetTotalIncidentCountByExternalIdAsync(string id)
        {
            var sql = @"
                SELECT  -- IncidentCount
                        spi.year,
                        spi.count
                FROM    data.statistics_product_incidents AS spi
                JOIN    geocoder.neighborhood n ON n.id = spi.neighborhood_id
                WHERE   n.external_id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            List<IncidentYearPair> pairs = new();
            await foreach (var reader in context.EnumerableReaderAsync())
            {
                pairs.Add(new()
                {
                    Year = reader.GetInt(0),
                    TotalCount = reader.GetInt(1)
                });
            }

            return pairs;
        }

        /// <summary>
        ///     Get total incident count by id.
        /// </summary>
        /// <param name="id">Municipality identifier.</param>
        public async Task<IEnumerable<IncidentYearPair>> GetMunicipalityIncidentCountByIdAsync(string id)
        {
            // FUTURE: Maybe move query to db?
            var sql = @"
				SELECT  -- Incident
                        year.year, 
                        count(i.id) AS count
                FROM    report.incident i
                JOIN	geocoder.address a ON i.address::text = a.id::text
                JOIN	geocoder.building b ON a.building_id::text = b.id::text
                JOIN	geocoder.neighborhood n ON n.id::text = b.neighborhood_id::text
                JOIN	geocoder.district d ON d.id::text = n.district_id::text
                JOIN	geocoder.municipality m ON m.id = d.municipality_id,
                LATERAL CAST(date_part('year'::text, i.create_date)::integer AS integer) year(year)
                WHERE	n.id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, m.geom)";
            }

            sql += $"\r\n GROUP BY m.id, year.year";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            List<IncidentYearPair> pairs = new();
            await foreach (var reader in context.EnumerableReaderAsync())
            {
                pairs.Add(new()
                {
                    Year = reader.GetInt(0),
                    TotalCount = reader.GetInt(1)
                });
            }

            return pairs;
        }

        /// <summary>
        ///     Get total incident count by external id.
        /// </summary>
        /// <param name="id">Municipality identifier.</param>
        public async Task<IEnumerable<IncidentYearPair>> GetMunicipalityIncidentCountByExternalIdAsync(string id)
        {
            // FUTURE: Maybe move query to db?
            var sql = @"
				SELECT  -- Incident
                        year.year, 
                        count(i.id) AS count
                FROM    report.incident i
                JOIN	geocoder.address a ON i.address::text = a.id::text
                JOIN	geocoder.building b ON a.building_id::text = b.id::text
                JOIN	geocoder.neighborhood n ON n.id::text = b.neighborhood_id::text
                JOIN	geocoder.district d ON d.id::text = n.district_id::text
                JOIN	geocoder.municipality m ON m.id = d.municipality_id,
                LATERAL CAST(date_part('year'::text, i.create_date)::integer AS integer) year(year)
                WHERE	n.external_id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, m.geom)";
            }

            sql += $"\r\n GROUP BY m.id, year.year";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            List<IncidentYearPair> pairs = new();
            await foreach (var reader in context.EnumerableReaderAsync())
            {
                pairs.Add(new()
                {
                    Year = reader.GetInt(0),
                    TotalCount = reader.GetInt(1)
                });
            }

            return pairs;
        }

        /// <summary>
        ///     Get total report count by id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<List<InquiryYearPair>> GetTotalReportCountByIdAsync(string id)
        {
            var sql = @"
                SELECT  -- ReportCount
                        spi2.year,
                        spi2.count
                FROM    data.statistics_product_inquiries AS spi2
                JOIN    geocoder.neighborhood n ON n.id = spi2.neighborhood_id
                WHERE   n.id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            sql += $"\r\n LIMIT 1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            List<InquiryYearPair> pairs = new();
            await foreach (var reader in context.EnumerableReaderAsync())
            {
                pairs.Add(new()
                {
                    Year = reader.GetInt(0),
                    TotalCount = reader.GetInt(1)
                });
            }

            return pairs;
        }

        /// <summary>
        ///     Get total report count by external id.
        /// </summary>
        /// <param name="id">Neighborhood identifier.</param>
        public async Task<List<InquiryYearPair>> GetTotalReportCountByExternalIdAsync(string id)
        {
            var sql = @"
                SELECT  -- ReportCount
                        spi2.year,
                        spi2.count
                FROM    data.statistics_product_inquiries AS spi2
                JOIN    geocoder.neighborhood n ON n.id = spi2.neighborhood_id
                WHERE   n.external_id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, n.geom)";
            }

            sql += $"\r\n LIMIT 1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            List<InquiryYearPair> pairs = new();
            await foreach (var reader in context.EnumerableReaderAsync())
            {
                pairs.Add(new()
                {
                    Year = reader.GetInt(0),
                    TotalCount = reader.GetInt(1)
                });
            }

            return pairs;
        }

        /// <summary>
        ///     Get total report count by id.
        /// </summary>
        /// <param name="id">Municipality identifier.</param>
        public async Task<IEnumerable<InquiryYearPair>> GetMunicipalityReportCountByIdAsync(string id)
        {
            // FUTURE: Maybe move query to db?
            var sql = @"
				SELECT  -- ReportCount
                        year.year,
                        count(i.id) AS count
                FROM    report.inquiry_sample i
                JOIN    geocoder.address a ON i.address::text = a.id::text
                JOIN    geocoder.building b ON a.building_id::text = b.id::text
                JOIN    geocoder.neighborhood n ON n.id::text = b.neighborhood_id::text
                JOIN    geocoder.district d ON d.id::text = n.district_id::text
                JOIN    geocoder.municipality m ON m.id = d.municipality_id,
                LATERAL CAST(date_part('year'::text, i.create_date)::integer AS integer) year(year)
                WHERE	n.id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, m.geom)";
            }

            sql += $"\r\n GROUP BY m.id, year.year";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            List<InquiryYearPair> pairs = new();
            await foreach (var reader in context.EnumerableReaderAsync())
            {
                pairs.Add(new()
                {
                    Year = reader.GetInt(0),
                    TotalCount = reader.GetInt(1)
                });
            }

            return pairs;
        }

        /// <summary>
        ///     Get total report count by external id.
        /// </summary>
        /// <param name="id">Municipality identifier.</param>
        public async Task<IEnumerable<InquiryYearPair>> GetMunicipalityReportCountByExternalIdAsync(string id)
        {
            // FUTURE: Maybe move query to db?
            var sql = @"
				SELECT  -- ReportCount
                        year.year,
                        count(i.id) AS count
                FROM    report.inquiry_sample i
                JOIN    geocoder.address a ON i.address::text = a.id::text
                JOIN    geocoder.building b ON a.building_id::text = b.id::text
                JOIN    geocoder.neighborhood n ON n.id::text = b.neighborhood_id::text
                JOIN    geocoder.district d ON d.id::text = n.district_id::text
                JOIN    geocoder.municipality m ON m.id = d.municipality_id,
                LATERAL CAST(date_part('year'::text, i.create_date)::integer AS integer) year(year)
                WHERE   n.external_id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, m.geom)";
            }

            sql += $"\r\n GROUP BY m.id, year.year";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            List<InquiryYearPair> pairs = new();
            await foreach (var reader in context.EnumerableReaderAsync())
            {
                pairs.Add(new()
                {
                    Year = reader.GetInt(0),
                    TotalCount = reader.GetInt(1)
                });
            }

            return pairs;
        }
    }
}
#pragma warning disable CA1812 // Internal class is never instantiated
