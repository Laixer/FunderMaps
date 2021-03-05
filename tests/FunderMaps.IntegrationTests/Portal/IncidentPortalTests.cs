using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using FunderMaps.AspNetCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Portal
{
    /// <summary>
    ///     Anonymous tests for the incident create endpoint.
    /// </summary>
    public class IncidentPortalTests : IClassFixture<PortalWebApplicationFactory>
    {
        private readonly PortalWebApplicationFactory _factory;
        private readonly HttpClient _client;

        // FUTURE: Move into common test code
        internal static readonly IList<Address> Addresses = new List<Address>
        {
            new Address
            {
                Id = "gfm-0000a5dce4114767a57d8facaddf53d6",
                BuildingNumber = "102 C ",
                PostalCode = "1017PP",
                Street = "Leidsekade",
                IsActive = true,
                ExternalId = "NL.IMBAG.NUMMERAANDUIDING.0363200012151882",
                ExternalSource = ExternalDataSource.NlBag,
                City = "Delft",
                BuildingId = "gfm-47bfe7a54b2344d7a44da341027b89a5",
            },
            new Address
            {
                Id = "gfm-0a687b09a74a43b1b60b8a649a4d2c1b",
                BuildingNumber = "9",
                PostalCode = "5707KM",
                Street = "Mr. Rietkerklaan",
                IsActive = true,
                ExternalId = "NL.IMBAG.NUMMERAANDUIDING.0794200000077355",
                ExternalSource = ExternalDataSource.NlBag,
                City = "Delft",
                BuildingId = "gfm-47bfe7a54b2344d7a44da341027b89a5",
            },
            new Address
            {
                Id = "gfm-67f8fd79bf3a461c923e7c24c0fb479f",
                BuildingNumber = "725",
                PostalCode = "8446DR",
                Street = "Heerenhage",
                IsActive = false,
                ExternalId = "NL.IMBAG.NUMMERAANDUIDING.0074200000377867",
                ExternalSource = ExternalDataSource.NlBag,
                City = "Delft",
                BuildingId = "gfm-47bfe7a54b2344d7a44da341027b89a5",
            },
            new Address
            {
                Id = "gfm-67f8fe46717947d8bcd22bee5ca092ae",
                BuildingNumber = "116",
                PostalCode = "4621EV",
                Street = "Beukstraat",
                IsActive = true,
                ExternalId = "NL.IMBAG.NUMMERAANDUIDING.0748200000039597",
                ExternalSource = ExternalDataSource.NlBag,
                City = "Delft",
                BuildingId = "gfm-47bfe7a54b2344d7a44da341027b89a5",
            },
            new Address
            {
                Id = "gfm-67f9b843843846a89c5700f048dfe5a0",
                BuildingNumber = "8 A",
                PostalCode = "1016PE",
                Street = "Tweede Rozendwarsstraat",
                IsActive = true,
                ExternalId = "NL.IMBAG.NUMMERAANDUIDING.0363200012071002",
                ExternalSource = ExternalDataSource.NlBag,
                City = "Delft",
                BuildingId = "gfm-47bfe7a54b2344d7a44da341027b89a5",
            },
            new Address
            {
                Id = "gfm-67fcde96c989437f95dd4a903681661e",
                BuildingNumber = "3",
                PostalCode = "1073XN",
                Street = "Tweede Jacob van Campenstraat",
                IsActive = true,
                ExternalId = "NL.IMBAG.NUMMERAANDUIDING.0363200000423238",
                ExternalSource = ExternalDataSource.NlBag,
                City = "Delft",
                BuildingId = "gfm-47bfe7a54b2344d7a44da341027b89a5",
            },
            new Address
            {
                Id = "gfm-67fce470f8e343bba28b443faf5495cb",
                BuildingNumber = "34 A 02",
                PostalCode = "6227BZ",
                Street = "Einsteinstraat",
                IsActive = true,
                ExternalId = "NL.IMBAG.NUMMERAANDUIDING.0935200000097589",
                ExternalSource = ExternalDataSource.NlBag,
                City = "Delft",
                BuildingId = "gfm-47bfe7a54b2344d7a44da341027b89a5",
            },
            new Address
            {
                Id = "gfm-67fcf92aede845258d80acec7df12f14",
                BuildingNumber = "279 C ",
                PostalCode = "3028AD",
                Street = "Franselaan",
                IsActive = true,
                ExternalId = "NL.IMBAG.NUMMERAANDUIDING.0599200000270883",
                ExternalSource = ExternalDataSource.NlBag,
                City = "Delft",
                BuildingId = "gfm-47bfe7a54b2344d7a44da341027b89a5",
            },
            new Address
            {
                Id = "gfm-67fd1215c9224ef1b6eb2af0b75b9ccd",
                BuildingNumber = "15  ",
                PostalCode = "2271TT",
                Street = "van Deventerlaan",
                IsActive = true,
                ExternalId = "NL.IMBAG.NUMMERAANDUIDING.1916200000035516",
                ExternalSource = ExternalDataSource.NlBag,
                City = "Delft",
                BuildingId = "gfm-47bfe7a54b2344d7a44da341027b89a5",
            },
        };

        internal static readonly IList<Building> Buildings = new List<Building>
        {
            new Building
            {
                Id = "gfm-47bfe7a54b2344d7a44da341027b89a5",
                BuiltYear = System.DateTime.Now,
                IsActive = true,
                Geometry = "7b2274797065223a224d756c7469506f6c79676f6e222c22636f6f7264696e61746573223a5b5b5b5b352e3833333038343731312c35332e3039333332373638385d2c5b352e3833333033393531392c35332e3039333331343130385d2c5b352e3833333031393439362c35332e30393333333832325d2c5b352e3833323938353930392c35332e3039333332383132385d2c5b352e3833333036303236382c35332e3039333233383438355d2c5b352e38333331333930362c35332e3039333236323136365d2c5b352e3833333038343731312c35332e3039333332373638385d5d5d5d7d",
                ExternalId = "NL.IMBAG.PAND.1916200000035516",
                ExternalSource = ExternalDataSource.NlBag,
                BuildingType = BuildingType.House,
                NeighborhoodId  = "gfm-20e90c3481f346d783159332e882afb3",
            },
        };

        public IncidentPortalTests(PortalWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory
                .WithDataStoreList(Addresses)
                .WithDataStoreList(Buildings)
                .CreateClient();
        }

        // [Fact]
        // public async Task CreateIncidentReturnOk()
        // {
        //     // Arrange.
        //     var incident = new IncidentDtoFaker().Generate();

        //     // Act.
        //     var response = await _client.PostAsJsonAsync("api/incident-portal/submit", incident);

        //     // Assert.
        //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        // }

        // [Theory]
        // [InlineData("4621EV")]
        // [InlineData("2271TT")]
        // public async Task GetAllAddressByQueryReturnMatchingAddressList(string query)
        // {
        //     // Act
        //     var response = await _client.GetAsync($"api/incident-portal/address-suggest?query={query}");
        //     var returnList = await response.Content.ReadFromJsonAsync<List<Address>>();

        //     // Assert
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //     Assert.True(returnList.Count > 0);
        // }

        [Fact]
        public async Task UploadDocumentReturnDocument()
        {
            // Arrange
            using var formContent = new FileUploadContent(mediaType: "application/pdf", fileExtension: "pdf");

            // Act
            var response = await _client.PostAsync("api/incident-portal/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<DocumentDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Name);
        }

        // FUTURE: FIX: FIXME: XXX
        [Fact(Skip = "Change testcase to also allow on external address id")]
        public async Task GetRiskAnalysisReturnAnalysis()
        {
            // Arrange
            var analysisProducts = new AnalysisProductFaker().Generate();
            var client = _factory
                .WithDataStoreItem(analysisProducts)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/incident-portal/risk?id={analysisProducts.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<AnalysisRiskPlusDto>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.NeighborhoodId);
        }

        // [Theory]
        // [InlineData(null, 0, null)]
        // [InlineData("gfm-asdkkgfsljshdf", 0, null)]
        // [InlineData(null, 22, null)]
        // [InlineData(null, 0, "just@email.nl")]
        // [InlineData(null, 19, "email@email.com")]
        // [InlineData("gfm-kdshfjdsfkljdhsf", 0, "valid@valid.nl")]
        // [InlineData("aaa-invalidid", 84, "perfect@mail.com")]
        // [InlineData("gfm-correctid", 1492, "yesyes@disgood.nl")]
        // [InlineData("gfm-correctid", -1000, "actually@right.nl")]
        // [InlineData("gfm-lsdhfdsfajh", 14390, "invalidemail")]
        // public async Task CreateInvalidIncidentReturnBadRequest(string address, int clientId, string email)
        // {
        //     // Arrange.
        //     var incident = new IncidentDtoFaker()
        //         .RuleFor(f => f.Address, f => address)
        //         .RuleFor(f => f.ClientId, f => clientId)
        //         .RuleFor(f => f.Email, f => email)
        //         .Generate();

        //     // Act.
        //     var response = await _client.PostAsJsonAsync("api/incident-portal/submit", incident);

        //     // Assert.
        //     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        // }

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

        // [Theory]
        // [MemberData(nameof(RegressionCreateInvalidEnumReturnBadRequestData))]
        // public async Task RegressionCreateInvalidEnumReturnBadRequest(IncidentDto incident)
        // {
        //     // Act.
        //     var response = await _client.PostAsJsonAsync("api/incident-portal/submit", incident);
        //     var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

        //     // Assert.
        //     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //     Assert.Equal((short)HttpStatusCode.BadRequest, returnObject.Status);
        //     Assert.Contains("validation", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
        // }

        // [Fact]
        // public async Task RegressionCreateInvalidPhoneReturnBadRequest()
        // {
        //     // Arrange.
        //     var incident = new IncidentDtoFaker()
        //             .RuleFor(f => f.PhoneNumber, f => "12345678901234567")
        //             .Generate();

        //     // Act.
        //     var response = await _client.PostAsJsonAsync("api/incident-portal/submit", incident);
        //     var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

        //     // Assert.
        //     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //     Assert.Equal((short)HttpStatusCode.BadRequest, returnObject.Status);
        //     Assert.Contains("validation", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
        // }

        [Fact]
        public async Task CreateEmptyBodyReturnBadRequest()
        {
            // Act.
            var response = await _client.PostAsJsonAsync<IncidentDto>("api/incident-portal/submit", null);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UploadEmptyFormReturnBadRequest()
        {
            // Arrange
            using var formContent = new MultipartFormDataContent();

            // Act
            var response = await _client.PostAsync("api/incident-portal/upload-document", formContent);
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

            // Act
            var response = await _client.PostAsync("api/incident-portal/upload-document", formContent);
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

            // Act
            var response = await _client.PostAsync("api/incident-portal/upload-document", formContent);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal((short)HttpStatusCode.BadRequest, returnObject.Status);
            Assert.Contains("validation", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
