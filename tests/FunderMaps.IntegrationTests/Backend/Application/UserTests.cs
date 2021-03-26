﻿using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.IntegrationTests.Faker;
using System;
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
            Assert.Equal(Guid.Parse("1a93cfb3-f097-4697-a998-71cdd9cfaead"), returnObject.Id);
            Assert.Equal("lester@contoso.com", returnObject.Email);
        }

        [Fact]
        public async Task UpdateUserFromSessionReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateClient();
            var updateObject = new UserDtoFaker().Generate();

            // Act
            var response = await client.PutAsJsonAsync("api/user", updateObject);

            // Act
            var returnObject = await client.GetFromJsonAsync<UserDto>("api/user");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(updateObject.GivenName, returnObject.GivenName);
            Assert.Equal(updateObject.LastName, returnObject.LastName);
            Assert.Equal(updateObject.JobTitle, returnObject.JobTitle);
            Assert.Equal(updateObject.PhoneNumber, returnObject.PhoneNumber);
        }

        [Fact]
        public async Task ChangePasswordFromSessionReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateClient();
            var newObject = new ChangePasswordDtoFaker()
                .RuleFor(f => f.OldPassword, f => "fundermaps")
                .Generate();

            // Act
            var response = await client.PostAsJsonAsync("user/change-password", newObject);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            await TestStub.LoginAsync(Factory, "lester@contoso.com", newObject.NewPassword);

            response = await client.PostAsJsonAsync("user/change-password", new ChangePasswordDto()
            {
                OldPassword = newObject.NewPassword,
                NewPassword = "fundermaps",
            });

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            await TestStub.LoginAsync(Factory, "lester@contoso.com", "fundermaps");
        }
    }
}
