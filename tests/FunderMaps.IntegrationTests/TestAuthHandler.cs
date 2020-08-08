using FunderMaps.Core.Entities;
using FunderMaps.IntegrationTests.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthPrincipal AuthPrincipal { get; set; }

        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            TestAuthPrincipal authPrincipal)
            : base(options, logger, encoder, clock)
        {
            AuthPrincipal = authPrincipal;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var user = AuthPrincipal.User;
            var identity = new ClaimsIdentity("Test", ClaimTypes.Name, ClaimTypes.Role);
            if (user.Id != Guid.Empty)
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            }

            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
