using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.InputModels;
using Xunit;

namespace FunderMaps.IntegrationTests
{
    /// <summary>
    ///     Teststub for all FunderMaps tests.
    /// </summary>
    public static class TestStub
    {
        public static async Task VersionAsync<TStartup>(FixtureFactory<TStartup> factory)
            where TStartup : class
        {
            // Arrange
            using var client = factory.CreateUnauthorizedClient();

            // Act
            var response = await client.GetAsync("api/version");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("json", response.Content.Headers.ContentType.ToString(), StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("utf-8", response.Content.Headers.ContentType.ToString(), StringComparison.InvariantCultureIgnoreCase);
            Assert.True(response.Headers.CacheControl.Public);
            Assert.NotNull(response.Headers.CacheControl.MaxAge);
        }

        public static async Task<SignInSecurityTokenDto> LoginAsync<TStartup>(FixtureFactory<TStartup> factory, string username, string password)
            where TStartup : class
        {
            // Arrange
            using var client = factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/auth/signin", new SignInInputModel()
            {
                Email = username,
                Password = password,
            });
            var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Id);
            Assert.NotNull(returnObject.Token);
            Assert.NotNull(returnObject.Issuer);
            Assert.True(returnObject.ValidTo > returnObject.ValidFrom);

            return returnObject;
        }
    }
}