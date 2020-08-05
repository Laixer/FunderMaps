using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Extensions;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Report
{
    public class InquiryTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public InquiryTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        internal class FakeInquiryDtoData : EnumerableHelper<InquiryDto>
        {
            protected override IEnumerable<InquiryDto> GetEnumerableEntity()
            {
                return new InquiryDtoFaker().Generate(10, 1000);
            }
        }

        internal class FakeInquiryData : EnumerableHelper<Inquiry>
        {
            protected override IEnumerable<Inquiry> GetEnumerableEntity()
            {
                return new InquiryFaker().Generate(10, 1000);
            }
        }

        [Theory]
        [ClassData(typeof(FakeInquiryDtoData))]
        public async Task CreateInquiryReturnInquiry(InquiryDto inquiry)
        {
            // Arrange
            var client = _factory.CreateClient();
            var inquiryDataStore = _factory.Services.GetService<EntityDataStore<Inquiry>>();

            // Act
            var response = await client.PostAsJsonAsync("api/inquiry", inquiry).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(inquiryDataStore.IsSet);
            var actualInquiry = await response.Content.ReadFromJsonAsync<InquiryDto>().ConfigureAwait(false);

            // Assert
            Assert.NotEqual(inquiry.Id, actualInquiry.Id); // TODO: By formal definition it is possile this fails due to bad luck
            Assert.Equal(inquiry.DocumentName, actualInquiry.DocumentName);
            Assert.Equal(inquiry.Inspection, actualInquiry.Inspection);
            Assert.Equal(inquiry.JointMeasurement, actualInquiry.JointMeasurement);
            Assert.Equal(inquiry.FloorMeasurement, actualInquiry.FloorMeasurement);
            Assert.Equal(inquiry.Note, actualInquiry.Note);
            Assert.Equal(inquiry.DocumentDate, actualInquiry.DocumentDate);
            Assert.Equal(inquiry.DocumentFile, actualInquiry.DocumentFile);
            Assert.Equal(AuditStatus.Todo, actualInquiry.AuditStatus);
            Assert.Equal(inquiry.Type, actualInquiry.Type);
            Assert.Equal(inquiry.StandardF3o, actualInquiry.StandardF3o);
            //Assert.Equal(inquiry.Attribution, actualInquiry.Attribution);
            Assert.Equal(inquiry.AccessPolicy, actualInquiry.AccessPolicy);
        }

        [Theory]
        [ClassData(typeof(FakeInquiryData))]
        public async Task GetInquiryByIdReturnSingleInquiry(Inquiry inquiry)
        {
            // Arrange
            var client = _factory
                .WithDataStoreList(inquiry)
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualInquiry = await response.Content.ReadFromJsonAsync<InquiryDto>().ConfigureAwait(false);

            // Assert
            Assert.Equal(inquiry.Id, actualInquiry.Id); // TODO: By formal definition it is possile this fails due to bad luck
            Assert.Equal(inquiry.DocumentName, actualInquiry.DocumentName);
            Assert.Equal(inquiry.Inspection, actualInquiry.Inspection);
            Assert.Equal(inquiry.JointMeasurement, actualInquiry.JointMeasurement);
            Assert.Equal(inquiry.FloorMeasurement, actualInquiry.FloorMeasurement);
            Assert.Equal(inquiry.Note, actualInquiry.Note);
            Assert.Equal(inquiry.DocumentDate, actualInquiry.DocumentDate);
            Assert.Equal(inquiry.DocumentFile, actualInquiry.DocumentFile);
            Assert.Equal(inquiry.AuditStatus, actualInquiry.AuditStatus);
            Assert.Equal(inquiry.Type, actualInquiry.Type);
            Assert.Equal(inquiry.StandardF3o, actualInquiry.StandardF3o);
            //Assert.Equal(inquiry.Attribution, actualInquiry.Attribution);
            Assert.Equal(inquiry.AccessPolicy, actualInquiry.AccessPolicy);
        }

        [Fact]
        public async Task GetAllInquiryReturnPageInquiry()
        {
            // Arrange
            var client = _factory
                .WithDataStoreList(new InquiryFaker().Generate(10, 100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var inquiryList = await response.Content.ReadFromJsonAsync<List<InquiryDto>>().ConfigureAwait(false);
            Assert.NotNull(inquiryList);

            // Assert
            Assert.True(inquiryList.Count > 0);
        }

        [Fact]
        public async Task GetAllInquiryReturnAllInquiry()
        {
            // Arrange
            var client = _factory
                .WithDataStoreList(new InquiryFaker().Generate(100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry?limit=100").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var inquiryList = await response.Content.ReadFromJsonAsync<List<InquiryDto>>().ConfigureAwait(false);
            Assert.NotNull(inquiryList);

            // Assert
            Assert.Equal(100, inquiryList.Count);
        }

        [Theory]
        [ClassData(typeof(FakeInquiryData))]
        public async Task UpdateInquiryReturnNoContent(Inquiry inquiry)
        {
            // Arrange
            var newInquiry = new InquiryFaker().Generate();
            var client = _factory
                .WithDataStoreList(inquiry)
                .CreateClient();
            var inquiryDataStore = _factory.Services.GetService<EntityDataStore<Inquiry>>();

            // Act
            var response = await client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}", newInquiry).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(1, inquiryDataStore.Entities.Count);

            // Assert
            var actualInquiry = inquiryDataStore.Entities[0];
            Assert.Equal(inquiry.Id, actualInquiry.Id);
            Assert.Equal(newInquiry.DocumentName, actualInquiry.DocumentName);
            Assert.Equal(newInquiry.Inspection, actualInquiry.Inspection);
            Assert.Equal(newInquiry.JointMeasurement, actualInquiry.JointMeasurement);
            Assert.Equal(newInquiry.FloorMeasurement, actualInquiry.FloorMeasurement);
            Assert.Equal(newInquiry.Note, actualInquiry.Note);
            Assert.Equal(newInquiry.DocumentDate, actualInquiry.DocumentDate);
            Assert.Equal(newInquiry.DocumentFile, actualInquiry.DocumentFile);
            Assert.Equal(newInquiry.Type, actualInquiry.Type);
            Assert.Equal(newInquiry.StandardF3o, actualInquiry.StandardF3o);
            Assert.Equal(inquiry.AuditStatus, actualInquiry.AuditStatus);
        }

        [Theory]
        [InlineData(AuditStatus.Pending, AuditStatus.PendingReview, "status_review")]
        [InlineData(AuditStatus.PendingReview, AuditStatus.Rejected, "status_rejected")]
        [InlineData(AuditStatus.PendingReview, AuditStatus.Done, "status_approved")]
        public async Task SetStatusInquiryReturnNoContent(AuditStatus initialStatus, AuditStatus status, string url)
        {
            // Arrange
            var inquiry = new InquiryFaker()
                .RuleFor(f => f.AuditStatus, f => initialStatus)
                .Generate();
            var client = _factory
                .WithDataStoreList(inquiry)
                .CreateClient();
            var inquiryDataStore = _factory.Services.GetService<EntityDataStore<Inquiry>>();

            // Act
            var response = await client.PutAsync($"api/inquiry/{inquiry.Id}/{url}", null).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Assert
            Assert.Equal(status, inquiryDataStore.Entities[0].AuditStatus);
        }

        [Theory]
        [ClassData(typeof(FakeInquiryData))]
        public async Task DeleteInquiryReturnNoContent(Inquiry inquiry)
        {
            // Arrange
            var client = _factory
                .WithDataStoreList(inquiry)
                .CreateClient();
            var inquiryDataStore = _factory.Services.GetService<EntityDataStore<Inquiry>>();

            // Act
            var response = await client.DeleteAsync($"api/inquiry/{inquiry.Id}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Assert
            Assert.False(inquiryDataStore.IsSet);
        }
    }
}
