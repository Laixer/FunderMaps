using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Security.Claims;

namespace FunderMaps.IntegrationTests.Authentication
{
    public class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public User User { get; set; } = new UserFaker().Generate();
        public Organization Organization { get; set; } = new OrganizationFaker().Generate();
        public OrganizationRole OrganizationRole { get; set; } = new Bogus.Faker().PickRandom<OrganizationRole>();
        public IEnumerable<Claim> Claims { get; set; }
    }
}
