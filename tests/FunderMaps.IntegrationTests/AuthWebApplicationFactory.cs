using FunderMaps.IntegrationTests.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FunderMaps.IntegrationTests
{
    public abstract class AuthWebApplicationFactory<TStartup> : CustomWebApplicationFactory<TStartup>
        where TStartup : class
    {
        public AuthWebApplicationFactory<TStartup> ConfigureAuthentication(Action<TestAuthenticationSchemeOptions> initializer = null)
        {
            var authPrincipal = Services.GetService<TestAuthenticationSchemeOptions>();
            initializer?.Invoke(authPrincipal);

            return this;
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
            services.AddSingleton<TestAuthenticationSchemeOptions>();

            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
        }
    }
}
