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
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
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
            //services.AddScoped<IOrganizationRepository, TestUserRepository>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(ConfigureServices);
            builder.ConfigureTestServices(ConfigureTestServices);
        }

        // TODO Helper, move ?
        public virtual CustomWebApplicationFactory<TStartup> WithDataStoreList<TEntity>(TEntity entity)
            where TEntity : BaseEntity
        {
            var dataStore = Services.GetService<EntityDataStore<TEntity>>();
            dataStore.Entities = new List<TEntity> { entity };

            return this;
        }

        // TODO Helper, move ?
        public virtual CustomWebApplicationFactory<TStartup> WithDataStoreList<TEntity>(IList<TEntity> list)
            where TEntity : BaseEntity
        {
            var dataStore = Services.GetService<EntityDataStore<TEntity>>();
            dataStore.Entities = list;

            return this;
        }
    }
}
