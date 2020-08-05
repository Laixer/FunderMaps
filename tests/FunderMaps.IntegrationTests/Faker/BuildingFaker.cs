using Bogus;
using FunderMaps.Core.Entities;
using System;

namespace FunderMaps.IntegrationTests.Faker
{
    public class BuildingFaker : Faker<Building>
    {
        public BuildingFaker()
        {
            RuleFor(f => f.Id, f => $"gfm-{new Bogus.Faker().Random.Hash(32)}");
            RuleFor(f => f.BuiltYear, f => f.Date.Between(DateTime.Parse("900-01-01"), DateTime.Parse("2100-01-01")));
            RuleFor(f => f.IsActive, f => f.Random.Bool(0.9f));
            RuleFor(f => f.Address, f => new AddressFaker().Generate().Id);
            RuleFor(f => f.ExternalId, f => $"NL.IMBAG.NUMMERAANDUIDING.{f.Random.ReplaceNumbers("################")}");
            RuleFor(f => f.ExternalSource, f => "bag");
        }
    }
}
