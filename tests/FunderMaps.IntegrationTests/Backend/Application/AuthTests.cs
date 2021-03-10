﻿using Bogus;
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
        public async Task SignInSuperuserReturnSuccessAndToken()
        {
            // Arrange
            using var client = Factory.CreateUnauthorizedClient();

            // Act
            var response = await client.PostAsJsonAsync("api/auth/signin", new SignInInputModel()
            {
                Email = Factory.Superuser.User.Email,
                Password = Factory.Superuser.Password,
            });
            var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Id);
            Assert.NotNull(returnObject.Token);
            Assert.True(returnObject.ValidTo > returnObject.ValidFrom);
        }

        [Fact]
        public async Task SignInVerifierReturnSuccessAndToken()
        {
            // Arrange
            using var client = Factory.CreateUnauthorizedClient();

            // Act
            var response = await client.PostAsJsonAsync("api/auth/signin", new SignInInputModel()
            {
                Email = Factory.Verifier.User.Email,
                Password = Factory.Verifier.Password,
            });
            var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Id);
            Assert.NotNull(returnObject.Token);
            Assert.True(returnObject.ValidTo > returnObject.ValidFrom);
        }

        [Fact]
        public async Task SignInWriterReturnSuccessAndToken()
        {
            // Arrange
            using var client = Factory.CreateUnauthorizedClient();

            // Act
            var response = await client.PostAsJsonAsync("api/auth/signin", new SignInInputModel()
            {
                Email = Factory.Writer.User.Email,
                Password = Factory.Writer.Password,
            });
            var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Id);
            Assert.NotNull(returnObject.Token);
            Assert.True(returnObject.ValidTo > returnObject.ValidFrom);
        }

        [Fact]
        public async Task SignInReaderReturnSuccessAndToken()
        {
            // Arrange
            using var client = Factory.CreateUnauthorizedClient();

            // Act
            var response = await client.PostAsJsonAsync("api/auth/signin", new SignInInputModel()
            {
                Email = Factory.Reader.User.Email,
                Password = Factory.Reader.Password,
            });
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
            using var client = Factory.CreateClient();

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
            using var client = Factory.CreateUnauthorizedClient();

            // Act
            var response = await client.PostAsJsonAsync("api/auth/signin", new SignInInputModel()
            {
                Email = Factory.Superuser.User.Email,
                Password = new Randomizer().Password(64),
            });
            var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal((short)HttpStatusCode.Unauthorized, returnObject.Status);
            Assert.Contains("Login", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
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
