using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

[assembly: WebJobsStartup(typeof(FunderMaps.Tdm.Startup))]

namespace FunderMaps.Tdm
{
    /// <summary>
    /// Configure webjob dependency injection.
    /// </summary>
    public class Startup : IWebJobsStartup
    {
        /// <summary>
        /// Configure services.
        /// </summary>
        /// <param name="builder"><see cref="IWebJobsBuilder"/>.</param>
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddLocalConfig();
            builder.AddTdmSync();

            builder.Services.TryAddSingleton<StorageAccountProvider>();
            builder.Services.TryAddSingleton<FunctionSharedState>();
        }
    }
}
