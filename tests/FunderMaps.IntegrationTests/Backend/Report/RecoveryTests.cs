using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
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
    public class RecoveryTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RecoveryTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task RecoveryLifeCycle()
        {
            var recovery = await ReportStub.CreateRecoveryAsync(Factory);

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/recovery/{recovery.Id}");
                var returnObject = await response.Content.ReadFromJsonAsync<RecoveryDto>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
                Assert.Null(returnObject.UpdateDate);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/recovery?limit=10");
                var returnList = await response.Content.ReadFromJsonAsync<List<RecoveryDto>>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(returnList.Count >= 1);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();
                var newObject = new RecoveryDtoFaker()
                    .RuleFor(f => f.Reviewer, f => Guid.Parse("21c403fe-45fc-4106-9551-3aada1bbdec3"))
                    .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                    .Generate();

                // Act
                var response = await client.PutAsJsonAsync($"api/recovery/{recovery.Id}", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

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

        [Fact]
        public async Task UploadDocumentReturnDocument()
        {
            // Arrange
            using var formContent = new FileUploadContent(mediaType: "application/pdf", fileExtension: "pdf");
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsync("api/recovery/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<DocumentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Name);
        }
    }
}
