﻿using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Extensions;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class IncidentTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public IncidentTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateIncidentReturnIncident()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/incident", new IncidentDtoFaker().Generate());
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
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            using var byteArrayContent = new ByteArrayContent(new byte[] { 0x0, 0x0 });
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
            using var formContent = new MultipartFormDataContent
            {
                { byteArrayContent, "input", "inputfile.pdf" }
            };

            // Act
            var response = await client.PostAsync("api/inquiry/upload-document", formContent); // TODO: There is no such controller?
            var returnObject = await response.Content.ReadFromJsonAsync<DocumentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Name);
        }

        [Fact]
        public async Task GetIncidentByIdReturnSingleIncident()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var incident = await client.PostAsJsonGetFromJsonAsync<IncidentDto, IncidentDto>("api/incident", new IncidentDtoFaker().Generate());

            // Act
            var response = await client.GetAsync($"api/incident/{incident.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<IncidentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.StartsWith("FIR", returnObject.Id, StringComparison.InvariantCulture);
            Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
        }

        [Fact]
        public async Task GetAllIncidentReturnNavigationIncident()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(new IncidentFaker().Generate(10, 100))
                .CreateClient();
            for (int i = 0; i < 10; i++)
            {
                await client.PostAsJsonGetFromJsonAsync<IncidentDto, IncidentDto>("api/incident", new IncidentDtoFaker().Generate());
            }

            // Act
            var response = await client.GetAsync($"api/incident?limit=10");
            var returnList = await response.Content.ReadFromJsonAsync<List<IncidentDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(10, returnList.Count);
        }

        [Fact]
        public async Task UpdateIncidentReturnNoContent()
        {
            // Arrange
            var newIncident = new IncidentDtoFaker().Generate();
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var incident = await client.PostAsJsonGetFromJsonAsync<IncidentDto, IncidentDto>("api/incident", new IncidentDtoFaker().Generate());

            // Act
            var response = await client.PutAsJsonAsync($"api/incident/{incident.Id}", newIncident);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteIncidentReturnNoContent()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var incident = await client.PostAsJsonGetFromJsonAsync<IncidentDto, IncidentDto>("api/incident", new IncidentDtoFaker().Generate());

            // Act
            var response = await client.DeleteAsync($"api/incident/{incident.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
