using Bogus;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;
using System;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="RecoverySampleDto"/>.
    /// </summary>
    public class RecoverySampleDtoFaker : Faker<RecoverySampleDto>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RecoverySampleDtoFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.Recovery, f => f.UniqueIndex);
            RuleFor(f => f.Note, f => f.Lorem.Text());
            RuleFor(f => f.Address, f => $"gfm-{f.Random.Hash(32)}");
            RuleFor(f => f.Status, f => f.PickRandom<RecoveryStatus>());
            RuleFor(f => f.Type, f => f.PickRandom<RecoveryType>());
            RuleFor(f => f.PileType, f => f.PickRandom<PileType>());
            RuleFor(f => f.Contractor, f => f.Random.Uuid());
            RuleFor(f => f.Facade, f => f.Random.ArrayElements((Facade[])Enum.GetValues(typeof(Facade))));
            RuleFor(f => f.Permit, f => f.Commerce.Product());
            RuleFor(f => f.PermitDate, f => f.Date.Past(30));
            RuleFor(f => f.RecoveryDate, f => f.Date.Past(30));
        }
    }
}
