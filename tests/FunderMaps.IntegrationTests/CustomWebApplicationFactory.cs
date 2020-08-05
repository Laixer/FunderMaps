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

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //services.Remove(services.SingleOrDefault(d => d.ServiceType.Name == "DbProvider"));
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(typeof(EntityDataStore<>));
                services.AddScoped<IAddressRepository, TestAddressRepository>();
                services.AddScoped<IContactRepository, TestContactRepository>();
                services.AddScoped<IIncidentRepository, TestIncidentRepository>();
                services.AddScoped<IInquiryRepository, TestInquiryRepository>();
                services.AddScoped<IInquirySampleRepository, TestInquirySampleRepository>();
            });
        }

        public virtual CustomWebApplicationFactory<TStartup> WithDataStoreList<TEntity>(TEntity entity)
            where TEntity : BaseEntity
        {
            var dataStore = Services.GetService<EntityDataStore<TEntity>>();
            dataStore.Entities = new List<TEntity> { entity };

            return this;
        }

        public virtual CustomWebApplicationFactory<TStartup> WithDataStoreList<TEntity>(IList<TEntity> list)
            where TEntity : BaseEntity
        {
            var dataStore = Services.GetService<EntityDataStore<TEntity>>();
            dataStore.Entities = list;

            return this;
        }
    }
}
