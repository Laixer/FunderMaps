using Bogus;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;

namespace FunderMaps.IntegrationTests.Faker
{
    /// <summary>
    ///     Faker for <see cref="AnalysisProduct"/>.
    /// </summary>
    public class StatisticsProductFaker : Faker<StatisticsProduct>
    {
        /// FUTURE Check subclass product initialization --> slow?
        public StatisticsProductFaker()
        {
            RuleFor(f => f.ConstructionYearDistribution, f => new ConstructionYearDistribution()); // Empty
            RuleFor(f => f.DataCollectedPercentage, f => f.Random.Double(0, 100));
            RuleFor(f => f.FoundationRiskDistribution, f => new FoundationRiskDistribution()); // Empty
            RuleFor(f => f.FoundationTypeDistribution, f => new FoundationTypeDistribution()); // Empty
            RuleFor(f => f.NeighborhoodCode, f => f.Random.Hash(6));
            // RuleFor(f => f.NeighborhoodId, f => $"gfm-{f.Random.Hash(32)}");
            RuleFor(f => f.TotalBuildingRestored, f => f.Random.UInt(0, 100));
            RuleFor(f => f.TotalIncidents, f => f.Random.UInt(0, 100));
            RuleFor(f => f.TotalReportCount, f => f.Random.UInt(0, 100));
        }
    }
}
