using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application;

/// <summary>
///     Create new instance.
/// </summary>
public class OrganizationTests(BackendFixtureFactory factory) : IClassFixture<BackendFixtureFactory>
{
    private BackendFixtureFactory Factory { get; } = factory;

    [Theory]
    [InlineData(OrganizationRole.Superuser)]
    [InlineData(OrganizationRole.Verifier)]
    [InlineData(OrganizationRole.Writer)]
    [InlineData(OrganizationRole.Reader)]
    public async Task GetOrganizationFromSessionReturnSingleOrganization(OrganizationRole role)
    {
        // Arrange
        using var client = Factory.CreateClient(role);

        // Act
        var response = await client.GetAsync("api/organization");
        var returnObject = await response.Content.ReadFromJsonAsync<Organization>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal(Guid.Parse("05203318-6c55-43c1-a6a6-bb8c83f930c3"), returnObject.Id);
        Assert.NotNull(returnObject.Name);
        Assert.NotNull(returnObject.Email);
    }

    [Fact]
    public async Task UpdateOrganizationFromSessionReturnNoContent()
    {
        // Arrange
        using var client = Factory.CreateClient(OrganizationRole.Superuser);
        var updateObject = new OrganizationFaker().Generate();

        // Act
        var response = await client.PutAsJsonAsync("api/organization", updateObject);

        // Act
        var returnObject = await client.GetFromJsonAsync<Organization>("api/organization");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Theory]
    [InlineData(OrganizationRole.Verifier)]
    [InlineData(OrganizationRole.Writer)]
    [InlineData(OrganizationRole.Reader)]
    public async Task UpdateOrganizationFromSessionReturnForbidden(OrganizationRole role)
    {
        // Arrange
        using var client = Factory.CreateClient(role);

        // Act
        var response = await client.PutAsJsonAsync("api/organization", new OrganizationFaker().Generate());

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
