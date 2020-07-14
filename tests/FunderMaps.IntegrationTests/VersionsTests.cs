﻿using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests
{
    public class VersionsTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public VersionsTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetEndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/version").ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.True(response.Content.Headers.ContentType.ToString().Contains("json", StringComparison.InvariantCultureIgnoreCase));
            Assert.True(response.Content.Headers.ContentType.ToString().Contains("utf-8", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
