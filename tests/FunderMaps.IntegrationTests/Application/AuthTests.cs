using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Services;
using FunderMaps.IntegrationTests.Extensions;
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
    public class AuthTests : IClassFixture<AuthWebApplicationFactory<Startup>>
    {
        private readonly AuthWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        private readonly User sessionUser = new UserFaker().Generate();
        private readonly string password = new Randomizer().Password(128);

        public AuthTests(AuthWebApplicationFactory<Startup> factory)
        {
            var passwordHash = new PasswordHasher().HashPassword(password);
            _factory = factory;
            _client = _factory
                .WithAuthOptions(sessionUser)
                .WithDataStoreList(new UserRecord { User = sessionUser, Password = passwordHash })
                .CreateClient();
        }

        [Fact]
        public async Task SignInReturnSuccessAndToken()
        {
            // Arrange
            var signIn = new SignInDto
            {
                Email = sessionUser.Email,
                Password = password,
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/auth/signin", signIn);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var securityToken = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>().ConfigureAwait(false);
            Assert.NotNull(securityToken.Token);
            Assert.True(securityToken.TokenValidity > 0);
        }
    }
}
