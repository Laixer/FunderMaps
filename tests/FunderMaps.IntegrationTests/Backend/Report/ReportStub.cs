using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    /// <summary>
    ///     Teststub for all report tests.
    /// </summary>
    public static class ReportStub
    {
        public static async Task<RecoveryDto> CreateRecoveryAsync(BackendFixtureFactory factory)
        {
            // Arrange
            var recovery = new RecoveryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Guid.Parse("21c403fe-45fc-4106-9551-3aada1bbdec3"))
                .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                .Generate();
            using var client = factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/recovery", recovery);
            var returnObject = await response.Content.ReadFromJsonAsync<RecoveryDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
            Assert.Null(returnObject.UpdateDate);

            return returnObject;
        }

        public static async Task DeleteRecoveryAsync(BackendFixtureFactory factory, RecoveryDto recovery)
        {
            // Arrange
            using var client = factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/recovery/{recovery.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
