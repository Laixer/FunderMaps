using FunderMaps.IntegrationTests.Authentication;
using FunderMaps.Testing.Repositories;
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

        public AuthWebApplicationFactory<TStartup> WithAuthenticationStores()
        {
            var authPrincipal = Services.GetService<TestAuthenticationSchemeOptions>();

            WithDataStoreItem(new UserRecord { User = authPrincipal.User });
            WithDataStoreItem(authPrincipal.Organization);
            WithDataStoreItem(new OrganizationUserRecord
            {
                UserId = authPrincipal.User.Id,
                OrganizationId = authPrincipal.Organization.Id,
                OrganizationRole = authPrincipal.OrganizationRole,
            });

            return this;
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureTestServices(services);

            services.AddSingleton<TestAuthenticationSchemeOptions>();

            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
        }
    }
}
