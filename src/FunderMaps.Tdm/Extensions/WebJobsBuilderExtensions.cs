using FunderMaps.Tdm;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;

namespace Microsoft.Extensions.Hosting;
{
    /// <summary>
    /// Web jobs builder extensions.
    /// </summary>
    internal static class WebJobsBuilderExtensions
    {
        /// <summary>
        /// Add TdmSync service to webjob builder
        /// </summary>
        /// <param name="builder"><see cref="IWebJobsBuilder"/>.</param>
        /// <returns><see cref="IWebJobsBuilder"/>.</returns>
        public static IWebJobsBuilder AddTdmSync(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddOptions<TdmServiceOptions>()
                .Configure<IConfiguration>((options, config) =>
                {
                    options.Config = new TdmClient.TdmConfig();
                    config.Bind("NvmOAuth", options.Config);
                });
            builder.Services.TryAddSingleton<TdmService>();

            return builder;
        }

        /// <summary>
        /// Add local configuration to webjob builder.
        /// </summary>
        /// <param name="builder"><see cref="IWebJobsBuilder"/>.</param>
        /// <returns><see cref="IWebJobsBuilder"/>.</returns>
        public static IWebJobsBuilder AddLocalConfig(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var serviceProvider = builder.Services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            // Replace configuration service with a chained configuration.
            builder.Services.Replace(ServiceDescriptor.Singleton<IConfiguration>((_)
                => new ConfigurationBuilder()
                    .AddConfiguration(configuration)
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("local.settings.json", optional: true)
                    .Build()));

            return builder;
        }
    }
}
