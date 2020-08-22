using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class InquiryTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public InquiryTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateInquiryReturnInquiry()
        {
            // Arrange
            var inquiry = new InquiryDtoFaker().Generate();
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/inquiry", inquiry);
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
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();

            // TODO: Test using faker?
            using var byteArrayContent = new ByteArrayContent(new byte[] { 0x0, 0x0 });
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
            using var formContent = new MultipartFormDataContent
            {
                { byteArrayContent, "input", "inputfile.pdf" }
            };

            // Act
            var response = await client.PostAsync("api/inquiry/upload-document", formContent); // TODO: There is no such controller?
            var returnObject = await response.Content.ReadFromJsonAsync<DocumentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Name);
        }

        [Fact]
        public async Task GetInquiryByIdReturnSingleInquiry()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}");
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
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            for (int i = 0; i < 10; i++)
            {
                await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());
            }

            // Act
            var response = await client.GetAsync($"api/inquiry?limit=10");
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
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());

            // Act
            var response = await client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}", newInquiry);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task SetStatusReviewInquiryReturnNoContent()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());
            await client.PostAsJsonGetFromJsonAsync<InquirySampleDto, InquirySampleDto>($"api/inquiry/{inquiry.Id}/sample", new InquirySampleDtoFaker().Generate());

            // Act
            var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/status_review", new StatusChangeDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("status_rejected")]
        [InlineData("status_approved")]
        public async Task SetStatusRejectedAndApprovedInquiryReturnNoContent(string uri)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());
            await client.PostAsJsonGetFromJsonAsync<InquirySampleDto, InquirySampleDto>($"api/inquiry/{inquiry.Id}/sample", new InquirySampleDtoFaker().Generate());
            await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/status_review", new StatusChangeDtoFaker().Generate());

            // Act
            var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/{uri}", new StatusChangeDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteInquiryReturnNoContent()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());

            // Act
            var response = await client.DeleteAsync($"api/inquiry/{inquiry.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
