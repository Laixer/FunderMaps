using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.Extensions.DependencyInjection;
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
                FoundationDamageCause = FoundationDamageCause.BioInfection,
                DocumentFile = new string[] { "https://example.com/file.pdf" },
                Note = "This is an incident in testing",
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
            Assert.Equal(incident.Name, actualIncident.Name);
            Assert.Equal(incident.Email, actualIncident.Email);
            Assert.Equal(incident.PhoneNumber, actualIncident.PhoneNumber);
            Assert.Equal(incident.Address, actualIncident.Address);
        }
    }
}
