using Bogus;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.InputModels;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Backend;
using FunderMaps.Testing.Extensions;
using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice
{
    /// <summary>
    ///     Tests our authentication.
    /// </summary>
    public class AuthTests : IClassFixture<AuthWebserviceWebApplicationFactory>
    {
        private readonly AuthWebserviceWebApplicationFactory _factory;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthTests(AuthWebserviceWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task SignInReturnSuccessAndToken()
        {
            // Arrange
            var client1 = new AuthBackendWebApplicationFactory()
                .ConfigureAuthentication(options => options.User.Role = ApplicationRole.Administrator)
                .WithAuthenticationStores()
                .CreateClient();
            var organizationProp = await client1.PostAsJsonGetFromJsonAsync<OrganizationProposalDto, OrganizationProposalDto>("api/organization/proposal", new OrganizationProposalDtoFaker().Generate());
            var client2 = new BackendWebApplicationFactory()
                .CreateClient();
            var organizationSetup = new OrganizationSetupDtoFaker().Generate();
            var organization = await client2.PostAsJsonAsync($"api/organization/{organizationProp.Id}/setup", organizationSetup);
            var client3 = _factory
                .CreateClient();
            var signIn = new SignInInputModel
            {
                Email = organizationSetup.Email,
                Password = organizationSetup.Password,
            };

            // Act
            var response = await client3.PostAsJsonAsync("api/auth/signin", signIn);
            var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Id);
            Assert.NotNull(returnObject.Token);
            Assert.True(returnObject.ValidTo > returnObject.ValidFrom);
        }

        [Fact]
        public async Task RefreshSignInReturnSuccessAndToken()
        {
            // Arrange
            var client = _factory
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/auth/token-refresh");
            var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Id);
            Assert.NotNull(returnObject.Token);
            Assert.True(returnObject.ValidTo > returnObject.ValidFrom);
        }

        [Fact]
        public async Task SignInInvalidCredentialsReturnError()
        {
            // Arrange
            var client1 = new AuthBackendWebApplicationFactory()
                .ConfigureAuthentication(options => options.User.Role = ApplicationRole.Administrator)
                .WithAuthenticationStores()
                .CreateClient();
            var organizationProp = await client1.PostAsJsonGetFromJsonAsync<OrganizationProposalDto, OrganizationProposalDto>("api/organization/proposal", new OrganizationProposalDtoFaker().Generate());
            var client2 = new BackendWebApplicationFactory()
                .CreateClient();
            var organizationSetup = new OrganizationSetupDtoFaker().Generate();
            var organization = await client2.PostAsJsonAsync($"api/organization/{organizationProp.Id}/setup", organizationSetup);
            var client3 = _factory
                .CreateClient();
            var signIn = new SignInInputModel
            {
                Email = organizationSetup.Email,
                Password = new Randomizer().Password(64),
            };

            // Act
            var response = await client3.PostAsJsonAsync("api/auth/signin", signIn);
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal((short)HttpStatusCode.Unauthorized, returnObject.Status);
            Assert.Contains("Login", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
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
