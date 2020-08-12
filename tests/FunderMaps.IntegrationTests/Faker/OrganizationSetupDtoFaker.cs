using Bogus;
using FunderMaps.IntegrationTests.Extensions;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.IntegrationTests.Faker
{
    public class OrganizationSetupDtoFaker : Faker<OrganizationSetupDto>
    {
        public OrganizationSetupDtoFaker()
        {
            RuleFor(f => f.Email, f => f.Internet.Email());
            RuleFor(f => f.Password, f => f.Random.Password(64));
        }
    }
}
