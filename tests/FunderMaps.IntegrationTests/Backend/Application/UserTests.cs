using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Testing.Faker;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class UserTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public UserTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task GetUserFromSessionReturnSingleUser()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/user");
            var returnObject = await response.Content.ReadFromJsonAsync<UserDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Factory.Reader.User.Id, returnObject.Id);
        }

        [Fact]
        public async Task UpdateUserFromSessionReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PutAsJsonAsync("api/user", new UserFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
