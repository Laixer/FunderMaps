using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Services;
using FunderMaps.IntegrationTests.Repositories;
using FunderMaps.IntegrationTests.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FunderMaps.IntegrationTests
{
    public abstract class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
         where TStartup : class
    {
        public CustomWebApplicationFactory()
        {
            ClientOptions.HandleCookies = false;
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            //services.Remove(services.SingleOrDefault(d => d.ServiceType.Name == "DbProvider"));
        }

        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(EntityDataStore<>));
            services.AddSingleton(typeof(ObjectDataStore));

            // Repositories
            services.AddScoped<IAddressRepository, TestAddressRepository>();
            services.AddScoped<IAnalysisRepository, TestAnalysisRepository>();
            services.AddScoped<IBuildingRepository, TestBuildingRepository>();
            services.AddScoped<IContactRepository, TestContactRepository>();
            services.AddScoped<IIncidentRepository, TestIncidentRepository>();
            services.AddScoped<IInquiryRepository, TestInquiryRepository>();
            services.AddScoped<IInquirySampleRepository, TestInquirySampleRepository>();
            services.AddScoped<IRecoveryRepository, TestRecoveryRepository>();
            services.AddScoped<IRecoverySampleRepository, TestRecoverySampleRepository>();
            services.AddScoped<IStatisticsRepository, TestStatisticsRepository>();
            services.AddScoped<IUserRepository, TestUserRepository>();
            services.AddScoped<IOrganizationProposalRepository, TestOrganizationProposalRepository>();
            services.AddScoped<IOrganizationRepository, TestOrganizationRepository>();
            services.AddScoped<IOrganizationUserRepository, TestOrganizationUserRepository>();

            // Services
            services.AddScoped<IUserTrackingService, TestUserTrackingService>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(ConfigureServices);
            builder.ConfigureTestServices(ConfigureTestServices);
        }

        // FUTURE Helper, move ?
        public virtual CustomWebApplicationFactory<TStartup> WithDataStoreList<TEntity>(TEntity entity)
            where TEntity : BaseEntity<TEntity>
        {
            var dataStore = Services.GetService<EntityDataStore<TEntity>>();
            dataStore.Reset(entity);

            return this;
        }

        // FUTURE Helper, move ?
        public virtual CustomWebApplicationFactory<TStartup> WithDataStoreList<TEntity>(IEnumerable<TEntity> list)
            where TEntity : BaseEntity<TEntity>
        {
            var dataStore = Services.GetService<EntityDataStore<TEntity>>();
            dataStore.Reset(list);

            return this;
        }

        // FUTURE Helper, move ?
        public virtual CustomWebApplicationFactory<TStartup> WithObjectStoreItem<TObject>(TObject obj)
            where TObject : class
        {
            // TODO Implement better
            var dataStore = Services.GetService<ObjectDataStore>();
            dataStore.ClearByTypeAndAddSingle<TObject>(obj);

            return this;
        }

        // FUTURE Helper, move ?
        public virtual CustomWebApplicationFactory<TStartup> WithObjectStoreList<TObject>(IEnumerable<TObject> objList)
            where TObject : class
        {
            // TODO Implement better
            var dataStore = Services.GetService<ObjectDataStore>();
            dataStore.ClearByTypeAndAddList<TObject>(objList);

            return this;
        }
    }
}
