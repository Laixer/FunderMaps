using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FunderMaps.IndicentEndpoint.Startup))]

namespace FunderMaps.IndicentEndpoint
{
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Configure function host.
        /// </summary>
        /// <param name="builder">See <see cref="IFunctionsHostBuilder"/>.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // Register services from application modules.
            builder.Services.AddFunderMapsCoreServices();
            builder.Services.AddFunderMapsCloudServices();
            builder.Services.AddFunderMapsDataServices("FunderMapsConnection");
        }
    }
}
