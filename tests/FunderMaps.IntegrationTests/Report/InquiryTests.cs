using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
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
                return new InquiryDtoFaker().Generate(100);
            }
        }

        internal class FakeInquiryData : EnumerableHelper<Inquiry>
        {
            protected override IEnumerable<Inquiry> GetEnumerableEntity()
            {
                return new InquiryFaker().Generate(100);
            }
        }

        [Theory]
        [ClassData(typeof(FakeInquiryDtoData))]
        public async Task CreateIncidentReturnIncident(InquiryDto inquiry)
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
    }
}
