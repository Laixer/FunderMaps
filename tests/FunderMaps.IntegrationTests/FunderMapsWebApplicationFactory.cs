using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests
{
    /// <summary>
    ///     FunderMaps webapplication factory.
    /// </summary>
    public class FunderMapsWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>, IAsyncLifetime
         where TStartup : class
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public FunderMapsWebApplicationFactory() => ClientOptions.HandleCookies = false;

        /// <summary>
        ///     Called immediately after the class has been created, before it is used.
        /// </summary>
        public virtual Task InitializeAsync() => Task.CompletedTask;

        /// <summary>
        ///     Configure test host services.
        /// </summary>
        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
        }

        /// <summary>
        ///     Configure the application host.
        /// </summary>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureTestServices);
            builder.ConfigureLogging(options =>
            {
                options.AddFilter(logLevel => logLevel >= LogLevel.Error);
            });
        }

        /// <summary>
        ///     Called when an object is no longer needed.
        /// </summary>
        public virtual Task DisposeAsync() => Task.CompletedTask;
    }
}
