using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class InquirySampleTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquirySampleTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task CreateInquirySampleReturnInquirySample()
        {
            // Arrange
            var inquiry = new InquiryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Factory.Verifier.User.Id)
                .RuleFor(f => f.Contractor, f => Factory.Organization.Id)
                .Generate();
            var sample = new InquirySampleDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .Generate();
            using var client = Factory.CreateClient();
            inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", inquiry);

            // Act
            var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/sample", sample);
            var returnObject = await response.Content.ReadFromJsonAsync<InquirySampleDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(inquiry.Id, returnObject.Inquiry);
        }

        [Fact]
        public async Task GetInquirySampleByIdReturnSingleInquirySample()
        {
            // Arrange
            var inquiry = new InquiryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Factory.Verifier.User.Id)
                .RuleFor(f => f.Contractor, f => Factory.Organization.Id)
                .Generate();
            var sample = new InquirySampleDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .Generate();
            using var client = Factory.CreateClient();
            inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", inquiry);
            sample = await client.PostAsJsonGetFromJsonAsync<InquirySampleDto, InquirySampleDto>($"api/inquiry/{inquiry.Id}/sample", sample);

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}");
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
            var inquiry = new InquiryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Factory.Verifier.User.Id)
                .RuleFor(f => f.Contractor, f => Factory.Organization.Id)
                .Generate();
            var sample = new InquirySampleDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .Generate();
            using var client = Factory.CreateClient();
            inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", inquiry);
            sample = await client.PostAsJsonGetFromJsonAsync<InquirySampleDto, InquirySampleDto>($"api/inquiry/{inquiry.Id}/sample", sample);

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}/sample?limit=10");
            var returnList = await response.Content.ReadFromJsonAsync<List<InquirySampleDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count >= 1);
        }

        [Fact]
        public async Task UpdateInquirySampleReturnNoContent()
        {
            // Arrange
            var inquiry = new InquiryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Factory.Verifier.User.Id)
                .RuleFor(f => f.Contractor, f => Factory.Organization.Id)
                .Generate();
            var samples = new InquirySampleDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .Generate(2);
            using var client = Factory.CreateClient();
            inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", inquiry);
            var sample = await client.PostAsJsonGetFromJsonAsync<InquirySampleDto, InquirySampleDto>($"api/inquiry/{inquiry.Id}/sample", samples.First());

            // Act
            var response = await client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}", samples.Last());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteInquirySampleReturnNoContent()
        {
            // Arrange
            var inquiry = new InquiryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Factory.Verifier.User.Id)
                .RuleFor(f => f.Contractor, f => Factory.Organization.Id)
                .Generate();
            var sample = new InquirySampleDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .Generate();
            using var client = Factory.CreateClient();
            inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", inquiry);
            sample = await client.PostAsJsonGetFromJsonAsync<InquirySampleDto, InquirySampleDto>($"api/inquiry/{inquiry.Id}/sample", sample);

            // Act
            var response = await client.DeleteAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
