using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Extensions;

namespace FunderMaps.Data.Repositories;

internal sealed class StatisticsRepository : DbServiceBase, IStatisticsRepository
{
    public async Task<FoundationTypeDistribution> GetFoundationTypeDistributionByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- FoundationTypeDistribution
                    spft.foundation_type,
                    round(spft.percentage::numeric, 2)
            FROM    data.statistics_product_foundation_type AS spft
            WHERE   spft.neighborhood_id = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        List<FoundationTypePair> pairs = [];
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

    public async Task<ConstructionYearDistribution> GetConstructionYearDistributionByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- ConstructionYearDistribution
                    spcy.year_from,
                    spcy.count
            FROM    data.statistics_product_construction_years AS spcy
            WHERE   spcy.neighborhood_id  = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        List<ConstructionYearPair> pairs = [];
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

    public async Task<decimal> GetDataCollectedPercentageByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- DataCollected
                    round(spdc.percentage::numeric, 2)
            FROM    data.statistics_product_data_collected AS spdc
            WHERE   spdc.neighborhood_id = @id
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        return await context.ScalarAsync<decimal>(resultGuard: false);
    }

    public async Task<FoundationRiskDistribution> GetFoundationRiskDistributionByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- FoundationRiskDistribution
                    spfr.foundation_risk,
                    round(spfr.percentage::numeric, 2)
            FROM    data.statistics_product_foundation_risk AS spfr
            WHERE   spfr.neighborhood_id  = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        var map = new Dictionary<FoundationRisk, decimal>()
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

    public async Task<long> GetTotalBuildingRestoredCountByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- BuildingRestoredCount
                    spbr.count
            FROM    data.statistics_product_buildings_restored AS spbr
            WHERE   spbr.neighborhood_id = @id
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        return await context.ScalarAsync<long>(resultGuard: false);
    }

    public async Task<IEnumerable<IncidentYearPair>> GetTotalIncidentCountByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- IncidentCount
                    spi.year,
                    spi.count
            FROM    data.statistics_product_incidents AS spi
            WHERE   spi.neighborhood_id = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        List<IncidentYearPair> pairs = [];
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

    public async Task<IEnumerable<IncidentYearPair>> GetMunicipalityIncidentCountByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- IncidentCount
                    spim.year,
                    spim.count
            FROM    data.statistics_product_incident_municipality spim
            WHERE   spim.municipality_id = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        List<IncidentYearPair> pairs = [];
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

    public async Task<List<InquiryYearPair>> GetTotalReportCountByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- ReportCount
                    spi.year,
                    spi.count
            FROM    data.statistics_product_inquiries AS spi
            WHERE   spi.neighborhood_id = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        List<InquiryYearPair> pairs = [];
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

    public async Task<IEnumerable<InquiryYearPair>> GetMunicipalityReportCountByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- ReportCount
                    spim.year,
                    spim.count
            FROM    data.statistics_product_inquiry_municipality spim
            WHERE   spim.municipality_id = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        List<InquiryYearPair> pairs = [];
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
