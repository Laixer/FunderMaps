using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using System.Net.Http;
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

        public IncidentPortalTests(BackendWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
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
    }
}
