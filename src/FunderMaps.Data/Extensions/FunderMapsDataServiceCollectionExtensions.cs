using FunderMaps.Core.Interfaces.Repositories;
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
        ///     Adds the data services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFunderMapsDataServices(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Keep the order in which they are directory listed
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAnalysisRepository, AnalysisRepository>();
            services.AddScoped<IBuildingRepository, BuildingRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IIncidentRepository, IncidentRepository>();
            services.AddScoped<IInquiryRepository, InquiryRepository>();
            services.AddScoped<IInquirySampleRepository, InquirySampleRepository>();
            services.AddScoped<IOrganizationProposalRepository, OrganizationProposalRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrganizationUserRepository, OrganizationUserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectSampleRepository, ProjectSampleRepository>();
            services.AddScoped<IRecoveryRepository, RecoveryRepository>();
            services.AddScoped<IRecoverySampleRepository, RecoverySampleRepository>();
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<ITrackingRepository, TrackingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

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
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrEmpty(dbConfigName))
            {
                throw new ArgumentNullException(nameof(dbConfigName));
            }

            services.AddFunderMapsDataServices();
            services.AddSingleton<DbProvider, NpgsqlDbProvider>();
            services.Configure<DbProviderOptions>(options => options.ConnectionStringName = dbConfigName);

            return services;
        }
    }
}
