using FunderMaps.WebApi.InputModels;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report
{
    /// <summary>
    ///     Anonymous tests for the incident create endpoint.
    /// </summary>
    public class IncidentSubmitTests : IClassFixture<BackendWebApplicationFactory>
    {
        private readonly BackendWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public IncidentSubmitTests(BackendWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory
                .CreateClient();
        }

        [Fact]
        public async Task SubmitIncidentReturnNoContent()
        {
            // Arrange.
            var incident = new IncidentInputModel
            {
                Address = "gfm-dsfdlshfjsdhfljdshf",
                ClientId = 14,
                Email = "yeet@yoodle.com"
            };

            // Act.
            var response = await _client.PostAsJsonAsync("api/incident/submit", incident);
            var responseObject = await response.Content.ReadAsStringAsync(); // TODO Remove

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
        public async Task SubmitInvalidIncidentReturnBadRequest(string address, int clientId, string email)
        {
            // Arrange.
            var incident = new IncidentInputModel
            {
                Address = address,
                ClientId = clientId,
                Email = email
            };

            // Act.
            var response = await _client.PostAsJsonAsync("api/incident/submit", incident);
            var responseObject = await response.Content.ReadAsStringAsync(); // TODO Remove

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SubmitNothingReturnBadRequest()
        {
            // Act.
            var response = await _client.PostAsJsonAsync<IncidentInputModel>("api/incident/submit", null);
            var responseObject = await response.Content.ReadAsStringAsync(); // TODO Remove

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
