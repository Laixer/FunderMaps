using FunderMaps.Core.Entities;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.IntegrationTests.Repositories;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Application
{
    public class UserTests : IClassFixture<AuthWebApplicationFactory<Startup>>
    {
        private readonly AuthWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        private readonly User sessionUser = new UserFaker().Generate();

        public UserTests(AuthWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory
                .WithAuthOptions(sessionUser)
                .WithDataStoreList(new UserRecord { User = sessionUser })
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
