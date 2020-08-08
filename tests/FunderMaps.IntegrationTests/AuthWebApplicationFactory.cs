using FunderMaps.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace FunderMaps.IntegrationTests
{
    public class AuthWebApplicationFactory<TStartup> : CustomWebApplicationFactory<TStartup>
        where TStartup : class
    {
        public AuthWebApplicationFactory<TStartup> WithAuthOptions(User user)
        {
            var authPrincipal = Services.GetService<TestAuthPrincipal>();
            authPrincipal.User = user;

            return this;
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureTestServices(services);

            services.AddSingleton<TestAuthPrincipal>();

            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
        }
    }
}
