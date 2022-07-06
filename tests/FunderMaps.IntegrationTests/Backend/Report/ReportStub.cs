using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
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

        public static async Task<IncidentDto> CreateIncidentAsync(BackendFixtureFactory factory)
        {
            // Arrange
            var incident = new IncidentDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .Generate();
            using var client = factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/incident", incident);
            var returnObject = await response.Content.ReadFromJsonAsync<IncidentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.StartsWith("FIR", returnObject.Id, StringComparison.InvariantCulture);
            Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);

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

        public static async Task DeleteIncidentAsync(BackendFixtureFactory factory, IncidentDto incident)
        {
            // Arrange
            using var client = factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/incident/{incident.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        public static async Task<RecoverySampleDto> CreateRecoverySampleAsync(BackendFixtureFactory factory, RecoveryDto recovery)
        {
            // Arrange
            using var client = factory.CreateClient(OrganizationRole.Writer);
            var newObject = new RecoverySampleDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-f53334d806ab4ab386e8df29111add21")
                .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                .Generate();

            // Act
            var response = await client.PostAsJsonAsync($"api/recovery/{recovery.Id}/sample", newObject);
            var returnObject = await response.Content.ReadFromJsonAsync<RecoverySampleDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(recovery.Id, returnObject.Recovery);
            Assert.Equal(newObject.Address, returnObject.Address);

            return returnObject;
        }

        public static async Task<InquirySampleDto> CreateInquirySampleAsync(BackendFixtureFactory factory, InquiryDto inquiry)
        {
            using var client = factory.CreateClient(OrganizationRole.Writer);
            var newObject = new InquirySampleDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-2687feed6a624636b70700cd374fbd17")
                .Generate();

            // Act
            var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/sample", newObject);
            var returnObject = await response.Content.ReadFromJsonAsync<InquirySampleDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(inquiry.Id, returnObject.Inquiry);
            Assert.Equal(newObject.Address, returnObject.Address);

            return returnObject;
        }
    }
}
