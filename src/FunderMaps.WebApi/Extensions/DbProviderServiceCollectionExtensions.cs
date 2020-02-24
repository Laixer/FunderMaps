using FunderMaps.Providers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Add database provider to service collection.
    /// </summary>
    public static class DbProviderServiceCollectionExtensions
    {
        /// <summary>
        /// Add a DbProvider to the service container.
        /// </summary>
        /// <param name="dbConfigName">Database connection string.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddDbProvider(this IServiceCollection services, string dbConfigName)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrEmpty(dbConfigName))
            {
                throw new ArgumentNullException(nameof(dbConfigName));
            }

            services.AddSingleton<DbProvider>();
            services.Configure<DbProviderOptions>(options => options.ConnectionStringName = dbConfigName);

            return services;
        }
    }
}
