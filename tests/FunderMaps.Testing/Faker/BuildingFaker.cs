using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using System;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="Building"/>.
    /// </summary>
    public class BuildingFaker : Faker<Building>
    {
        public BuildingFaker()
        {
            RuleFor(f => f.Id, f => $"gfm-{f.Random.Hash(32)}");
            RuleFor(f => f.BuiltYear, f => f.Date.Between(DateTime.Parse("900-01-01"), DateTime.Parse("2100-01-01")));
            RuleFor(f => f.IsActive, f => f.Random.Bool(0.9f));
            // RuleFor(f => f.Address, f => $"gfm-{f.Random.Hash(32)}");
            RuleFor(f => f.ExternalId, f => $"NL.IMBAG.PAND.{f.Random.ReplaceNumbers("################")}");
            RuleFor(f => f.ExternalSource, f => f.PickRandom<ExternalDataSource>());
            RuleFor(f => f.buildingType, f => f.PickRandom<BuildingType>());
            RuleFor(f => f.NeighborhoodId, f => $"gfm-{f.Random.Hash(32)}");
        }
    }
}
