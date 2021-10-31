using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report;

public class InquirySampleTests : IClassFixture<BackendFixtureFactory>
{
    private BackendFixtureFactory Factory { get; }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public InquirySampleTests(BackendFixtureFactory factory)
        => Factory = factory;

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
            Assert.True(returnObject.Count >= 1);
        }

        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<InquirySampleDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(sample.Id, returnObject.Id);
            Assert.Equal(inquiry.Id, returnObject.Inquiry);
        }

        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}/sample");
            var returnList = await response.Content.ReadFromJsonAsync<List<InquirySampleDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count >= 1);
        }

        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Writer);
            var newObject = new InquirySampleDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
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
        var sample = await ReportStub.CreateInquirySampleAsync(Factory, inquiry);

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
        var sample = await ReportStub.CreateInquirySampleAsync(Factory, inquiry);

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
            var newObject = new RecoverySampleDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
                .Generate();

            // Act
            var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/sample", newObject);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        {
            // Arrange
            using var client = Factory.CreateClient();
            var newObject = new RecoverySampleDtoFaker()
                .RuleFor(f => f.Address, f => "gfm-351cc5645ab7457b92d3629e8c163f0b")
                .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
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
