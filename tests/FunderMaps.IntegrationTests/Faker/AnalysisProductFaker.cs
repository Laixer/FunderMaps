using Bogus;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using System;

namespace FunderMaps.IntegrationTests.Faker
{
    /// <summary>
    ///     Faker for <see cref="AnalysisProduct"/>.
    /// </summary>
    public class AnalysisProductFaker : Faker<AnalysisProduct>
    {
        // FUTURE Check subclass product initialization --> slow?
        public AnalysisProductFaker()
        {
            RuleFor(f => f.BuildingHeight, f => f.Random.Double(3, 25));
            RuleFor(f => f.ConstructionYear, f => new DateTimeOffset(f.Random.Int(900, 2100), 1, 1, 1, 1, 1, TimeSpan.Zero));
            RuleFor(f => f.ConstructionYearDistribution, f => null);
            RuleFor(f => f.DataCollectedPercentage, f => f.Random.Double(0, 100));
            RuleFor(f => f.DewateringDepth, f => f.Random.Double(-100, 100));
            RuleFor(f => f.Drystand, f => f.Random.Double(-100, 100));
            RuleFor(f => f.ExternalId, f => $"NL.IMBAG.PAND.{f.Random.Hash(16)}");
            RuleFor(f => f.ExternalSource, f => f.PickRandom<ExternalDataSource>());
            RuleFor(f => f.FoundationRisk, f => f.PickRandom<FoundationRisk>());
            RuleFor(f => f.FoundationRiskDistribution, f => null);
            RuleFor(f => f.FoundationType, f => f.PickRandom<FoundationType>()); ;
            RuleFor(f => f.FoundationTypeDistribution, f => null);
            RuleFor(f => f.FullDescription, f => f.Random.Words(10));
            RuleFor(f => f.GroundLevel, f => f.Random.Double(-300, 300));
            RuleFor(f => f.GroundWaterLevel, f => f.Random.Double(-100, 0));
            RuleFor(f => f.Id, f => $"gfm-{f.Random.Hash(32)}");
            RuleFor(f => f.NeighborhoodId, f => $"gfm-{f.Random.Hash(32)}");
            RuleFor(f => f.Reliability, f => f.PickRandom<Reliability>());
            RuleFor(f => f.RestorationCosts, f => f.Random.Double(500, 50000));
            RuleFor(f => f.TerrainDescription, f => f.Random.Words(10));
            RuleFor(f => f.TotalBuildingRestoredCount, f => f.Random.UInt(3, 100));
            RuleFor(f => f.TotalIncidentCount, f => f.Random.UInt(3, 100));
            RuleFor(f => f.TotalReportCount, f => f.Random.UInt(3, 100));
        }
    }
}
