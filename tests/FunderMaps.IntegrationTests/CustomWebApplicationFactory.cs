using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureTestServices);
            builder.ConfigureLogging(options =>
            {
                options.AddFilter(logLevel => logLevel >= LogLevel.Error);
            });
        }

        public virtual Task DisposeAsync() => Task.CompletedTask;
    }
}
