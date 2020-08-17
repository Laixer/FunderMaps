using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.IntegrationTests.Repositories;
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
            services.AddScoped<IAddressRepository, TestAddressRepository>();
            services.AddScoped<IContactRepository, TestContactRepository>();
            services.AddScoped<IIncidentRepository, TestIncidentRepository>();
            services.AddScoped<IInquiryRepository, TestInquiryRepository>();
            services.AddScoped<IInquirySampleRepository, TestInquirySampleRepository>();
            services.AddScoped<IRecoveryRepository, TestRecoveryRepository>();
            services.AddScoped<IRecoverySampleRepository, TestRecoverySampleRepository>();
            services.AddScoped<IUserRepository, TestUserRepository>();
            services.AddScoped<IOrganizationProposalRepository, TestOrganizationProposalRepository>();
            services.AddScoped<IOrganizationRepository, TestOrganizationRepository>();
            services.AddScoped<IOrganizationUserRepository, TestOrganizationUserRepository>();
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
    }
}
