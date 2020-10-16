using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Testing.Faker;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class UserTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;
        private readonly HttpClient _client;

        private readonly User sessionUser = new UserFaker().Generate();

        public UserTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory
                .ConfigureAuthentication(options => options.User = sessionUser)
                .WithAuthenticationStores()
                .CreateClient();
        }

        [Fact]
        public async Task GetUserFromSessionReturnSingleUser()
        {
            // Act
            var response = await _client.GetAsync("api/user");
            var returnObject = await response.Content.ReadFromJsonAsync<UserDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(sessionUser.Id, returnObject.Id);
        }

        [Fact]
        public async Task UpdateUserFromSessionReturnNoContent()
        {
            // Arrange
            var newUser = new UserFaker().Generate();

            // Act
            var response = await _client.PutAsJsonAsync("api/user", newUser);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
