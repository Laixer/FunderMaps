using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data;
using FunderMaps.Data.Providers;
using FunderMaps.Data.Repositories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Provides extension methods for services from this assembly.
    /// </summary>
    public static class FunderMapsDataServiceCollectionExtensions
    {
        /// <summary>
        ///     Add repository with application context injection to container.
        /// </summary>
        /// <remarks>
        ///     This service factory create the repository service and initializes the context.
        ///     <para>
        ///         Repositories that obey the <see cref="DbContextBase"/> contract can request the application context
        ///         from inheritance. The application context is per scope so repositories need to be scoped as well.
        ///     </para>
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        private static IServiceCollection AddContextRepository<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : DbContextBase, TService, new()
            => services.AddScoped<TService, TImplementation>(serviceProvider =>
            {
                TImplementation repository = new();
                DbContextBase injectorBase = repository as DbContextBase;
                injectorBase.AppContext = serviceProvider.GetRequiredService<FunderMaps.Core.AppContext>();
                injectorBase.DbProvider = serviceProvider.GetService<DbProvider>();
                // TODO: 
                // injectorBase.DbContextFactory = serviceProvider.GetRequiredService<DbContextFactory>()
                return repository;
            });

        /// <summary>
        ///     Adds the data services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFunderMapsDataServices(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Register context repositories with the DI container.
            // NOTE: Keep the order in which they are directory listed
            services.AddContextRepository<IAddressRepository, AddressRepository>();
            services.AddContextRepository<IAnalysisRepository, AnalysisRepository>();
            services.AddContextRepository<IBuildingRepository, BuildingRepository>();
            services.AddContextRepository<IBundleRepository, BundleRepository>();
            services.AddContextRepository<IContactRepository, ContactRepository>();
            services.AddContextRepository<IIncidentRepository, IncidentRepository>();
            services.AddContextRepository<IInquiryRepository, InquiryRepository>();
            services.AddContextRepository<IInquirySampleRepository, InquirySampleRepository>();
            services.AddContextRepository<ILayerRepository, LayerRepository>();
            services.AddContextRepository<IOrganizationProposalRepository, OrganizationProposalRepository>();
            services.AddContextRepository<IOrganizationRepository, OrganizationRepository>();
            services.AddContextRepository<IOrganizationUserRepository, OrganizationUserRepository>();
            services.AddContextRepository<IProjectRepository, ProjectRepository>();
            services.AddContextRepository<IProjectSampleRepository, ProjectSampleRepository>();
            services.AddContextRepository<IRecoveryRepository, RecoveryRepository>();
            services.AddContextRepository<IRecoverySampleRepository, RecoverySampleRepository>();
            services.AddContextRepository<IStatisticsRepository, StatisticsRepository>();
            services.AddContextRepository<ITestRepository, TestRepository>();
            services.AddContextRepository<ITrackingRepository, TrackingRepository>();
            services.AddContextRepository<IUserRepository, UserRepository>();

            return services;
        }

        /// <summary>
        ///     Adds the data services and database provider to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="dbConfigName">Database connection string.</param>
        /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFunderMapsDataServices(this IServiceCollection services, string dbConfigName)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrEmpty(dbConfigName))
            {
                throw new ArgumentNullException(nameof(dbConfigName));
            }

            services.AddFunderMapsDataServices();
            services.AddSingleton<DbProvider, NpgsqlDbProvider>();
            services.Configure<DbProviderOptions>(options =>
            {
                options.ConnectionStringName = dbConfigName;
                options.ConnectionTimeout = 5; // in seconds
                options.CommandTimeout = 5; // in seconds
                options.MinPoolSize = 0;
                options.MaxPoolSize = 25;
            });

            return services;
        }
    }
}
