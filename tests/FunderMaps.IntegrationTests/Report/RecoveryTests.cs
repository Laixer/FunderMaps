using FunderMaps.Core.Entities;
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
    public class RecoveryTests : IClassFixture<AuthWebApplicationFactory<Startup>>
    {
        private readonly AuthWebApplicationFactory<Startup> _factory;

        public RecoveryTests(AuthWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        internal class FakeRecoveryDtoData : EnumerableHelper<RecoveryDto>
        {
            protected override IEnumerable<RecoveryDto> GetEnumerableEntity()
            {
                return new RecoveryDtoFaker().Generate(10, 100);
            }
        }

        internal class FakeRecoveryData : EnumerableHelper<Recovery>
        {
            protected override IEnumerable<Recovery> GetEnumerableEntity()
            {
                return new RecoveryFaker().Generate(10, 100);
            }
        }

        [Theory]
        [ClassData(typeof(FakeRecoveryDtoData))]
        public async Task CreateRecoveryReturnRecovery(RecoveryDto recovery)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var recoveryDataStore = _factory.Services.GetService<EntityDataStore<Recovery>>();

            // Act
            var response = await client.PostAsJsonAsync("api/recovery", recovery).ConfigureAwait(false);
            var actualRecovery = await response.Content.ReadFromJsonAsync<RecoveryDto>().ConfigureAwait(false);

            // Assert
            //Assert.Equal(AuditStatus.Todo, actualRecovery.AuditStatus);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(recoveryDataStore.IsSet);
            Assert.Equal(recovery.Note, actualRecovery.Note);
            Assert.Equal(recovery.Type, actualRecovery.Type);
            Assert.Equal(recovery.DocumentFile, actualRecovery.DocumentFile);
            Assert.Equal(recovery.DocumentDate, actualRecovery.DocumentDate);
            Assert.Equal(recovery.AccessPolicy, actualRecovery.AccessPolicy);
        }

        [Theory]
        [ClassData(typeof(FakeRecoveryData))]
        public async Task GetRecoveryByIdReturnSingleRecovery(Recovery recovery)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(recovery)
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/recovery/{recovery.Id}").ConfigureAwait(false);
            var actualRecovery = await response.Content.ReadFromJsonAsync<RecoveryDto>().ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(recovery.Note, actualRecovery.Note);
            Assert.Equal(recovery.Type, actualRecovery.Type);
            Assert.Equal(recovery.DocumentFile, actualRecovery.DocumentFile);
            Assert.Equal(recovery.DocumentDate, actualRecovery.DocumentDate);
            Assert.Equal(recovery.AccessPolicy, actualRecovery.AccessPolicy);
        }

        [Fact]
        public async Task GetAllRecoveryReturnPageRecovery()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(new RecoveryFaker().Generate(10, 100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/recovery").ConfigureAwait(false);
            var recoveryList = await response.Content.ReadFromJsonAsync<List<RecoveryDto>>().ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(recoveryList.Count > 0);
        }

        [Fact]
        public async Task GetAllRecoveryReturnAllRecovery()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(new RecoveryFaker().Generate(100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/recovery?limit=100").ConfigureAwait(false);
            var recoveryList = await response.Content.ReadFromJsonAsync<List<RecoveryDto>>().ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(100, recoveryList.Count);
        }

        [Theory]
        [ClassData(typeof(FakeRecoveryData))]
        public async Task UpdateRecoveryReturnNoContent(Recovery recovery)
        {
            // Arrange
            var newRecovery = new RecoveryDtoFaker().Generate(); // TODO Move outside of test
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(recovery)
                .CreateClient();
            var recoveryDataStore = _factory.Services.GetService<EntityDataStore<Recovery>>();

            // Act
            var response = await client.PutAsJsonAsync($"api/recovery/{recovery.Id}", newRecovery).ConfigureAwait(false);

            // Assert
            var actualRecovery = recoveryDataStore.Entities[0];
            Assert.Equal(recovery.Id, actualRecovery.Id);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(1, recoveryDataStore.Entities.Count);
            Assert.Equal(newRecovery.Note, actualRecovery.Note);
            Assert.Equal(newRecovery.Type, actualRecovery.Type);
            Assert.Equal(newRecovery.DocumentFile, actualRecovery.DocumentFile);
            Assert.Equal(newRecovery.DocumentDate, actualRecovery.DocumentDate);
            Assert.Equal(newRecovery.AccessPolicy, actualRecovery.AccessPolicy);
        }

        [Theory]
        [ClassData(typeof(FakeRecoveryData))]
        public async Task DeleteRecoveryReturnNoContent(Recovery recovery)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(recovery)
                .CreateClient();
            var recoveryDataStore = _factory.Services.GetService<EntityDataStore<Recovery>>();

            // Act
            var response = await client.DeleteAsync($"api/recovery/{recovery.Id}").ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.False(recoveryDataStore.IsSet);
        }
    }
}
