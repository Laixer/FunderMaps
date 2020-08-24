using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class RecoveryTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public RecoveryTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateRecoveryReturnRecovery()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/recovery", new RecoveryDtoFaker().Generate());
            var returnObject = await response.Content.ReadFromJsonAsync<RecoveryDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
            Assert.Null(returnObject.UpdateDate);
        }

        [Fact]
        public async Task GetRecoveryByIdReturnSingleRecovery()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var recovery = await client.PostAsJsonGetFromJsonAsync<RecoveryDto, RecoveryDto>("api/recovery", new RecoveryDtoFaker().Generate());

            // Act
            var response = await client.GetAsync($"api/recovery/{recovery.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<RecoveryDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
            Assert.Null(returnObject.UpdateDate);
        }

        [Fact]
        public async Task GetAllRecoveryReturnNavigationRecovery()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            for (int i = 0; i < 10; i++)
            {
                await client.PostAsJsonGetFromJsonAsync<RecoveryDto, RecoveryDto>("api/recovery", new RecoveryDtoFaker().Generate());
            }

            // Act
            var response = await client.GetAsync($"api/recovery?limit=10");
            var returnList = await response.Content.ReadFromJsonAsync<List<RecoveryDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(10, returnList.Count);
        }

        [Fact]
        public async Task UpdateRecoveryReturnNoContent()
        {
            // Arrange
            var newRecovery = new RecoveryDtoFaker().Generate();
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var recovery = await client.PostAsJsonGetFromJsonAsync<RecoveryDto, RecoveryDto>("api/recovery", new RecoveryDtoFaker().Generate());

            // Act
            var response = await client.PutAsJsonAsync($"api/recovery/{recovery.Id}", newRecovery);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteRecoveryReturnNoContent()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var recovery = await client.PostAsJsonGetFromJsonAsync<RecoveryDto, RecoveryDto>("api/recovery", new RecoveryDtoFaker().Generate());

            // Act
            var response = await client.DeleteAsync($"api/recovery/{recovery.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
