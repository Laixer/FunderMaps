using FunderMaps.Core.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
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

        private static ClaimsPrincipal CreateUserPrincipal(User user,
            Organization organization,
            OrganizationRole organizationRole,
            string authenticationType,
            IEnumerable<Claim> additionalClaims = null)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(FunderMapsAuthenticationClaimTypes.Organization, organization.Id.ToString()),
                new Claim(FunderMapsAuthenticationClaimTypes.OrganizationRole, organizationRole.ToString()),
            };

            var identity = new ClaimsIdentity(claims, authenticationType, ClaimTypes.Name, ClaimTypes.Role);

            if (additionalClaims != null)
            {
                foreach (var claim in additionalClaims)
                {
                    identity.AddClaim(claim);
                }
            }

            return new ClaimsPrincipal(identity);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var principal = CreateUserPrincipal(
                AuthSchemeOptions.User,
                AuthSchemeOptions.Organization,
                AuthSchemeOptions.OrganizationRole,
                "Test",
                AuthSchemeOptions.Claims);
            var ticket = new AuthenticationTicket(principal, "Test");
            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
