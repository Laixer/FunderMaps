using Bogus;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using System;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="AnalysisProduct"/>.
    /// </summary>
    public class AnalysisProductFaker : Faker<AnalysisProduct>
    {
        // FUTURE: Class is imcomplete.
        // FUTURE Check subclass product initialization --> slow?
        public AnalysisProductFaker()
        {
            RuleFor(f => f.BuildingHeight, f => f.Random.Double(3, 25));
            RuleFor(f => f.ConstructionYear, f => new DateTimeOffset(f.Random.Int(900, 2100), 1, 1, 1, 1, 1, TimeSpan.Zero));
            RuleFor(f => f.DewateringDepth, f => f.Random.Double(-100, 100));
            RuleFor(f => f.Drystand, f => f.Random.Double(-100, 100));
            RuleFor(f => f.ExternalId, f => $"NL.IMBAG.PAND.{f.Random.Hash(16)}");
            RuleFor(f => f.ExternalSource, f => f.PickRandom<ExternalDataSource>());
            RuleFor(f => f.FoundationType, f => f.PickRandom<FoundationType>());
            RuleFor(f => f.GroundWaterLevel, f => f.Random.Double(-100, 0));
            RuleFor(f => f.Id, f => $"gfm-{f.Random.Hash(32)}");
            RuleFor(f => f.NeighborhoodId, f => $"gfm-{f.Random.Hash(32)}");
            RuleFor(f => f.RestorationCosts, f => f.Random.Double(500, 50000));
            RuleFor(f => f.Statistics.FoundationTypeDistribution, f => null);
            RuleFor(f => f.Statistics.FoundationRiskDistribution, f => null);
            RuleFor(f => f.Statistics.ConstructionYearDistribution, f => null);
            RuleFor(f => f.Statistics.DataCollectedPercentage, f => f.Random.Double(0, 100));
            RuleFor(f => f.Statistics.TotalBuildingRestoredCount, f => f.Random.UInt(3, 100));
            RuleFor(f => f.Statistics.TotalIncidentCount, f => f.Random.UInt(3, 100));
            RuleFor(f => f.Statistics.TotalReportCount, f => f.Random.UInt(3, 100));
        }
    }
}
