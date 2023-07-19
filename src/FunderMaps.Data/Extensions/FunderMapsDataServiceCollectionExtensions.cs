using FunderMaps.Core.Components;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Providers;
using FunderMaps.Data.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Provides extension methods for services from this assembly.
/// </summary>
public static class FunderMapsDataServiceCollectionExtensions
{
    /// <summary>
    ///     Add repository with application context injection to container.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This service factory create the repository service and initializes the context.
    ///     </para>
    ///     <para>
    ///         Repositories that obey the <see cref="DbServiceBase"/> contract can request the application context
    ///         from inheritance. The application context is per scope so repositories need to be scoped as well.
    ///     </para>
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    private static IServiceCollection AddContextRepository<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : DbServiceBase, TService, new()
        => services.AddScoped<TService, TImplementation>(serviceProvider =>
        {
            TImplementation repository = new();
            DbServiceBase injectorBase = repository as DbServiceBase;

            injectorBase.AppContext = serviceProvider.GetRequiredService<FunderMaps.Core.AppContext>();
            injectorBase.Cache = serviceProvider.GetRequiredService<IMemoryCache>();
            injectorBase.DbContextFactory = ActivatorUtilities.CreateInstance<DbContextFactory>(serviceProvider);

            return repository;
        });

    /// <summary>
    ///     Adds the data services to the container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>An instance of <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddFunderMapsDataServices(this IServiceCollection services)
    {
        // The data layer depends upon the memory cache service to provide the ability to cache
        // objects to memory. The memory cache may have already been registered with the container
        // by some other package, however we cannot expect this to be the case.
        services.AddMemoryCache();

        services.AddSingleton<DbProvider, NpgsqlDbProvider>();

        // Register context repositories with the DI container.
        // NOTE: Keep the order in which they are directory listed
        services.AddContextRepository<IAddressRepository, AddressRepository>();
        services.AddContextRepository<IAnalysisRepository, AnalysisRepository>();
        services.AddContextRepository<IBuildingRepository, BuildingRepository>();
        services.AddContextRepository<IBundleRepository, BundleRepository>();
        services.AddContextRepository<IContractorRepository, ContractorRepository>();
        services.AddContextRepository<IIncidentRepository, IncidentRepository>();
        services.AddContextRepository<IInquiryRepository, InquiryRepository>();
        services.AddContextRepository<IInquirySampleRepository, InquirySampleRepository>();
        services.AddContextRepository<IKeystoreRepository, KeystoreRepository>();
        services.AddContextRepository<IMapsetRepository, MapsetRepository>();
        services.AddContextRepository<IOrganizationRepository, OrganizationRepository>();
        services.AddContextRepository<IOrganizationUserRepository, OrganizationUserRepository>();
        services.AddContextRepository<IRecoveryRepository, RecoveryRepository>();
        services.AddContextRepository<IRecoverySampleRepository, RecoverySampleRepository>();
        services.AddContextRepository<IStatisticsRepository, StatisticsRepository>();
        services.AddContextRepository<ITestRepository, TestRepository>();
        services.AddContextRepository<ITelemetryRepository, TelemetryRepository>();
        services.AddContextRepository<IUserRepository, UserRepository>();

        return services;
    }
}
