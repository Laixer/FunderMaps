using FunderMaps.Core.Entities;
using FunderMaps.IntegrationTests.Extensions;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Report
{
    public class RecoverySampleTests : IClassFixture<AuthWebApplicationFactory<Startup>>
    {
        private readonly AuthWebApplicationFactory<Startup> _factory;

        public RecoverySampleTests(AuthWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        internal class FakeRecoverySampleDtoData : EnumerableHelper<RecoverySampleDto>
        {
            protected override IEnumerable<RecoverySampleDto> GetEnumerableEntity()
            {
                return new RecoverySampleDtoFaker().Generate(10, 100);
            }

            protected override IEnumerable<object[]> GetData()
            {
                var recovery = new RecoveryFaker();
                return GetEnumerableEntity().Select(s => new object[] { recovery.Generate(), s });
            }
        }

        internal class FakeRecoverySampleData : EnumerableHelper<RecoverySample>
        {
            protected override IEnumerable<RecoverySample> GetEnumerableEntity()
            {
                return new RecoverySampleFaker().Generate(10, 100);
            }

            protected override IEnumerable<object[]> GetData()
            {
                var recovery = new RecoveryFaker();
                return GetEnumerableEntity().Select(s =>
                {
                    var i = recovery.Generate();
                    s.Recovery = i.Id;
                    return new object[] { i, s };
                });
            }
        }

        [Theory]
        [ClassData(typeof(FakeRecoverySampleDtoData))]
        public async Task CreateRecoverySampleReturnRecoverySample(Recovery recovery, RecoverySampleDto sample)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(recovery)
                .CreateClient();
            var recoverySampleDataStore = _factory.Services.GetService<EntityDataStore<Recovery>>();

            // Act
            var response = await client.PostAsJsonAsync($"api/recovery/{recovery.Id}/sample", sample).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(recoverySampleDataStore.IsSet);
            var actualRecoverySample = await response.Content.ReadFromJsonAsync<RecoverySampleDto>().ConfigureAwait(false);

            // Assert
            Assert.Equal(sample.Note, actualRecoverySample.Note);
            Assert.Equal(sample.Address, actualRecoverySample.Address);
            Assert.Equal(sample.Status, actualRecoverySample.Status);
            Assert.Equal(sample.Type, actualRecoverySample.Type);
            Assert.Equal(sample.PileType, actualRecoverySample.PileType);
            Assert.Equal(sample.Contractor, actualRecoverySample.Contractor);
            Assert.Equal(sample.Facade, actualRecoverySample.Facade);
            Assert.Equal(sample.Permit, actualRecoverySample.Permit);
            Assert.Equal(sample.PermitDate, actualRecoverySample.PermitDate);
            Assert.Equal(sample.RecoveryDate, actualRecoverySample.RecoveryDate);
        }

        [Theory]
        [ClassData(typeof(FakeRecoverySampleData))]
        public async Task GetRecoverySampleByIdReturnSingleRecoverySample(Recovery recovery, RecoverySample sample)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(recovery)
                .WithDataStoreList(sample)
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/recovery/{recovery.Id}/sample/{sample.Id}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualRecoverySample = await response.Content.ReadFromJsonAsync<RecoverySampleDto>().ConfigureAwait(false);

            // Assert
            Assert.Equal(sample.Note, actualRecoverySample.Note);
            Assert.Equal(sample.Address, actualRecoverySample.Address);
            Assert.Equal(sample.Status, actualRecoverySample.Status);
            Assert.Equal(sample.Type, actualRecoverySample.Type);
            Assert.Equal(sample.PileType, actualRecoverySample.PileType);
            Assert.Equal(sample.Contractor, actualRecoverySample.Contractor);
            Assert.Equal(sample.Facade, actualRecoverySample.Facade);
            Assert.Equal(sample.Permit, actualRecoverySample.Permit);
            Assert.Equal(sample.PermitDate, actualRecoverySample.PermitDate);
            Assert.Equal(sample.RecoveryDate, actualRecoverySample.RecoveryDate);
        }

        [Fact]
        public async Task GetAllRecoverySampleReturnPageRecovery()
        {
            // Arrange
            var recovery = new RecoveryFaker().Generate();
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(recovery)
                .WithDataStoreList(new RecoverySampleFaker().RuleFor(f => f.Recovery, f => recovery.Id).Generate(10, 100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/recovery/{recovery.Id}/sample").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var recoverySampleList = await response.Content.ReadFromJsonAsync<List<RecoverySampleDto>>().ConfigureAwait(false);
            Assert.NotNull(recoverySampleList);

            // Assert
            Assert.True(recoverySampleList.Count > 0);
        }

        [Fact]
        public async Task GetAllRecoverySampleReturnAllRecovery()
        {
            // Arrange
            var recovery = new RecoveryFaker().Generate();
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(recovery)
                .WithDataStoreList(new RecoverySampleFaker().RuleFor(f => f.Recovery, f => recovery.Id).Generate(100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/recovery/{recovery.Id}/sample?limit=100").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var recoverySampleList = await response.Content.ReadFromJsonAsync<List<RecoverySampleDto>>().ConfigureAwait(false);
            Assert.NotNull(recoverySampleList);

            // Assert
            Assert.Equal(100, recoverySampleList.Count);
        }

        [Theory]
        [ClassData(typeof(FakeRecoverySampleData))]
        public async Task UpdateRecoverySampleReturnNoContent(Recovery recovery, RecoverySample sample)
        {
            // Arrange
            var newSample = new RecoverySampleFaker().Generate();
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(recovery)
                .WithDataStoreList(sample)
                .CreateClient();
            var recoverySampleDataStore = _factory.Services.GetService<EntityDataStore<RecoverySample>>();

            // Act
            var response = await client.PutAsJsonAsync($"api/recovery/{recovery.Id}/sample/{sample.Id}", newSample).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(1, recoverySampleDataStore.Entities.Count);

            // Assert
            var actualSample = recoverySampleDataStore.Entities[0];
            Assert.Equal(sample.Id, actualSample.Id);
            Assert.Equal(newSample.Note, actualSample.Note);
            Assert.Equal(newSample.Address, actualSample.Address);
            Assert.Equal(newSample.Status, actualSample.Status);
            Assert.Equal(newSample.Type, actualSample.Type);
            Assert.Equal(newSample.PileType, actualSample.PileType);
            Assert.Equal(newSample.Contractor, actualSample.Contractor);
            Assert.Equal(newSample.Facade, actualSample.Facade);
            Assert.Equal(newSample.Permit, actualSample.Permit);
            Assert.Equal(newSample.PermitDate, actualSample.PermitDate);
            Assert.Equal(newSample.RecoveryDate, actualSample.RecoveryDate);
        }

        [Theory]
        [ClassData(typeof(FakeRecoverySampleData))]
        public async Task DeleteRecoverySampleReturnNoContent(Recovery recovery, RecoverySample sample)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(recovery)
                .WithDataStoreList(sample)
                .CreateClient();
            var recoverySampleDataStore = _factory.Services.GetService<EntityDataStore<RecoverySample>>();

            // Act
            var response = await client.DeleteAsync($"api/recovery/{recovery.Id}/sample/{sample.Id}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Assert
            Assert.False(recoverySampleDataStore.IsSet);
        }
    }
}
