using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class RecoverySampleTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public RecoverySampleTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
        }

        [Fact]
        public async Task CreateRecoverySampleReturnRecoverySample()
        {
            // Arrange
            var recovery = await _client.PostAsJsonGetFromJsonAsync<RecoveryDto, RecoveryDto>("api/recovery", new RecoveryDtoFaker().Generate());

            // Act
            var response = await _client.PostAsJsonAsync($"api/recovery/{recovery.Id}/sample", new RecoverySampleDtoFaker().Generate());
            var returnObject = await response.Content.ReadFromJsonAsync<RecoverySampleDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(recovery.Id, returnObject.Recovery);
        }

        [Fact]
        public async Task GetRecoverySampleByIdReturnSingleRecoverySample()
        {
            // Arrange
            var recovery = await _client.PostAsJsonGetFromJsonAsync<RecoveryDto, RecoveryDto>("api/recovery", new RecoveryDtoFaker().Generate());
            var sample = await _client.PostAsJsonGetFromJsonAsync<RecoverySampleDto, RecoverySampleDto>($"api/recovery/{recovery.Id}/sample", new RecoverySampleDtoFaker().Generate());

            // Act
            var response = await _client.GetAsync($"api/recovery/{recovery.Id}/sample/{sample.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<RecoverySampleDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(sample.Id, returnObject.Id);
            Assert.Equal(recovery.Id, returnObject.Recovery);
        }

        [Fact]
        public async Task GetAllRecoverySampleReturnNavigationRecoverySample()
        {
            // Arrange
            var recovery = await _client.PostAsJsonGetFromJsonAsync<RecoveryDto, RecoveryDto>("api/recovery", new RecoveryDtoFaker().Generate());
            for (int i = 0; i < 10; i++)
            {
                await _client.PostAsJsonGetFromJsonAsync<RecoverySampleDto, RecoverySampleDto>($"api/recovery/{recovery.Id}/sample", new RecoverySampleDtoFaker().Generate());
            }

            // Act
            var response = await _client.GetAsync($"api/recovery/{recovery.Id}/sample?limit=10");
            var returnList = await response.Content.ReadFromJsonAsync<List<RecoverySampleDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(10, returnList.Count);
        }

        [Fact]
        public async Task UpdateRecoverySampleReturnNoContent()
        {
            // Arrange
            var newSample = new RecoverySampleFaker().Generate();
            var recovery = await _client.PostAsJsonGetFromJsonAsync<RecoveryDto, RecoveryDto>("api/recovery", new RecoveryDtoFaker().Generate());
            var sample = await _client.PostAsJsonGetFromJsonAsync<RecoverySampleDto, RecoverySampleDto>($"api/recovery/{recovery.Id}/sample", new RecoverySampleDtoFaker().Generate());

            // Act
            var response = await _client.PutAsJsonAsync($"api/recovery/{recovery.Id}/sample/{sample.Id}", newSample);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteRecoverySampleReturnNoContent()
        {
            // Arrange
            var recovery = await _client.PostAsJsonGetFromJsonAsync<RecoveryDto, RecoveryDto>("api/recovery", new RecoveryDtoFaker().Generate());
            var sample = await _client.PostAsJsonGetFromJsonAsync<RecoverySampleDto, RecoverySampleDto>($"api/recovery/{recovery.Id}/sample", new RecoverySampleDtoFaker().Generate());

            // Act
            var response = await _client.DeleteAsync($"api/recovery/{recovery.Id}/sample/{sample.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
