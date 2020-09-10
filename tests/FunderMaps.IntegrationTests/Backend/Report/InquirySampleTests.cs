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
    public class InquirySampleTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public InquirySampleTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory
                .WithAuthenticationStores()
                .CreateClient();
        }

        [Fact]
        public async Task CreateInquirySampleReturnInquirySample()
        {
            // Arrange
            var inquiry = await _client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());

            // Act
            var response = await _client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/sample", new InquirySampleDtoFaker().Generate());
            var returnObject = await response.Content.ReadFromJsonAsync<InquirySampleDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(inquiry.Id, returnObject.Inquiry);
        }

        [Fact]
        public async Task GetInquirySampleByIdReturnSingleInquirySample()
        {
            // Arrange
            var inquiry = await _client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());
            var sample = await _client.PostAsJsonGetFromJsonAsync<InquirySampleDto, InquirySampleDto>($"api/inquiry/{inquiry.Id}/sample", new InquirySampleDtoFaker().Generate());

            // Act
            var response = await _client.GetAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<InquirySampleDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(sample.Id, returnObject.Id);
            Assert.Equal(inquiry.Id, returnObject.Inquiry);
        }

        [Fact]
        public async Task GetAllInquirySampleReturnNavigationInquirySample()
        {
            // Arrange
            var inquiry = await _client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());
            for (int i = 0; i < 10; i++)
            {
                await _client.PostAsJsonGetFromJsonAsync<InquirySampleDto, InquirySampleDto>($"api/inquiry/{inquiry.Id}/sample", new InquirySampleDtoFaker().Generate());
            }

            // Act
            var response = await _client.GetAsync($"api/inquiry/{inquiry.Id}/sample?limit=10");
            var returnList = await response.Content.ReadFromJsonAsync<List<InquirySampleDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(10, returnList.Count);
        }

        [Fact]
        public async Task UpdateInquirySampleReturnNoContent()
        {
            // Arrange
            var newSample = new InquirySampleFaker().Generate();
            var inquiry = await _client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());
            var sample = await _client.PostAsJsonGetFromJsonAsync<InquirySampleDto, InquirySampleDto>($"api/inquiry/{inquiry.Id}/sample", new InquirySampleDtoFaker().Generate());

            // Act
            var response = await _client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}", newSample);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteInquirySampleReturnNoContent()
        {
            // Arrange
            var inquiry = await _client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", new InquiryDtoFaker().Generate());
            var sample = await _client.PostAsJsonGetFromJsonAsync<InquirySampleDto, InquirySampleDto>($"api/inquiry/{inquiry.Id}/sample", new InquirySampleDtoFaker().Generate());

            // Act
            var response = await _client.DeleteAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
