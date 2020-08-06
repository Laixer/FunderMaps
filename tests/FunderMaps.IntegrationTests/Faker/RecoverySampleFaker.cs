using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using System;

namespace FunderMaps.IntegrationTests.Faker
{
    public class RecoverySampleFaker : Faker<RecoverySample>
    {
        public RecoverySampleFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.Recovery, f => f.UniqueIndex);
            RuleFor(f => f.Note, f => f.Lorem.Text());
            RuleFor(f => f.Address, f => $"gfm-{f.Random.Hash(32)}");
            RuleFor(f => f.Status, f => f.PickRandom<RecoveryStatus>());
            RuleFor(f => f.Type, f => f.PickRandom<RecoveryType>());
            RuleFor(f => f.PileType, f => f.PickRandom<PileType>());
            RuleFor(f => f.Contractor, f => f.Random.Uuid()); // TODO: From org
            RuleFor(f => f.Facade, f => f.Random.ArrayElements((Facade[])Enum.GetValues(typeof(Facade))));
            RuleFor(f => f.Permit, f => f.Commerce.Product());
            RuleFor(f => f.PermitDate, f => f.Date.Past(30));
            RuleFor(f => f.RecoveryDate, f => f.Date.Past(30));
        }
    }
}
