using Bogus;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.InputModels;
using FunderMaps.Testing.Extensions;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class AuthTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task SignInReturnSuccessAndToken()
        {
            await TestStub.LoginAsync(Factory, "Javier40@yahoo.com", "fundermaps");
            await TestStub.LoginAsync(Factory, "Freda@contoso.com", "fundermaps");
            await TestStub.LoginAsync(Factory, "patsy@contoso.com", "fundermaps");
            await TestStub.LoginAsync(Factory, "lester@contoso.com", "fundermaps");
        }

        [Fact]
        public async Task RefreshSignInReturnSuccessAndToken()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/auth/token-refresh");
            var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Id);
            Assert.NotNull(returnObject.Token);
            Assert.NotNull(returnObject.Issuer);
            Assert.True(returnObject.ValidTo > returnObject.ValidFrom);
        }

        [Fact]
        public async Task SignInInvalidCredentialsReturnError()
        {
            // Arrange
            using var client = Factory.CreateUnauthorizedClient();

            // Act
            var response = await client.PostAsJsonAsync("api/auth/signin", new SignInInputModel()
            {
                Email = "lester@contoso.com",
                Password = new Randomizer().Password(64),
            });
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal((short)HttpStatusCode.Unauthorized, returnObject.Status);
            Assert.Contains("Login", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData("", "")]
        public async Task SignInInvalidRequestReturnBadRequest(string email, string password)
        {
            // Arrange
            using var client = Factory.CreateUnauthorizedClient();

            // Act
            var response = await client.PostAsJsonAsync("api/auth/signin", new SignInInputModel()
            {
                Email = email,
                Password = password,
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [ClassData(typeof(RandomStringGeneratorP2))]
        public async Task SignInRandomDataReturnNot500(string email, string password)
        {
            // Arrange
            using var client = Factory.CreateUnauthorizedClient();

            // Act
            var response = await client.PostAsJsonAsync("api/auth/signin", new SignInInputModel()
            {
                Email = email,
                Password = password,
            });

            // Assert
            Assert.NotEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Theory]
        [InlineData("api/user")]
        [InlineData("api/auth/token-refresh")]
        public async Task RefreshSignInReturnUnauthorized(string uri)
        {
            // Arrange
            using var client = Factory.CreateUnauthorizedClient();

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
