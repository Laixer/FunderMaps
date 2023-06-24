using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class InquiryTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquiryTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task UploadDocumentReturnDocument()
        {
            // Arrange
            using var formContent = new FileUploadContent(mediaType: "application/pdf", fileExtension: "pdf");
            using var client = Factory.CreateClient(OrganizationRole.Writer);

            // Act
            var response = await client.PostAsync("api/inquiry/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<DocumentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Name);
        }

        [Fact]
        public async Task UploadDocumentReturnForbidden()
        {
            // Arrange
            using var formContent = new FileUploadContent(mediaType: "application/pdf", fileExtension: "pdf");
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsync("api/inquiry/upload-document", formContent);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UploadEmptyFormReturnBadRequest()
        {
            // Arrange
            using var formContent = new MultipartFormDataContent();
            using var client = Factory.CreateClient(OrganizationRole.Writer);

            // Act
            var response = await client.PostAsync("api/inquiry/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal((short)HttpStatusCode.BadRequest, returnObject.Status);
            Assert.Contains("validation", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public async Task UploadEmptyDocumentReturnBadRequest()
        {
            // Arrange
            using var formContent = new FileUploadContent(
                mediaType: "application/pdf",
                fileExtension: "pdf",
                byteContentLength: 0);
            using var client = Factory.CreateClient(OrganizationRole.Writer);

            // Act
            var response = await client.PostAsync("api/inquiry/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal((short)HttpStatusCode.BadRequest, returnObject.Status);
            Assert.Contains("validation", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public async Task UploadForbiddenDocumentReturnBadRequest()
        {
            // Arrange
            using var formContent = new FileUploadContent(
                mediaType: "font/woff",
                fileExtension: "woff");
            using var client = Factory.CreateClient(OrganizationRole.Writer);

            // Act
            var response = await client.PostAsync("api/inquiry/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal((short)HttpStatusCode.BadRequest, returnObject.Status);
            Assert.Contains("validation", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public async Task InquiryLifeCycle()
        {
            var inquiry = await ReportStub.CreateInquiryAsync(Factory);

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/inquiry/{inquiry.Id}/download");
                var returnObject = await response.Content.ReadFromJsonAsync<BlobAccessLinkDto>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("https", returnObject.AccessLink.Scheme);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/inquiry/stats");
                var returnObject = await response.Content.ReadFromJsonAsync<DatasetStatsDto>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(returnObject.Count >= 1);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/inquiry/{inquiry.Id}");
                var returnObject = await response.Content.ReadFromJsonAsync<InquiryDto>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
                Assert.Null(returnObject.UpdateDate);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/inquiry");
                var returnList = await response.Content.ReadFromJsonAsync<List<InquiryDto>>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(returnList.Count >= 1);
            }

            {
                // Arrange
                using var client = Factory.CreateClient(OrganizationRole.Writer);
                var newObject = new InquiryDtoFaker()
                    .RuleFor(f => f.Reviewer, f => Guid.Parse("21c403fe-45fc-4106-9551-3aada1bbdec3"))
                    // .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                    .Generate();

                // Act
                var response = await client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            await ReportStub.DeleteInquiryAsync(Factory, inquiry);
        }

        [Theory]
        [InlineData(OrganizationRole.Reader)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Superuser)]
        public async Task InquiryLifeCycleForbidden(OrganizationRole role)
        {
            var inquiry = await ReportStub.CreateInquiryAsync(Factory);

            {
                // Arrange
                using var client = Factory.CreateClient();
                var newObject = new InquiryDtoFaker()
                    .RuleFor(f => f.Reviewer, f => Guid.Parse("21c403fe-45fc-4106-9551-3aada1bbdec3"))
                    // .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                    .Generate();

                // Act
                var response = await client.PostAsJsonAsync("api/inquiry", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();
                var newObject = new InquiryDtoFaker()
                    .RuleFor(f => f.Reviewer, f => Guid.Parse("21c403fe-45fc-4106-9551-3aada1bbdec3"))
                    // .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                    .Generate();

                // Act
                var response = await client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient(role);

                // Act
                var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/status_review", new StatusChangeDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient(role);

                // Act
                var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/status_approved", new StatusChangeDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient(role);

                // Act
                var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/status_rejected", new StatusChangeDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.DeleteAsync($"api/inquiry/{inquiry.Id}");

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            await ReportStub.DeleteInquiryAsync(Factory, inquiry);
        }

        [Fact]
        public async Task InquirySelfReviewForbidden()
        {
            var inquiry = await ReportStub.CreateInquiryAsync(Factory);

            {
                // Arrange
                using var client = Factory.CreateClient(OrganizationRole.Writer);
                var newObject = new RecoveryDtoFaker()
                    .RuleFor(f => f.Reviewer, f => Guid.Parse("aadc6b80-b447-443b-b4ed-fdfcb00976f2"))
                    // .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                    .Generate();

                // Act
                var response = await client.PostAsJsonAsync("api/inquiry", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();
                var newObject = new InquiryDtoFaker()
                    .RuleFor(f => f.Reviewer, f => Guid.Parse("aadc6b80-b447-443b-b4ed-fdfcb00976f2"))
                    // .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                    .Generate();

                // Act
                var response = await client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            await ReportStub.DeleteInquiryAsync(Factory, inquiry);
        }
    }
}
