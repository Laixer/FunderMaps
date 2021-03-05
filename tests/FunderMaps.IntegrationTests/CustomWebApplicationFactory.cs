using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Testing;
using FunderMaps.Testing.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FunderMaps.IntegrationTests
{
    public abstract class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
         where TStartup : class
    {
        public CustomWebApplicationFactory() => ClientOptions.HandleCookies = false;

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            //services.Remove(services.SingleOrDefault(d => d.ServiceType.Name == "DbProvider"));
        }

        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(DataStore<>));

            // Repositories
            services.AddScoped<IUserRepository, TestUserRepository>();
            services.AddScoped<IOrganizationProposalRepository, TestOrganizationProposalRepository>();
            services.AddScoped<IOrganizationRepository, TestOrganizationRepository>();
            services.AddScoped<IOrganizationUserRepository, TestOrganizationUserRepository>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(ConfigureServices);
            builder.ConfigureTestServices(ConfigureTestServices);
        }

        public virtual CustomWebApplicationFactory<TStartup> WithDataStoreItem<TItem>(TItem item)
            where TItem : class
        {
            var dataStore = Services.GetService<DataStore<TItem>>();
            dataStore.Reset(item);

            return this;
        }

        public virtual CustomWebApplicationFactory<TStartup> WithDataStoreList<TItem>(IEnumerable<TItem> list)
            where TItem : class
        {
            var dataStore = Services.GetService<DataStore<TItem>>();
            dataStore.Reset(list);

            return this;
        }
    }
}
