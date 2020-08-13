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

namespace FunderMaps.IntegrationTests.Backend.Report
{
    public class InquiryTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public InquiryTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        internal class FakeInquiryDtoData : EnumerableHelper<InquiryDto>
        {
            protected override IEnumerable<InquiryDto> GetEnumerableEntity()
            {
                return new InquiryDtoFaker().Generate(10, 100);
            }
        }

        internal class FakeInquiryData : EnumerableHelper<Inquiry>
        {
            protected override IEnumerable<Inquiry> GetEnumerableEntity()
            {
                // TODO; can have more stats
                return new InquiryFaker()
                    .RuleFor(f => f.AuditStatus, f => AuditStatus.Pending)
                    .Generate(10, 100);
            }
        }

        [Theory]
        [ClassData(typeof(FakeInquiryDtoData))]
        public async Task CreateInquiryReturnInquiry(InquiryDto inquiry)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();
            var inquiryDataStore = _factory.Services.GetService<EntityDataStore<Inquiry>>();

            // Act
            var response = await client.PostAsJsonAsync("api/inquiry", inquiry);
            var returnObject = await response.Content.ReadFromJsonAsync<InquiryDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(inquiryDataStore.IsSet);
            Assert.Equal(inquiry.DocumentName, returnObject.DocumentName);
            Assert.Equal(inquiry.Inspection, returnObject.Inspection);
            Assert.Equal(inquiry.JointMeasurement, returnObject.JointMeasurement);
            Assert.Equal(inquiry.FloorMeasurement, returnObject.FloorMeasurement);
            Assert.Equal(inquiry.Note, returnObject.Note);
            Assert.Equal(inquiry.DocumentDate, returnObject.DocumentDate);
            Assert.Equal(inquiry.DocumentFile, returnObject.DocumentFile);
            Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);
            Assert.Equal(inquiry.Type, returnObject.Type);
            Assert.Equal(inquiry.StandardF3o, returnObject.StandardF3o);
            //Assert.Equal(inquiry.Attribution, actualInquiry.Attribution);
            Assert.Equal(inquiry.AccessPolicy, returnObject.AccessPolicy);
        }

        [Theory]
        [ClassData(typeof(FakeInquiryData))]
        public async Task GetInquiryByIdReturnSingleInquiry(Inquiry inquiry)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(inquiry)
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<InquiryDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(inquiry.Id, returnObject.Id);
            Assert.Equal(inquiry.DocumentName, returnObject.DocumentName);
            Assert.Equal(inquiry.Inspection, returnObject.Inspection);
            Assert.Equal(inquiry.JointMeasurement, returnObject.JointMeasurement);
            Assert.Equal(inquiry.FloorMeasurement, returnObject.FloorMeasurement);
            Assert.Equal(inquiry.Note, returnObject.Note);
            Assert.Equal(inquiry.DocumentDate, returnObject.DocumentDate);
            Assert.Equal(inquiry.DocumentFile, returnObject.DocumentFile);
            Assert.Equal(inquiry.AuditStatus, returnObject.AuditStatus);
            Assert.Equal(inquiry.Type, returnObject.Type);
            Assert.Equal(inquiry.StandardF3o, returnObject.StandardF3o);
            //Assert.Equal(inquiry.Attribution, actualInquiry.Attribution);
            Assert.Equal(inquiry.AccessPolicy, returnObject.AccessPolicy);
        }

        [Fact]
        public async Task GetAllInquiryReturnPageInquiry()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(new InquiryFaker().Generate(10, 100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry");
            var returnList = await response.Content.ReadFromJsonAsync<List<InquiryDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count > 0);
        }

        [Fact]
        public async Task GetAllInquiryReturnAllInquiry()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(new InquiryFaker().Generate(100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry?limit=100");
            var returnList = await response.Content.ReadFromJsonAsync<List<InquiryDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(100, returnList.Count);
        }

        [Theory]
        [ClassData(typeof(FakeInquiryData))]
        public async Task UpdateInquiryReturnNoContent(Inquiry inquiry)
        {
            // Arrange
            var newInquiry = new InquiryFaker().Generate();
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(inquiry)
                .CreateClient();
            var inquiryDataStore = _factory.Services.GetService<EntityDataStore<Inquiry>>();

            // Act
            var response = await client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}", newInquiry);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(1, inquiryDataStore.Entities.Count);
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
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(inquiry)
                .CreateClient();
            var inquiryDataStore = _factory.Services.GetService<EntityDataStore<Inquiry>>();

            // Act
            var response = await client.PutAsync($"api/inquiry/{inquiry.Id}/{url}", null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(status, inquiryDataStore.Entities[0].AuditStatus);
        }

        [Theory]
        [ClassData(typeof(FakeInquiryData))]
        public async Task DeleteInquiryReturnNoContent(Inquiry inquiry)
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(inquiry)
                .CreateClient();
            var inquiryDataStore = _factory.Services.GetService<EntityDataStore<Inquiry>>();

            // Act
            var response = await client.DeleteAsync($"api/inquiry/{inquiry.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.False(inquiryDataStore.IsSet);
        }
    }
}
