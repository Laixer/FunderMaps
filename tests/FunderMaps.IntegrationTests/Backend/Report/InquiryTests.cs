using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
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
    public class InquiryTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquiryTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task CreateInquiryReturnInquiry()
        {
            // Arrange
            var inquiry = new InquiryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Factory.Verifier.User.Id)
                .RuleFor(f => f.Contractor, f => Factory.Organization.Id)
                .Generate();
            using var client = Factory.CreateClient();

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
            using var formContent = new FileUploadContent(mediaType: "application/pdf", fileExtension: "pdf");
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsync("api/inquiry/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<DocumentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Name);
        }

        [Fact]
        public async Task GetInquiryByIdReturnSingleInquiry()
        {
            // Arrange
            var inquiry = new InquiryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Factory.Verifier.User.Id)
                .RuleFor(f => f.Contractor, f => Factory.Organization.Id)
                .Generate();
            using var client = Factory.CreateClient();
            inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", inquiry);

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
            var inquiry = new InquiryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Factory.Verifier.User.Id)
                .RuleFor(f => f.Contractor, f => Factory.Organization.Id)
                .Generate();
            using var client = Factory.CreateClient();
            inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", inquiry);

            // Act
            var response = await client.GetAsync($"api/inquiry?limit=10");
            var returnList = await response.Content.ReadFromJsonAsync<List<InquiryDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count >= 1);
        }

        [Fact]
        public async Task UpdateInquiryReturnNoContent()
        {
            // Arrange
            var inquiries = new InquiryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Factory.Verifier.User.Id)
                .RuleFor(f => f.Contractor, f => Factory.Organization.Id)
                .Generate(2);
            using var client = Factory.CreateClient();
            var inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", inquiries.First());

            // Act
            var response = await client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}", inquiries.Last());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task SetStatusReviewInquiryReturnNoContent()
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
            var inquiry = new InquiryDtoFaker()
                .RuleFor(f => f.Reviewer, f => Factory.Verifier.User.Id)
                .RuleFor(f => f.Contractor, f => Factory.Organization.Id)
                .Generate();
            using var client = Factory.CreateClient();
            inquiry = await client.PostAsJsonGetFromJsonAsync<InquiryDto, InquiryDto>("api/inquiry", inquiry);

            // Act
            var response = await client.DeleteAsync($"api/inquiry/{inquiry.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteInquiryCascadeReturnNoContent()
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
            var response = await client.DeleteAsync($"api/inquiry/{inquiry.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
