using FunderMaps.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests
{
    public abstract class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>, IAsyncLifetime
         where TStartup : class
    {
        public CustomWebApplicationFactory() => ClientOptions.HandleCookies = false;

        public virtual Task InitializeAsync() => Task.CompletedTask;

        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(DataStore<>));
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureTestServices);
            builder.ConfigureLogging(options =>
            {
                options.AddFilter(logLevel => logLevel >= LogLevel.Error);
            });
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

        public virtual Task DisposeAsync() => Task.CompletedTask;
    }
}
