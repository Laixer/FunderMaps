using Bogus;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="AnalysisProduct"/>.
    /// </summary>
    public class StatisticsProductFaker : Faker<StatisticsProduct>
    {
        // FUTURE Check subclass product initialization --> slow?
        public StatisticsProductFaker()
        {
            RuleFor(f => f.ConstructionYearDistribution, f => CreateConstructionYearDistribution());
            RuleFor(f => f.DataCollectedPercentage, f => f.Random.Double(0, 100));
            RuleFor(f => f.FoundationRiskDistribution, f => CreateFoundationRiskDistribution());
            RuleFor(f => f.FoundationTypeDistribution, f => CreateFoundationTypeDistribution(new List<FoundationType> { f.PickRandom<FoundationType>() })); // FUTURE List always length 1.
            RuleFor(f => f.TotalBuildingRestoredCount, f => f.Random.UInt(0, 100));
            RuleFor(f => f.TotalIncidentCount, f => f.Random.UInt(0, 100));
            RuleFor(f => f.TotalReportCount, f => f.Random.UInt(0, 100));
        }

        private static ConstructionYearDistribution CreateConstructionYearDistribution()
        {
            var decades = new List<ConstructionYearPair>();

            var random = new Random();
            for (int i = 0; i < random.Next(1, 10); i++)
            {
                decades.Add(new ConstructionYearPair
                {
                    Decade = Years.FromDecade(10 * random.Next(90, 210)),
                    TotalCount = (uint)random.Next(1, 100)
                });
            }

            return new ConstructionYearDistribution
            {
                Decades = decades
            };
        }

        private static FoundationRiskDistribution CreateFoundationRiskDistribution()
        {
            var random = new Random();
            var result = new FoundationRiskDistribution();

            result.PercentageA = random.NextDouble() * 100;
            result.PercentageB = random.NextDouble() * (100 - result.PercentageA);
            result.PercentageC = random.NextDouble() * (100 - (result.PercentageA + result.PercentageB));
            result.PercentageD = random.NextDouble() * (100 - (result.PercentageA + result.PercentageB + result.PercentageC));
            result.PercentageE = 100 - (result.PercentageA + result.PercentageB + result.PercentageC + result.PercentageE);

            return result;
        }

        private static FoundationTypeDistribution CreateFoundationTypeDistribution(List<FoundationType> foundationTypes)
        {
            var pairs = new List<FoundationTypePair>();
            var random = new Random();

            for (int i = 0; i < foundationTypes.Count; i++)
            {
                pairs.Add(new FoundationTypePair
                {
                    FoundationType = foundationTypes[i],
                    Percentage = (uint)random.Next(1, 100)
                });
            }

            return new FoundationTypeDistribution
            {
                FoundationTypes = pairs
            };
        }
    }
}
