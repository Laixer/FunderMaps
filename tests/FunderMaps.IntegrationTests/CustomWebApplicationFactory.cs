using FunderMaps.Testing;
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

        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(DataStore<>));
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
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
