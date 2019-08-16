using FunderMaps.Providers;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Add database provider to service collection.
    /// </summary>
    public static class ServiceCollectionDatabaseServiceExtensions
    {
        /// <summary>
        /// Add a DbProvider to the service container.
        /// </summary>
        /// <param name="dbConfigName">Database connection string.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddDbProvider(this IServiceCollection services, string dbConfigName)
        {
            services.AddSingleton<DbProvider>();
            services.Configure<DbProviderOptions>(options => options.ConnectionStringName = dbConfigName);

            return services;
        }
    }
}
