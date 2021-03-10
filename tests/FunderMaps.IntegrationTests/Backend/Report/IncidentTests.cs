using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using FunderMaps.AspNetCore.DataTransferObjects;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class IncidentTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task CreateIncidentReturnIncident()
        {
            // Arrange
            var incident = new IncidentDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .Generate();
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/incident", incident);
            var returnObject = await response.Content.ReadFromJsonAsync<IncidentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.StartsWith("FIR", returnObject.Id, StringComparison.InvariantCulture);
            Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
        }

        [Fact]
        public async Task UploadDocumentReturnDocument()
        {
            // Arrange
            using var formContent = new FileUploadContent(mediaType: "application/pdf", fileExtension: "pdf");
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsync("api/incident/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<DocumentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Name);
        }

        [Fact]
        public async Task GetIncidentByIdReturnSingleIncident()
        {
            // Arrange
            var incident = new IncidentDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .Generate();
            using var client = Factory.CreateClient();
            incident = await client.PostAsJsonGetFromJsonAsync<IncidentDto, IncidentDto>("api/incident", incident);

            // Act
            var response = await client.GetAsync($"api/incident/{incident.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<IncidentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.StartsWith("FIR", returnObject.Id, StringComparison.InvariantCulture);
            Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
        }

        [Fact]
        public async Task UpdateIncidentReturnNoContent()
        {
            // Arrange
            var incident = new IncidentDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .Generate();
            using var client = Factory.CreateClient();
            incident = await client.PostAsJsonGetFromJsonAsync<IncidentDto, IncidentDto>("api/incident", incident);

            // Act
            var response = await client.PutAsJsonAsync($"api/incident/{incident.Id}", new IncidentDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteIncidentReturnNoContent()
        {
            // Arrange
            var incident = new IncidentDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .Generate();
            using var client = Factory.CreateClient();
            incident = await client.PostAsJsonGetFromJsonAsync<IncidentDto, IncidentDto>("api/incident", incident);

            // Act
            var response = await client.DeleteAsync($"api/incident/{incident.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
