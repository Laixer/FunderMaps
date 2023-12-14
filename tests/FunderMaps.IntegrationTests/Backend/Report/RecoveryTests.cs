using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class RecoveryTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RecoveryTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task UploadDocumentReturnDocument()
        {
            // Arrange
            using var formContent = new FileUploadContent(mediaType: "application/pdf", fileExtension: "pdf");
            using var client = Factory.CreateClient(OrganizationRole.Writer);

            // Act
            var response = await client.PostAsync("api/recovery/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<DocumentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
            Assert.NotNull(returnObject.Name);
        }

        [Fact]
        public async Task UploadDocumentReturnForbidden()
        {
            // Arrange
            using var formContent = new FileUploadContent(mediaType: "application/pdf", fileExtension: "pdf");
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsync("api/recovery/upload-document", formContent);

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
            var response = await client.PostAsync("api/recovery/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(returnObject);
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
            var response = await client.PostAsync("api/recovery/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(returnObject);
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
            var response = await client.PostAsync("api/recovery/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(returnObject);
            Assert.Equal((short)HttpStatusCode.BadRequest, returnObject.Status);
            Assert.Contains("validation", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
        }


        [Fact]
        public async Task RecoveryLifeCycle()
        {
            var recovery = await ReportStub.CreateRecoveryAsync(Factory);

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/recovery/{recovery.Id}/download");
                var returnObject = await response.Content.ReadFromJsonAsync<BlobAccessLinkDto>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(returnObject);
                Assert.Equal("https", returnObject.AccessLink?.Scheme);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/recovery/stats");
                var returnObject = await response.Content.ReadFromJsonAsync<DatasetStatsDto>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(returnObject);
                Assert.True(returnObject.Count >= 1);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/recovery/{recovery.Id}");
                var returnObject = await response.Content.ReadFromJsonAsync<Recovery>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(returnObject);
                Assert.Equal(AuditStatus.Todo, returnObject.State.AuditStatus);
                Assert.Null(returnObject.Record.UpdateDate);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/recovery");
                var returnList = await response.Content.ReadFromJsonAsync<List<Recovery>>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(returnList);
                Assert.True(returnList.Count >= 1);
            }

            {
                // Arrange
                using var client = Factory.CreateClient(OrganizationRole.Writer);
                var newObject = new RecoveryFaker()
                    .RuleFor(f => f.Attribution.Reviewer, f => Guid.Parse("21c403fe-45fc-4106-9551-3aada1bbdec3"))
                    .RuleFor(f => f.Attribution.Contractor, f => 10)
                    .Generate();

                // Act
                var response = await client.PutAsJsonAsync($"api/recovery/{recovery.Id}", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            await ReportStub.DeleteRecoveryAsync(Factory, recovery);
        }

        [Theory]
        [InlineData(OrganizationRole.Reader)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Superuser)]
        public async Task RecoveryLifeCycleForbidden(OrganizationRole role)
        {
            var recovery = await ReportStub.CreateRecoveryAsync(Factory);

            {
                // Arrange
                using var client = Factory.CreateClient();
                var newObject = new RecoveryFaker()
                    .RuleFor(f => f.Attribution.Reviewer, f => Guid.Parse("21c403fe-45fc-4106-9551-3aada1bbdec3"))
                    .RuleFor(f => f.Attribution.Contractor, f => 10)
                    .Generate();

                // Act
                var response = await client.PostAsJsonAsync("api/recovery", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();
                var newObject = new RecoveryFaker()
                    .RuleFor(f => f.Attribution.Reviewer, f => Guid.Parse("21c403fe-45fc-4106-9551-3aada1bbdec3"))
                    .RuleFor(f => f.Attribution.Contractor, f => 10)
                    .Generate();

                // Act
                var response = await client.PutAsJsonAsync($"api/recovery/{recovery.Id}", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient(role);

                // Act
                var response = await client.PostAsJsonAsync($"api/recovery/{recovery.Id}/status_review", new StatusChangeDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient(role);

                // Act
                var response = await client.PostAsJsonAsync($"api/recovery/{recovery.Id}/status_approved", new StatusChangeDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient(role);

                // Act
                var response = await client.PostAsJsonAsync($"api/recovery/{recovery.Id}/status_rejected", new StatusChangeDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.DeleteAsync($"api/recovery/{recovery.Id}");

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            await ReportStub.DeleteRecoveryAsync(Factory, recovery);
        }

        [Fact]
        public async Task RecoverySelfReviewForbidden()
        {
            var recovery = await ReportStub.CreateRecoveryAsync(Factory);

            {
                // Arrange
                using var client = Factory.CreateClient(OrganizationRole.Writer);
                var newObject = new RecoveryFaker()
                    .RuleFor(f => f.Attribution.Reviewer, f => Guid.Parse("aadc6b80-b447-443b-b4ed-fdfcb00976f2"))
                    .RuleFor(f => f.Attribution.Contractor, f => 10)
                    .Generate();

                // Act
                var response = await client.PostAsJsonAsync("api/recovery", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();
                var newObject = new RecoveryFaker()
                    .RuleFor(f => f.Attribution.Reviewer, f => Guid.Parse("aadc6b80-b447-443b-b4ed-fdfcb00976f2"))
                    .RuleFor(f => f.Attribution.Contractor, f => 10)
                    .Generate();

                // Act
                var response = await client.PutAsJsonAsync($"api/recovery/{recovery.Id}", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            await ReportStub.DeleteRecoveryAsync(Factory, recovery);
        }
    }
}
