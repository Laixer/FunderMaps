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

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class RecoverySampleTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public RecoverySampleTests(AuthBackendWebApplicationFactory factory)
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
            var response = await client.PostAsJsonAsync($"api/recovery/{recovery.Id}/sample", sample);
            var returnObject = await response.Content.ReadFromJsonAsync<RecoverySampleDto>();

            // Assert
            Assert.True(recoverySampleDataStore.IsSet);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(sample.Note, returnObject.Note);
            Assert.Equal(sample.Address, returnObject.Address);
            Assert.Equal(sample.Status, returnObject.Status);
            Assert.Equal(sample.Type, returnObject.Type);
            Assert.Equal(sample.PileType, returnObject.PileType);
            Assert.Equal(sample.Contractor, returnObject.Contractor);
            Assert.Equal(sample.Facade, returnObject.Facade);
            Assert.Equal(sample.Permit, returnObject.Permit);
            Assert.Equal(sample.PermitDate, returnObject.PermitDate);
            Assert.Equal(sample.RecoveryDate, returnObject.RecoveryDate);
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
            var response = await client.GetAsync($"api/recovery/{recovery.Id}/sample/{sample.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<RecoverySampleDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(sample.Note, returnObject.Note);
            Assert.Equal(sample.Address, returnObject.Address);
            Assert.Equal(sample.Status, returnObject.Status);
            Assert.Equal(sample.Type, returnObject.Type);
            Assert.Equal(sample.PileType, returnObject.PileType);
            Assert.Equal(sample.Contractor, returnObject.Contractor);
            Assert.Equal(sample.Facade, returnObject.Facade);
            Assert.Equal(sample.Permit, returnObject.Permit);
            Assert.Equal(sample.PermitDate, returnObject.PermitDate);
            Assert.Equal(sample.RecoveryDate, returnObject.RecoveryDate);
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
            var response = await client.GetAsync($"api/recovery/{recovery.Id}/sample");
            var returnList = await response.Content.ReadFromJsonAsync<List<RecoverySampleDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count > 0);
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
            var response = await client.GetAsync($"api/recovery/{recovery.Id}/sample?limit=100");
            var returnList = await response.Content.ReadFromJsonAsync<List<RecoverySampleDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(100, returnList.Count);
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
            var response = await client.PutAsJsonAsync($"api/recovery/{recovery.Id}/sample/{sample.Id}", newSample);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(1, recoverySampleDataStore.Entities.Count);
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
            var response = await client.DeleteAsync($"api/recovery/{recovery.Id}/sample/{sample.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.False(recoverySampleDataStore.IsSet);
        }
    }
}
