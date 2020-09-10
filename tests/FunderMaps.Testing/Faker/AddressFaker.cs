using Bogus;
using FunderMaps.Core.Entities;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="Address"/>.
    /// </summary>
    public class AddressFaker : Faker<Address>
    {
        public AddressFaker()
        {
            RuleFor(f => f.Id, f => $"gfm-{f.Random.Hash(32)}");
            RuleFor(f => f.BuildingNumber, f => $"{f.Address.BuildingNumber()} {f.Address.SecondaryAddress()}"); // FUTURE SecondaryAddress for some
            RuleFor(f => f.PostalCode, f => f.Address.ZipCode());
            RuleFor(f => f.Street, f => f.Address.StreetName());
            RuleFor(f => f.IsActive, f => f.Random.Bool(0.9f));
            RuleFor(f => f.ExternalId, f => $"NL.IMBAG.NUMMERAANDUIDING.{f.Random.ReplaceNumbers("################")}");
            RuleFor(f => f.ExternalSource, f => "nl_bag");
        }
    }
}
