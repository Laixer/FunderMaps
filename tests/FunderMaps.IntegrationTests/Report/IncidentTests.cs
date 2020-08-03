using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
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

        [Fact]
        public async Task CreateIncidentReturnIncident()
        {
            // Arrange
            var incident = new IncidentDto
            {
                Name = "Piet Puk",
                ClientId = 3,
                Email = "piet@puk.com",
                PhoneNumber = "06360557722",
                FoundationType = FoundationType.NoPileMasonry,
                Address = "gfm-67f8fe46717947d8bcd22bee5ca092ae",
                FoundationDamageCharacteristics = new FoundationDamageCharacteristics[]
                {
                    FoundationDamageCharacteristics.JammingDoorWindow,
                    FoundationDamageCharacteristics.Crack,
                    FoundationDamageCharacteristics.ThresholdAboveSubsurface,
                },
                EnvironmentDamageCharacteristics = new EnvironmentDamageCharacteristics[]
                {
                    EnvironmentDamageCharacteristics.ConstructionNearby,
                    EnvironmentDamageCharacteristics.SaggingSewerConnection,
                },
                Owner = true,
                FoundationRecovery = true,
                NeightborRecovery = true,
                ChainedBuilding = true,
                FoundationDamageCause = FoundationDamageCause.BioInfection,
                DocumentFile = new string[] { "https://example.com/file.pdf" },
                Note = "This is an incident in testing",
                InternalNote = "An internal node",
            };
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

        public class GetIncidentByIdReturnSingleIncidentData : EnumerableHelper<Incident>
        {
            protected override IEnumerable<Incident> GetEnumerableEntity()
            {
                return new IncidentFaker().Generate(100);
            }
        }

        [Theory]
        [ClassData(typeof(GetIncidentByIdReturnSingleIncidentData))]
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
                .WithDataStoreList(new IncidentFaker().Generate(10))
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

        public class UpdateIncidentReturnNoContentData : EnumerableHelper<Incident>
        {
            protected override IEnumerable<Incident> GetEnumerableEntity()
            {
                return new IncidentFaker().Generate(100);
            }
        }

        [Theory]
        [ClassData(typeof(UpdateIncidentReturnNoContentData))]
        public async Task UpdateIncidentReturnNoContent(Incident incident)
        {
            // Arrange
            var newIncident = new IncidentDtoFaker().Generate();
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
    }
}
