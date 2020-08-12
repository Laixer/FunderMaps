using FunderMaps.IntegrationTests.Authentication;
using FunderMaps.IntegrationTests.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FunderMaps.IntegrationTests
{
    public class AuthWebApplicationFactory<TStartup> : CustomWebApplicationFactory<TStartup>
        where TStartup : class
    {
        public AuthWebApplicationFactory<TStartup> WithAuthentication(Action<TestAuthenticationSchemeOptions> initializer = null)
        {
            var authPrincipal = Services.GetService<TestAuthenticationSchemeOptions>();
            initializer?.Invoke(authPrincipal);

            return this;
        }

        public AuthWebApplicationFactory<TStartup> WithAuthenticationStores()
        {
            var authPrincipal = Services.GetService<TestAuthenticationSchemeOptions>();

            WithDataStoreList(new UserRecord { User = authPrincipal.User });
            WithDataStoreList(authPrincipal.Organization);
            WithDataStoreList(new OrganizationUserRecord
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
