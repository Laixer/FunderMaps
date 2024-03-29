﻿using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application;

/// <summary>
///     Create new instance.
/// </summary>
public class OrganizationUserTests(BackendFixtureFactory factory) : IClassFixture<BackendFixtureFactory>
{
    private BackendFixtureFactory Factory { get; } = factory;

    // [Fact]
    // public async Task CreateOrganizationUserFromSessionReturnOrganizationUser()
    // {
    //     // Arrange
    //     using var client = Factory.CreateClient(OrganizationRole.Superuser);

    //     // Act
    //     var response = await client.PostAsJsonAsync("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());
    //     var returnObject = await response.Content.ReadFromJsonAsync<User>();

    //     // Assert
    //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //     Assert.NotNull(returnObject);
    // }

    [Fact]
    public async Task GetAllOrganizationUserFromSessionReturnAllOrganizationUser()
    {
        // Arrange
        using var client = Factory.CreateClient(OrganizationRole.Superuser);

        // Act
        var response = await client.GetAsync("api/organization/user");
        var returnList = await response.Content.ReadFromJsonAsync<List<User>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnList);
        Assert.True(returnList.Count >= 1);
    }

    // [Fact]
    // public async Task UpdateOrganizationUserFromSessionReturnNoContent()
    // {
    //     // Arrange
    //     using var client = Factory.CreateClient(OrganizationRole.Superuser);
    //     var user = await client.PostAsJsonGetFromJsonAsync<User, OrganizationUserPasswordDto>("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());

    //     // Act
    //     Assert.NotNull(user);
    //     var response = await client.PutAsJsonAsync($"api/organization/user/{user.Id}", new UserFaker().Generate());

    //     // Assert
    //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    // }

    // [Fact]
    // public async Task ChangeOrganizationUserRoleFromSessionReturnNoContent()
    // {
    //     // Arrange
    //     using var client = Factory.CreateClient(OrganizationRole.Superuser);
    //     var user = await client.PostAsJsonGetFromJsonAsync<User, OrganizationUserPasswordDto>("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());

    //     // Act
    //     Assert.NotNull(user);
    //     var response = await client.PostAsJsonAsync($"api/organization/user/{user.Id}/change-organization-role", new ChangeOrganizationRoleDtoFaker().Generate());

    //     // Assert
    //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    // }

    // [Fact]
    // public async Task ChangeOrganizationUserPasswordFromSessionReturnNoContent()
    // {
    //     // Arrange
    //     using var client = Factory.CreateClient(OrganizationRole.Superuser);
    //     var user = await client.PostAsJsonGetFromJsonAsync<User, OrganizationUserPasswordDto>("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());

    //     // Act
    //     Assert.NotNull(user);
    //     var response = await client.PostAsJsonAsync($"api/organization/user/{user.Id}/change-password", new ChangePasswordDtoFaker().Generate());

    //     // Assert
    //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    // }

    // [Fact]
    // public async Task DeleteOrganizationUserFromSessionReturnNoContent()
    // {
    //     // Arrange
    //     using var client = Factory.CreateClient(OrganizationRole.Superuser);
    //     var user = await client.PostAsJsonGetFromJsonAsync<User, OrganizationUserPasswordDto>("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());

    //     // Act
    //     Assert.NotNull(user);
    //     var response = await client.DeleteAsync($"api/organization/user/{user.Id}");

    //     // Assert
    //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    // }

    // [Theory]
    // [InlineData(OrganizationRole.Verifier)]
    // [InlineData(OrganizationRole.Writer)]
    // [InlineData(OrganizationRole.Reader)]
    // public async Task CreateOrganizationUserFromSessionReturnForbidden(OrganizationRole role)
    // {
    //     // Arrange
    //     using var client = Factory.CreateClient(role);

    //     // Act
    //     var response = await client.PostAsJsonAsync("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());

    //     // Assert
    //     Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    // }

    // [Theory]
    // [InlineData(OrganizationRole.Verifier)]
    // [InlineData(OrganizationRole.Writer)]
    // [InlineData(OrganizationRole.Reader)]
    // public async Task UpdateOrganizationUserFromSessionReturnForbidden(OrganizationRole role)
    // {
    //     // Arrange
    //     using var superuserClient = Factory.CreateClient(OrganizationRole.Superuser);
    //     var user = await superuserClient.PostAsJsonGetFromJsonAsync<User, OrganizationUserPasswordDto>("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());
    //     using var client = Factory.CreateClient(role);

    //     // Act
    //     Assert.NotNull(user);
    //     var response = await client.PutAsJsonAsync($"api/organization/user/{user.Id}", new UserFaker().Generate());

    //     // Assert
    //     Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    // }

    // [Theory]
    // [InlineData(OrganizationRole.Verifier)]
    // [InlineData(OrganizationRole.Writer)]
    // [InlineData(OrganizationRole.Reader)]
    // public async Task DeleteOrganizationUserFromSessionReturnForbidden(OrganizationRole role)
    // {
    //     // Arrange
    //     using var superuserClient = Factory.CreateClient(OrganizationRole.Superuser);
    //     var user = await superuserClient.PostAsJsonGetFromJsonAsync<User, OrganizationUserPasswordDto>("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());
    //     using var client = Factory.CreateClient(role);

    //     // Act
    //     Assert.NotNull(user);
    //     var response = await client.DeleteAsync($"api/organization/user/{user.Id}");

    //     // Assert
    //     Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    // }
}
