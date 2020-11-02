using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class InquiryTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public InquiryTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory
                .WithAuthenticationStores()
                .CreateClient();
        }

        [Fact]
        public async Task CreateInquiryReturnInquiry()
        {
            // Act
            var response = await _client.PostAsJsonAsync("api/inquiry", new InquiryDtoFaker().Generate());
            var returnObject = await response.Content.ReadFromJsonAsync<InquiryDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
            Assert.Null(returnObject.UpdateDate);
        }

        [Fact]
        public async Task UploadDocumentReturnDocument()
        {
            // Arrange
            using var formContent = new FileUploadContent(mediaType: "application/pdf", fileExtension: "pdf");

            // Act
            var response = await _client.PostAsync("api/inquiry/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<DocumentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Name);
        }

        [Fact]
        public async Task GetInquiryByIdReturnSingleInquiry()
        {
            // Arrange
            var inquiry = await _client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());

            // Act
            var response = await _client.GetAsync($"api/inquiry/{inquiry.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<InquiryDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
            Assert.Null(returnObject.UpdateDate);
        }

        [Fact]
        public async Task GetAllInquiryReturnNavigationInquiry()
        {
            // Arrange
            for (int i = 0; i < 10; i++)
            {
                await _client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());
            }

            // Act
            var response = await _client.GetAsync($"api/inquiry?limit=10");
            var returnList = await response.Content.ReadFromJsonAsync<List<InquiryDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(10, returnList.Count);
        }

        [Fact]
        public async Task UpdateInquiryReturnNoContent()
        {
            // Arrange
            var newInquiry = new InquiryDtoFaker().Generate();
            var inquiry = await _client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());

            // Act
            var response = await _client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}", newInquiry);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task SetStatusReviewInquiryReturnNoContent()
        {
            // Arrange
            var inquiry = await _client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());
            await _client.PostAsJsonGetFromJsonAsync<InquirySampleDto, InquirySampleDto>($"api/inquiry/{inquiry.Id}/sample", new InquirySampleDtoFaker().Generate());

            // Act
            var response = await _client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/status_review", new StatusChangeDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("status_rejected")]
        [InlineData("status_approved")]
        public async Task SetStatusRejectedAndApprovedInquiryReturnNoContent(string uri)
        {
            // Arrange
            var inquiry = await _client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());
            await _client.PostAsJsonGetFromJsonAsync<InquirySampleDto, InquirySampleDto>($"api/inquiry/{inquiry.Id}/sample", new InquirySampleDtoFaker().Generate());
            await _client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/status_review", new StatusChangeDtoFaker().Generate());

            // Act
            var response = await _client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/{uri}", new StatusChangeDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteInquiryReturnNoContent()
        {
            // Arrange
            var inquiry = await _client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());

            // Act
            var response = await _client.DeleteAsync($"api/inquiry/{inquiry.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
