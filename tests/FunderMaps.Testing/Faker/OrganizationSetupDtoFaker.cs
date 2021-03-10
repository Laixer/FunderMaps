using Bogus;
using FunderMaps.Testing.Extensions;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="OrganizationSetupDto"/>.
    /// </summary>
    public class OrganizationSetupDtoFaker : Faker<OrganizationSetupDto>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationSetupDtoFaker()
        {
            RuleFor(f => f.Email, f => f.Internet.Email());
            RuleFor(f => f.Password, f => f.Random.Password(64));
        }
    }
}
