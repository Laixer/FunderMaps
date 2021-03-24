using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
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
            using var client = factory.CreateClient(OrganizationRole.Writer);
            var newObject = new RecoveryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Guid.Parse("21c403fe-45fc-4106-9551-3aada1bbdec3"))
                .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                .Generate();

            // Act
            var response = await client.PostAsJsonAsync("api/recovery", newObject);
            var returnObject = await response.Content.ReadFromJsonAsync<RecoveryDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
            Assert.Null(returnObject.UpdateDate);

            return returnObject;
        }

        public static async Task<InquiryDto> CreateInquiryAsync(BackendFixtureFactory factory)
        {
            // Arrange
            var inquiry = new InquiryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Guid.Parse("21c403fe-45fc-4106-9551-3aada1bbdec3"))
                .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                .Generate();
            using var client = factory.CreateClient(OrganizationRole.Writer);

            // Act
            var response = await client.PostAsJsonAsync("api/inquiry", inquiry);
            var returnObject = await response.Content.ReadFromJsonAsync<InquiryDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
            Assert.Null(returnObject.UpdateDate);

            return returnObject;
        }

        public static async Task DeleteRecoveryAsync(BackendFixtureFactory factory, RecoveryDto recovery)
        {
            // Arrange
            using var client = factory.CreateClient(OrganizationRole.Superuser);

            // Act
            var response = await client.DeleteAsync($"api/recovery/{recovery.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        public static async Task DeleteInquiryAsync(BackendFixtureFactory factory, InquiryDto inquiry)
        {
            // Arrange
            using var client = factory.CreateClient(OrganizationRole.Superuser);

            // Act
            var response = await client.DeleteAsync($"api/inquiry/{inquiry.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        public static async Task<RecoverySampleDto> CreateRecoverySampleAsync(BackendFixtureFactory factory, RecoveryDto recovery)
        {
            // Arrange
            using var client = factory.CreateClient(OrganizationRole.Writer);
            var newObject = new RecoverySampleDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                .Generate();

            // Act
            var response = await client.PostAsJsonAsync($"api/recovery/{recovery.Id}/sample", newObject);
            var returnObject = await response.Content.ReadFromJsonAsync<RecoverySampleDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(newObject.Address, returnObject.Address);

            return returnObject;
        }
    }
}
