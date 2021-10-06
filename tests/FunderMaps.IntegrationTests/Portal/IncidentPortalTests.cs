using FunderMaps.Core.Types;
using FunderMaps.AspNetCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using FunderMaps.IntegrationTests.Faker;

namespace FunderMaps.IntegrationTests.Portal
{
    /// <summary>
    ///     Anonymous tests for the incident create endpoint.
    /// </summary>
    public class IncidentPortalTests : IClassFixture<PortalWebApplicationFactory>
    {
        private PortalWebApplicationFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentPortalTests(PortalWebApplicationFactory factory)
            => Factory = factory;

        [Fact]
        public async Task UploadDocumentReturnDocument()
        {
            // Arrange
            using var formContent = new FileUploadContent(mediaType: "application/pdf", fileExtension: "pdf");
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsync("api/incident-portal/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<DocumentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Name);
        }

        [Fact]
        public async Task UploadEmptyFormReturnBadRequest()
        {
            // Arrange
            using var formContent = new MultipartFormDataContent();
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsync("api/incident-portal/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsync("api/incident-portal/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsync("api/incident-portal/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal((short)HttpStatusCode.BadRequest, returnObject.Status);
            Assert.Contains("validation", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public async Task CreateIncidentReturnOk()
        {
            // Arrange
            var incident = new IncidentDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .Generate();
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/incident-portal/submit", incident);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("gfm-asdkkgfsljshdf")]
        [InlineData("aaa-invalidid")]
        [InlineData("gfm-correctid")]
        public async Task CreateIncidentReturnNotFound(string address)
        {
            // Arrange.
            var incident = new IncidentDtoFaker()
                .RuleFor(f => f.Address, f => address)
                .Generate();
            using var client = Factory.CreateClient();

            // Act.
            var response = await client.PostAsJsonAsync("api/incident-portal/submit", incident);

            // Assert.
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalidemail")]
        public async Task CreateInvalidEmailReturnBadRequest(string email)
        {
            // Arrange
            var incident = new IncidentDtoFaker()
                .RuleFor(f => f.Email, f => email)
                .Generate();
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/incident-portal/submit", incident);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateInvalidAddressReturnBadRequest()
        {
            // Arrange
            var incident = new IncidentDtoFaker()
                .RuleFor(f => f.Address, f => null)
                .Generate();
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/incident-portal/submit", incident);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        public static List<object[]> RegressionCreateInvalidEnumReturnBadRequestData
            => new()
            {
                new object[]
                {
                    new IncidentDtoFaker()
                        .RuleFor(f => f.FoundationType, f => (FoundationType)10000)
                        .Generate()
                },
                new object[]
                {
                    new IncidentDtoFaker()
                        .RuleFor(f => f.FoundationDamageCause, f => (FoundationDamageCause)10000)
                        .Generate()
                },
                new object[]
                {
                    new IncidentDtoFaker()
                        .RuleFor(f => f.QuestionType, f => (IncidentQuestionType)10000)
                        .Generate()
                },
                new object[]
                {
                    new IncidentDtoFaker()
                        .RuleFor(f => f.AuditStatus, f => (AuditStatus)10000)
                        .Generate()
                },
                new object[]
                {
                    new IncidentDtoFaker()
                        .RuleFor(f => f.FoundationDamageCharacteristics, f => new FoundationDamageCharacteristics[]
                        {
                            FoundationDamageCharacteristics.Crack,
                            (FoundationDamageCharacteristics)10000
                        })
                        .Generate()
                },
                new object[]
                {
                    new IncidentDtoFaker()
                        .RuleFor(f => f.EnvironmentDamageCharacteristics, f => new EnvironmentDamageCharacteristics[]
                        {
                            (EnvironmentDamageCharacteristics)10000
                        })
                        .Generate()
                },
            };

        [Theory]
        [MemberData(nameof(RegressionCreateInvalidEnumReturnBadRequestData))]
        public async Task CreateInvalidEnumReturnBadRequest(IncidentDto incident)
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/incident-portal/submit", incident);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal((short)HttpStatusCode.BadRequest, returnObject.Status);
            Assert.Contains("validation", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public async Task CreateInvalidPhoneReturnBadRequest()
        {
            // Arrange
            var incident = new IncidentDtoFaker()
                    .RuleFor(f => f.PhoneNumber, f => "12345678901234567")
                    .Generate();
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/incident-portal/submit", incident);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal((short)HttpStatusCode.BadRequest, returnObject.Status);
            Assert.Contains("validation", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public async Task CreateEmptyBodyReturnBadRequest()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync<IncidentDto>("api/incident-portal/submit", null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
