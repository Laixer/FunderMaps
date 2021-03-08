using FunderMaps.IntegrationTests.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FunderMaps.IntegrationTests
{
    /// <summary>
    ///     FunderMaps webapplication factory.
    /// </summary>
    public abstract class AdminWebApplicationFactory<TStartup> : FunderMapsWebApplicationFactory<TStartup>
        where TStartup : class
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AdminWebApplicationFactory<TStartup> ConfigureAuthentication(Action<TestAuthenticationSchemeOptions> initializer = null)
        {
            var authPrincipal = Services.GetService<TestAuthenticationSchemeOptions>();
            initializer?.Invoke(authPrincipal);

            return this;
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
            services.AddSingleton<TestAuthenticationSchemeOptions>();

            services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", null);
        }
    }
}
