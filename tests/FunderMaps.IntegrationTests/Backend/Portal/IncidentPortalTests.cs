using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Portal
{
    /// <summary>
    ///     Anonymous tests for the incident create endpoint.
    /// </summary>
    public class IncidentPortalTests : IClassFixture<BackendWebApplicationFactory>
    {
        private readonly BackendWebApplicationFactory _factory;
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
            },
        };

        public IncidentPortalTests(BackendWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory
                .WithDataStoreList(Addresses)
                .CreateClient();
        }

        [Fact]
        public async Task CreateIncidentReturnOk()
        {
            // Arrange.
            var incident = new IncidentDtoFaker().Generate();

            // Act.
            var response = await _client.PostAsJsonAsync("api/incident-portal/submit", incident);

            // Assert.
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("4621EV")]
        [InlineData("2271TT")]
        public async Task GetAllAddressByQueryReturnMatchingAddressList(string query)
        {
            // Act
            var response = await _client.GetAsync($"api/incident-portal/address-suggest?query={query}");
            var returnList = await response.Content.ReadFromJsonAsync<List<Address>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count > 0);
        }

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

        [Theory]
        [InlineData(null, 0, null)]
        [InlineData("gfm-asdkkgfsljshdf", 0, null)]
        [InlineData(null, 22, null)]
        [InlineData(null, 0, "just@email.nl")]
        [InlineData(null, 19, "email@email.com")]
        [InlineData("gfm-kdshfjdsfkljdhsf", 0, "valid@valid.nl")]
        [InlineData("aaa-invalidid", 84, "perfect@mail.com")]
        [InlineData("gfm-correctid", 1492, "yesyes@disgood.nl")]
        [InlineData("gfm-correctid", -1000, "actually@right.nl")]
        [InlineData("gfm-lsdhfdsfajh", 14390, "invalidemail")]
        public async Task CreateInvalidIncidentReturnBadRequest(string address, int clientId, string email)
        {
            // Arrange.
            var incident = new IncidentDtoFaker()
                .RuleFor(f => f.Address, f => address)
                .RuleFor(f => f.ClientId, f => clientId)
                .RuleFor(f => f.Email, f => email)
                .Generate();
            incident.Address = address;
            incident.ClientId = clientId;
            incident.Email = email;

            // Act.
            var response = await _client.PostAsJsonAsync("api/incident-portal/submit", incident);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateEmptyBodyReturnBadRequest()
        {
            // Act.
            var response = await _client.PostAsJsonAsync<IncidentDto>("api/incident-portal/submit", null);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("0000XX")]
        [InlineData("1111YY")]
        public async Task GetAllAddressByQueryReturnEmptyList(string query)
        {
            // Act
            var response = await _client.GetAsync($"api/incident-portal/address-suggest?query={query}");
            var returnList = await response.Content.ReadFromJsonAsync<List<Address>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Empty(returnList);
        }
    }
}
