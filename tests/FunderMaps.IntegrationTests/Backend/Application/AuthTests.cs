using Bogus;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.InputModels;
using FunderMaps.Core.Services;
using FunderMaps.Testing.Extensions;
using FunderMaps.Testing.Faker;
using FunderMaps.Testing.Repositories;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class AuthTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public AuthTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SignInReturnSuccessAndToken()
        {
            // Arrange
            var sessionUser = new UserFaker().Generate();
            var sessionOrganization = new OrganizationFaker().Generate();
            var password = new Randomizer().Password(128);
            var signIn = new SignInInputModel
            {
                Email = sessionUser.Email,
                Password = password,
            };
            using var random = new RandomGenerator();
            var client = _factory
                .WithAuthentication(options => options.User = sessionUser)
                .WithDataStoreList(new UserRecord { User = sessionUser, Password = new PasswordHasher(random).HashPassword(password) })
                .WithDataStoreList(sessionOrganization)
                .WithDataStoreList(new OrganizationUserRecord { UserId = sessionUser.Id, OrganizationId = sessionOrganization.Id })
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/auth/signin", signIn);
            var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Token);
            Assert.True(returnObject.TokenValidity > 0);
        }

        [Fact]
        public async Task RefreshSignInReturnSuccessAndToken()
        {
            // Arrange
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/auth/token-refresh");
            var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Token);
            Assert.True(returnObject.TokenValidity > 0);
        }

        [Theory]
        [InlineData("/")]
        [InlineData("api/user")]
        [InlineData("api/auth/token-refresh")]
        public async Task RefreshSignInReturnUnauthorized(string uri)
        {
            // Arrange
            using var factory = new BackendWebApplicationFactory();
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
