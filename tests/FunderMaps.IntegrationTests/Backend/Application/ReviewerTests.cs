﻿using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application;

/// <summary>
///     Create new instance.
/// </summary>
public class ReviewerTests(BackendFixtureFactory factory) : IClassFixture<BackendFixtureFactory>
{
    private BackendFixtureFactory Factory { get; } = factory;

    [Theory]
    [InlineData(OrganizationRole.Superuser)]
    [InlineData(OrganizationRole.Verifier)]
    [InlineData(OrganizationRole.Writer)]
    public async Task GetAllReviewerReturnAllReviewer(OrganizationRole role)
    {
        // Arrange
        using var client = Factory.CreateClient(role);

        // Act
        var response = await client.GetAsync("api/reviewer");
        var returnList = await response.Content.ReadFromJsonAsync<List<User>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnList);
        Assert.True(returnList.Count >= 1);
        Assert.True(response.Headers.CacheControl?.Private);
    }

    [Fact]
    public async Task GetAllReviewerReturnForbidden()
    {
        // Arrange
        using var client = Factory.CreateClient(OrganizationRole.Reader);

        // Act
        var response = await client.GetAsync("api/reviewer");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
