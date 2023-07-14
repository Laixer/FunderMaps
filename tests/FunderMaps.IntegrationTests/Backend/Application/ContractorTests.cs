using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application;

public class ContractorTests : IClassFixture<BackendFixtureFactory>
{
    private BackendFixtureFactory Factory { get; }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ContractorTests(BackendFixtureFactory factory)
        => Factory = factory;

    [Theory]
    [InlineData(OrganizationRole.Superuser)]
    [InlineData(OrganizationRole.Verifier)]
    [InlineData(OrganizationRole.Writer)]
    [InlineData(OrganizationRole.Reader)]
    public async Task GetAllContractorReturnAllContractor(OrganizationRole role)
    {
        // Arrange
        using var client = Factory.CreateClient(role);

        // Act
        var response = await client.GetAsync("api/contractor");
        var returnList = await response.Content.ReadFromJsonAsync<List<Contractor>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnList);
        Assert.True(returnList.Count >= 1);
        Assert.True(response.Headers.CacheControl?.Public);
    }
}
