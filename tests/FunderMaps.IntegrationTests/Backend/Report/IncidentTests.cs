using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using FunderMaps.AspNetCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class IncidentTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public IncidentTests(AuthBackendWebApplicationFactory factory)
        {
            _client = factory
                .WithAuthenticationStores()
                .CreateClient();
        }

        // [Fact]
        // public async Task CreateIncidentReturnIncident()
        // {
        //     // Act
        //     var response = await _client.PostAsJsonAsync("api/incident", new IncidentDtoFaker().Generate());
        //     var returnObject = await response.Content.ReadFromJsonAsync<IncidentDto>();

        //     // Assert
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //     Assert.StartsWith("FIR", returnObject.Id, StringComparison.InvariantCulture);
        //     Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
        // }

        [Fact]
        public async Task UploadDocumentReturnDocument()
        {
            // Arrange
            using var formContent = new FileUploadContent(mediaType: "application/pdf", fileExtension: "pdf");

            // Act
            var response = await _client.PostAsync("api/incident/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<DocumentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Name);
        }

        // [Fact]
        // public async Task GetIncidentByIdReturnSingleIncident()
        // {
        //     // Arrange
        //     var incident = await _client.PostAsJsonGetFromJsonAsync<IncidentDto, IncidentDto>("api/incident", new IncidentDtoFaker().Generate());

        //     // Act
        //     var response = await _client.GetAsync($"api/incident/{incident.Id}");
        //     var returnObject = await response.Content.ReadFromJsonAsync<IncidentDto>();

        //     // Assert
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //     Assert.StartsWith("FIR", returnObject.Id, StringComparison.InvariantCulture);
        //     Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
        // }

        // [Fact]
        // public async Task GetAllIncidentReturnNavigationIncident()
        // {
        //     // Arrange
        //     for (int i = 0; i < 10; i++)
        //     {
        //         await _client.PostAsJsonGetFromJsonAsync<IncidentDto, IncidentDto>("api/incident", new IncidentDtoFaker().Generate());
        //     }

        //     // Act
        //     var response = await _client.GetAsync($"api/incident?limit=10");
        //     var returnList = await response.Content.ReadFromJsonAsync<List<IncidentDto>>();

        //     // Assert
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //     Assert.Equal(10, returnList.Count);
        // }

        // [Fact]
        // public async Task UpdateIncidentReturnNoContent()
        // {
        //     // Arrange
        //     var newIncident = new IncidentDtoFaker().Generate();
        //     var incident = await _client.PostAsJsonGetFromJsonAsync<IncidentDto, IncidentDto>("api/incident", new IncidentDtoFaker().Generate());

        //     // Act
        //     var response = await _client.PutAsJsonAsync($"api/incident/{incident.Id}", newIncident);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        // }

        // [Fact]
        // public async Task DeleteIncidentReturnNoContent()
        // {
        //     // Arrange
        //     var incident = await _client.PostAsJsonGetFromJsonAsync<IncidentDto, IncidentDto>("api/incident", new IncidentDtoFaker().Generate());

        //     // Act
        //     var response = await _client.DeleteAsync($"api/incident/{incident.Id}");

        //     // Assert
        //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        // }
    }
}
