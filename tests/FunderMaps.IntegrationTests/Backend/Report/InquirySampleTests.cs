﻿using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report;

/// <summary>
///     Create new instance.
/// </summary>
public class InquirySampleTests(BackendFixtureFactory factory) : IClassFixture<BackendFixtureFactory>
{
    private BackendFixtureFactory Factory { get; } = factory;

    [Fact]
    public async Task InquirySampleLifeCycle()
    {
        var inquiry = await ReportStub.CreateInquiryAsync(Factory);
        var sample = await ReportStub.CreateInquirySampleAsync(Factory, inquiry);

        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}/sample/stats");
            var returnObject = await response.Content.ReadFromJsonAsync<DatasetStatsDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
            Assert.True(returnObject.Count >= 1);
        }

        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<InquirySample>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
            Assert.Equal(sample.Id, returnObject.Id);
            Assert.Equal(inquiry.Id, returnObject.Inquiry);
        }

        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}/sample");
            var returnList = await response.Content.ReadFromJsonAsync<List<InquirySample>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnList);
            Assert.True(returnList.Count >= 1);
        }

        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Writer);
            var newObject = new InquirySampleFaker()
                .RuleFor(f => f.Address, f => "gfm-5c9a38bae2b9495b8ef9dc5684ba4fc5")
                .Generate();

            // Act
            var response = await client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}", newObject);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        await ReportStub.DeleteInquiryAsync(Factory, inquiry);
    }

    [Fact]
    public async Task InquirySampleResetLifeCycle()
    {
        var inquiry = await ReportStub.CreateInquiryAsync(Factory);
        await ReportStub.CreateInquirySampleAsync(Factory, inquiry);

        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Writer);

            // Act
            var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/status_review", new StatusChangeDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Superuser);

            // Act
            var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/reset", new { });

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        await ReportStub.DeleteInquiryAsync(Factory, inquiry);
    }

    [Theory]
    [InlineData("status_approved")]
    [InlineData("status_rejected")]
    public async Task InquirySampleStatusLifeCycle(string uri)
    {
        var inquiry = await ReportStub.CreateInquiryAsync(Factory);
        await ReportStub.CreateInquirySampleAsync(Factory, inquiry);

        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Writer);

            // Act
            var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/status_review", new StatusChangeDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Verifier);

            // Act
            var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/{uri}", new StatusChangeDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Superuser);

            // Act
            var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/reset", new { });

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        await ReportStub.DeleteInquiryAsync(Factory, inquiry);
    }

    [Fact]
    public async Task InquirySampleDeleteLifeCycle()
    {
        var inquiry = await ReportStub.CreateInquiryAsync(Factory);
        var sample = await ReportStub.CreateInquirySampleAsync(Factory, inquiry);

        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Writer);

            // Act
            var response = await client.DeleteAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        await ReportStub.DeleteInquiryAsync(Factory, inquiry);
    }

    [Fact]
    public async Task InquirySampleLifeCycleForbidden()
    {
        var inquiry = await ReportStub.CreateInquiryAsync(Factory);
        var sample = await ReportStub.CreateInquirySampleAsync(Factory, inquiry);

        {
            // Arrange
            using var client = Factory.CreateClient();
            var newObject = new RecoverySampleFaker()
                .RuleFor(f => f.Building, f => "gfm-989b8bd98dfb47e1ae26bd8c0c8be2cb")
                // .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                .Generate();

            // Act
            var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/sample", newObject);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        {
            // Arrange
            using var client = Factory.CreateClient();
            var newObject = new RecoverySampleFaker()
                .RuleFor(f => f.Building, f => "gfm-dd25089ebda44dd5b6061000135f60f4")
                // .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                .Generate();

            // Act
            var response = await client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}", newObject);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        await ReportStub.DeleteInquiryAsync(Factory, inquiry);
    }
}
