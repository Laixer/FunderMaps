using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Extensions;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Report
{
    public class IncidentTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public IncidentTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        internal class FakeIncidentDtoData : EnumerableHelper<IncidentDto>
        {
            protected override IEnumerable<IncidentDto> GetEnumerableEntity()
            {
                return new IncidentDtoFaker().Generate(10, 1000);
            }
        }

        internal class FakeIncidentData : EnumerableHelper<Incident>
        {
            protected override IEnumerable<Incident> GetEnumerableEntity()
            {
                return new IncidentFaker().Generate(10, 1000);
            }
        }

        [Theory]
        [ClassData(typeof(FakeIncidentDtoData))]
        public async Task CreateIncidentReturnIncident(IncidentDto incident)
        {
            // Arrange
            var client = _factory.CreateClient();
            var incidentDataStore = _factory.Services.GetService<EntityDataStore<Incident>>();
            var contactDataStore = _factory.Services.GetService<EntityDataStore<Contact>>();

            // Act
            var response = await client.PostAsJsonAsync("api/incident", incident).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(incidentDataStore.IsSet);
            Assert.True(contactDataStore.IsSet);
            var actualIncident = await response.Content.ReadFromJsonAsync<IncidentDto>().ConfigureAwait(false);

            // Assert
            Assert.StartsWith("FIR", actualIncident.Id, System.StringComparison.InvariantCulture);
            Assert.Equal(AuditStatus.Todo, actualIncident.AuditStatus);
            Assert.Equal(incident.Name, actualIncident.Name);
            Assert.Equal(incident.Email, actualIncident.Email);
            Assert.Equal(incident.PhoneNumber, actualIncident.PhoneNumber);
            Assert.Equal(incident.FoundationType, actualIncident.FoundationType);
            Assert.Equal(incident.Address, actualIncident.Address);
            Assert.Equal(incident.FoundationDamageCharacteristics, actualIncident.FoundationDamageCharacteristics);
            Assert.Equal(incident.EnvironmentDamageCharacteristics, actualIncident.EnvironmentDamageCharacteristics);
            Assert.Equal(incident.Owner, actualIncident.Owner);
            Assert.Equal(incident.FoundationRecovery, actualIncident.FoundationRecovery);
            Assert.Equal(incident.NeightborRecovery, actualIncident.NeightborRecovery);
            Assert.Equal(incident.ChainedBuilding, actualIncident.ChainedBuilding);
            Assert.Equal(incident.FoundationDamageCause, actualIncident.FoundationDamageCause);
            Assert.Equal(incident.DocumentFile, actualIncident.DocumentFile);
            Assert.Equal(incident.Note, actualIncident.Note);
            Assert.Equal(incident.InternalNote, actualIncident.InternalNote);
        }

        [Theory]
        [ClassData(typeof(FakeIncidentData))]
        public async Task GetIncidentByIdReturnSingleIncident(Incident incident)
        {
            // Arrange
            var client = _factory
                .WithDataStoreList(incident)
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/incident/{incident.Id}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualIncident = await response.Content.ReadFromJsonAsync<IncidentDto>().ConfigureAwait(false);

            // Assert
            Assert.StartsWith("FIR", actualIncident.Id, System.StringComparison.InvariantCulture);
            Assert.Equal(incident.Id, actualIncident.Id);
            Assert.Equal(incident.AuditStatus, actualIncident.AuditStatus);
            Assert.Equal(incident.FoundationType, actualIncident.FoundationType);
            Assert.Equal(incident.Address, actualIncident.Address);
            Assert.Equal(incident.FoundationDamageCharacteristics, actualIncident.FoundationDamageCharacteristics);
            Assert.Equal(incident.EnvironmentDamageCharacteristics, actualIncident.EnvironmentDamageCharacteristics);
            Assert.Equal(incident.Owner, actualIncident.Owner);
            Assert.Equal(incident.FoundationRecovery, actualIncident.FoundationRecovery);
            Assert.Equal(incident.NeightborRecovery, actualIncident.NeightborRecovery);
            Assert.Equal(incident.ChainedBuilding, actualIncident.ChainedBuilding);
            Assert.Equal(incident.FoundationDamageCause, actualIncident.FoundationDamageCause);
            Assert.Equal(incident.DocumentFile, actualIncident.DocumentFile);
            Assert.Equal(incident.Note, actualIncident.Note);
            Assert.Equal(incident.InternalNote, actualIncident.InternalNote);
        }

        [Fact]
        public async Task GetAllIncidentReturnPageIncident()
        {
            // Arrange
            var client = _factory
                .WithDataStoreList(new IncidentFaker().Generate(10, 100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/incident").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var incidentList = await response.Content.ReadFromJsonAsync<List<IncidentDto>>().ConfigureAwait(false);
            Assert.NotNull(incidentList);

            // Assert
            Assert.True(incidentList.Count > 0);
        }

        [Fact]
        public async Task GetAllIncidentReturnAllIncident()
        {
            // Arrange
            var client = _factory
                .WithDataStoreList(new IncidentFaker().Generate(100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/incident?limit=100").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var incidentList = await response.Content.ReadFromJsonAsync<List<IncidentDto>>().ConfigureAwait(false);
            Assert.NotNull(incidentList);

            // Assert
            Assert.Equal(100, incidentList.Count);
        }

        [Theory]
        [ClassData(typeof(FakeIncidentData))]
        public async Task UpdateIncidentReturnNoContent(Incident incident)
        {
            // Arrange
            var newIncident = new IncidentDtoFaker().Generate(); // TODO Move outside of test
            var client = _factory
                .WithDataStoreList(incident)
                .CreateClient();
            var incidentDataStore = _factory.Services.GetService<EntityDataStore<Incident>>();

            // Act
            var response = await client.PutAsJsonAsync($"api/incident/{incident.Id}", newIncident).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(1, incidentDataStore.Entities.Count);

            // Assert
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
                .WithDataStoreList(incident)
                .CreateClient();
            var incidentDataStore = _factory.Services.GetService<EntityDataStore<Incident>>();

            // Act
            var response = await client.DeleteAsync($"api/incident/{incident.Id}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Assert
            Assert.False(incidentDataStore.IsSet);
        }
    }
}
