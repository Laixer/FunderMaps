using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.IntegrationTests.Faker;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application;

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
        using var client = Factory.CreateAlterClient();

        // Act
        var response = await client.GetAsync("api/user");
        var returnObject = await response.Content.ReadFromJsonAsync<UserDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(Guid.Parse("ab403d16-e428-4a75-9eec-3dd08b294988"), returnObject.Id);
        Assert.Equal("corene@contoso.com", returnObject.Email);
    }

    [Fact]
    public async Task UpdateUserFromSessionReturnNoContent()
    {
        // Arrange
        using var client = Factory.CreateAlterClient();
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
    public async Task UpdateUserPhoneNumberFromSessionReturnNoContent()
    {
        // Arrange
        using var client = Factory.CreateAlterClient();
        var updateObject = new UserDtoFaker()
            .RuleFor(f => f.PhoneNumber, f => "+(31) 2234-89-12")
            .Generate();

        // Act
        var response = await client.PutAsJsonAsync("api/user", updateObject);

        // Act
        var returnObject = await client.GetFromJsonAsync<UserDto>("api/user");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal("3122348912", returnObject.PhoneNumber);
    }

    [Fact]
    public async Task ChangePasswordFromSessionReturnNoContent()
    {
        // Arrange
        using var client = Factory.CreateAlterClient();
        var newObject = new ChangePasswordDtoFaker()
            .RuleFor(f => f.OldPassword, f => "fundermaps")
            .Generate();

        // Act
        var response = await client.PostAsJsonAsync("user/change-password", newObject);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await TestStub.LoginAsync(Factory, "corene@contoso.com", newObject.NewPassword);

        response = await client.PostAsJsonAsync("user/change-password", new ChangePasswordDto()
        {
            OldPassword = newObject.NewPassword,
            NewPassword = "fundermaps",
        });

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await TestStub.LoginAsync(Factory, "corene@contoso.com", "fundermaps");
    }
}
