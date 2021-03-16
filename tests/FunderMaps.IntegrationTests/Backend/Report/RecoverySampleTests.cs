using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class RecoverySampleTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RecoverySampleTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task RecoverySampleLifeCycle()
        {
            var recovery = await ReportStub.CreateRecoveryAsync(Factory);
            var sample = await ReportStub.CreateRecoverySampleAsync(Factory, recovery);

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/recovery/{recovery.Id}/sample/{sample.Id}");
                var returnObject = await response.Content.ReadFromJsonAsync<RecoverySampleDto>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(sample.Id, returnObject.Id);
                Assert.Equal(recovery.Id, returnObject.Recovery);
            }

            {
                // // Arrange
                // using var client = Factory.CreateClient();

                // // Act
                // var response = await client.GetAsync($"api/recovery/{recovery.Id}/sample");
                // var returnList = await response.Content.ReadFromJsonAsync<List<RecoverySampleDto>>();

                // // Assert
                // Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                // Assert.True(returnList.Count >= 1);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();
                var newObject = new RecoverySampleDtoFaker()
                    .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                    .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                    .Generate();

                // Act
                var response = await client.PutAsJsonAsync($"api/recovery/{recovery.Id}/sample/{sample.Id}", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            await ReportStub.DeleteRecoverySampleAsync(Factory, recovery, sample);
            await ReportStub.DeleteRecoveryAsync(Factory, recovery);
        }

        [Fact]
        public async Task RecoveryLifeCycleForbidden()
        {
            var recovery = await ReportStub.CreateRecoveryAsync(Factory);

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.PostAsJsonAsync($"api/recovery/{recovery.Id}/status_review", new StatusChangeDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.PostAsJsonAsync($"api/recovery/{recovery.Id}/status_approved", new StatusChangeDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.PostAsJsonAsync($"api/recovery/{recovery.Id}/status_rejected", new StatusChangeDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            await ReportStub.DeleteRecoveryAsync(Factory, recovery);
        }
    }
}
