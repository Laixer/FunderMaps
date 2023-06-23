using FunderMaps.Core.Components;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Providers;
using FunderMaps.Data.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Provides extension methods for services from this assembly.
/// </summary>
public static class FunderMapsDataServiceCollectionExtensions
{
    /// <summary>
    ///     Configuration.
    /// </summary>
    public static IConfiguration Configuration { get; set; }

    /// <summary>
    ///     Host environment.
    /// </summary>
    public static IHostEnvironment HostEnvironment { get; set; }

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
            injectorBase.Cache = serviceProvider.GetService<IMemoryCache>();
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
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        // The startup essential properties can be used to setup components.
        (Configuration, HostEnvironment) = services.BuildStartupProperties();

        // Register context repositories with the DI container.
        // NOTE: Keep the order in which they are directory listed
        services.AddContextRepository<IAddressRepository, AddressRepository>();
        services.AddContextRepository<IAnalysisRepository, AnalysisRepository>();
        services.AddContextRepository<IContactRepository, ContactRepository>();
        services.AddContextRepository<IContractorRepository, ContractorRepository>();
        services.AddContextRepository<IIncidentRepository, IncidentRepository>();
        services.AddContextRepository<IInquiryRepository, InquiryRepository>();
        services.AddContextRepository<IInquirySampleRepository, InquirySampleRepository>();
        services.AddContextRepository<IMapsetRepository, MapsetRepository>();
        services.AddContextRepository<IOrganizationProposalRepository, OrganizationProposalRepository>();
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
        // services.AddTransient<Npgsql.Logging.NpgsqlLogger, DataNpgsqlLogger>();
        services.Configure<DbProviderOptions>(options =>
        {
            options.ConnectionStringName = dbConfigName;
        });

        return services;
    }
}
