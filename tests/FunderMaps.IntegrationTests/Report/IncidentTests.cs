using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Extensions;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Report
{
    public class IncidentTests : IClassFixture<AuthWebApplicationFactory<Startup>>
    {
        private readonly AuthWebApplicationFactory<Startup> _factory;

        public IncidentTests(AuthWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        internal class FakeIncidentDtoData : EnumerableHelper<IncidentDto>
        {
            protected override IEnumerable<IncidentDto> GetEnumerableEntity()
            {
                return new IncidentDtoFaker().Generate(10, 100);
            }
        }

        internal class FakeIncidentData : EnumerableHelper<Incident>
        {
            protected override IEnumerable<Incident> GetEnumerableEntity()
            {
                return new IncidentFaker().Generate(10, 100);
            }
        }

        [Theory]
        [ClassData(typeof(FakeIncidentDtoData))]
        public async Task CreateIncidentReturnIncident(IncidentDto incident)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var incidentDataStore = _factory.Services.GetService<EntityDataStore<Incident>>();
            var contactDataStore = _factory.Services.GetService<EntityDataStore<Contact>>();

            // Act
            var response = await client.PostAsJsonAsync("api/incident", incident);
            var returnObject = await response.Content.ReadFromJsonAsync<IncidentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(incidentDataStore.IsSet);
            Assert.True(contactDataStore.IsSet);
            Assert.StartsWith("FIR", returnObject.Id, StringComparison.InvariantCulture);
            Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
            Assert.Equal(incident.Name, returnObject.Name);
            Assert.Equal(incident.Email, returnObject.Email);
            Assert.Equal(incident.PhoneNumber, returnObject.PhoneNumber);
            Assert.Equal(incident.FoundationType, returnObject.FoundationType);
            Assert.Equal(incident.Address, returnObject.Address);
            Assert.Equal(incident.FoundationDamageCharacteristics, returnObject.FoundationDamageCharacteristics);
            Assert.Equal(incident.EnvironmentDamageCharacteristics, returnObject.EnvironmentDamageCharacteristics);
            Assert.Equal(incident.Owner, returnObject.Owner);
            Assert.Equal(incident.FoundationRecovery, returnObject.FoundationRecovery);
            Assert.Equal(incident.NeightborRecovery, returnObject.NeightborRecovery);
            Assert.Equal(incident.ChainedBuilding, returnObject.ChainedBuilding);
            Assert.Equal(incident.FoundationDamageCause, returnObject.FoundationDamageCause);
            Assert.Equal(incident.DocumentFile, returnObject.DocumentFile);
            Assert.Equal(incident.Note, returnObject.Note);
            Assert.Equal(incident.InternalNote, returnObject.InternalNote);
        }

        [Theory]
        [ClassData(typeof(FakeIncidentData))]
        public async Task GetIncidentByIdReturnSingleIncident(Incident incident)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(incident)
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/incident/{incident.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<IncidentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.StartsWith("FIR", returnObject.Id, StringComparison.InvariantCulture);
            Assert.Equal(incident.Id, returnObject.Id);
            Assert.Equal(incident.AuditStatus, returnObject.AuditStatus);
            Assert.Equal(incident.FoundationType, returnObject.FoundationType);
            Assert.Equal(incident.Address, returnObject.Address);
            Assert.Equal(incident.FoundationDamageCharacteristics, returnObject.FoundationDamageCharacteristics);
            Assert.Equal(incident.EnvironmentDamageCharacteristics, returnObject.EnvironmentDamageCharacteristics);
            Assert.Equal(incident.Owner, returnObject.Owner);
            Assert.Equal(incident.FoundationRecovery, returnObject.FoundationRecovery);
            Assert.Equal(incident.NeightborRecovery, returnObject.NeightborRecovery);
            Assert.Equal(incident.ChainedBuilding, returnObject.ChainedBuilding);
            Assert.Equal(incident.FoundationDamageCause, returnObject.FoundationDamageCause);
            Assert.Equal(incident.DocumentFile, returnObject.DocumentFile);
            Assert.Equal(incident.Note, returnObject.Note);
            Assert.Equal(incident.InternalNote, returnObject.InternalNote);
        }

        [Fact]
        public async Task GetAllIncidentReturnPageIncident()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(new IncidentFaker().Generate(10, 100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/incident");
            var returnList = await response.Content.ReadFromJsonAsync<List<IncidentDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count > 0);
        }

        [Fact]
        public async Task GetAllIncidentReturnAllIncident()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(new IncidentFaker().Generate(100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/incident?limit=100");
            var returnList = await response.Content.ReadFromJsonAsync<List<IncidentDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(100, returnList.Count);
        }

        [Theory]
        [ClassData(typeof(FakeIncidentData))]
        public async Task UpdateIncidentReturnNoContent(Incident incident)
        {
            // Arrange
            var newIncident = new IncidentDtoFaker().Generate(); // TODO Move outside of test
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(incident)
                .CreateClient();
            var incidentDataStore = _factory.Services.GetService<EntityDataStore<Incident>>();

            // Act
            var response = await client.PutAsJsonAsync($"api/incident/{incident.Id}", newIncident);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(1, incidentDataStore.Entities.Count);
            var actualIncident = incidentDataStore.Entities[0];
            Assert.Equal(incident.Id, actualIncident.Id);
            Assert.Equal(incident.AuditStatus, actualIncident.AuditStatus);
            Assert.Equal(newIncident.FoundationType, actualIncident.FoundationType);
            Assert.Equal(newIncident.FoundationDamageCharacteristics, actualIncident.FoundationDamageCharacteristics);
            Assert.Equal(newIncident.EnvironmentDamageCharacteristics, actualIncident.EnvironmentDamageCharacteristics);
            Assert.Equal(newIncident.Owner, actualIncident.Owner);
            Assert.Equal(newIncident.FoundationRecovery, actualIncident.FoundationRecovery);
            Assert.Equal(newIncident.NeightborRecovery, actualIncident.NeightborRecovery);
            Assert.Equal(newIncident.ChainedBuilding, actualIncident.ChainedBuilding);
            Assert.Equal(newIncident.FoundationDamageCause, actualIncident.FoundationDamageCause);
            Assert.Equal(newIncident.DocumentFile, actualIncident.DocumentFile);
            Assert.Equal(newIncident.Note, actualIncident.Note);
            Assert.Equal(newIncident.InternalNote, actualIncident.InternalNote);
        }

        [Theory]
        [ClassData(typeof(FakeIncidentData))]
        public async Task DeleteIncidentReturnNoContent(Incident incident)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(incident)
                .CreateClient();
            var incidentDataStore = _factory.Services.GetService<EntityDataStore<Incident>>();

            // Act
            var response = await client.DeleteAsync($"api/incident/{incident.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.False(incidentDataStore.IsSet);
        }
    }
}
