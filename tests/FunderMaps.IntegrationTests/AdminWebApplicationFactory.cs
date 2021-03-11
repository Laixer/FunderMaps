using FunderMaps.Core.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests
{
    /// <summary>
    ///     FunderMaps webapplication factory.
    /// </summary>
    public class AdminWebApplicationFactory<TStartup> : FunderMapsWebApplicationFactory<TStartup>
        where TStartup : class
    {
        private class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions
        {
            public User User { get; set; } = new UserFaker().Generate();
            public Organization Organization { get; set; } = new OrganizationFaker().Generate();
            public OrganizationRole OrganizationRole { get; set; } = new Bogus.Faker().PickRandom<OrganizationRole>();
            public IEnumerable<Claim> Claims { get; set; }
        }

        private class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
        {
            public const string AuthenticationScheme = "Test";

            public TestAuthenticationSchemeOptions AuthSchemeOptions { get; set; }

            public TestAuthHandler(
                IOptionsMonitor<AuthenticationSchemeOptions> options,
                ILoggerFactory logger,
                UrlEncoder encoder,
                ISystemClock clock,
                TestAuthenticationSchemeOptions authPrincipal)
                : base(options, logger, encoder, clock)
            {
                AuthSchemeOptions = authPrincipal;
            }

            private static ClaimsPrincipal CreateUserPrincipal(
                User user,
                Organization organization,
                OrganizationRole organizationRole,
                string authenticationType,
                IEnumerable<Claim> additionalClaims = null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(FunderMapsAuthenticationClaimTypes.Tenant, organization.Id.ToString()),
                    new Claim(FunderMapsAuthenticationClaimTypes.TenantRole, organizationRole.ToString()),
                };

                ClaimsIdentity identity = new(claims, authenticationType, ClaimTypes.Name, ClaimTypes.Role);

                if (additionalClaims is not null)
                {
                    foreach (var claim in additionalClaims)
                    {
                        identity.AddClaim(claim);
                    }
                }

                return new(identity);
            }

            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                ClaimsPrincipal principal = CreateUserPrincipal(
                    AuthSchemeOptions.User,
                    AuthSchemeOptions.Organization,
                    AuthSchemeOptions.OrganizationRole,
                    AuthenticationScheme,
                    AuthSchemeOptions.Claims);
                AuthenticationTicket ticket = new(principal, AuthenticationScheme);
                var result = AuthenticateResult.Success(ticket);

                return Task.FromResult(result);
            }
        }

        /// <summary>
        ///     Configure test host services.
        /// </summary>
        protected override void ConfigureTestServices(IServiceCollection services)
        {
            services.AddSingleton<TestAuthenticationSchemeOptions>((sp) =>
            {
                var options = new TestAuthenticationSchemeOptions();
                options.User.Role = ApplicationRole.Administrator;
                return options;
            });
            services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", null);
        }
    }
}
