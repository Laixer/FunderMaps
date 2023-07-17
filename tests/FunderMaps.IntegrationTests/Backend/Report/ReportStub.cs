using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Report;

/// <summary>
///     Teststub for all report tests.
/// </summary>
public static class ReportStub
{
    public static async Task<Recovery> CreateRecoveryAsync(BackendFixtureFactory factory)
    {
        // Arrange
        using var client = factory.CreateClient(OrganizationRole.Writer);
        var newObject = new RecoveryFaker()
            .RuleFor(f => f.Attribution.Reviewer, f => Guid.Parse("21c403fe-45fc-4106-9551-3aada1bbdec3"))
            .RuleFor(f => f.Attribution.Contractor, f => 10)
            .Generate();

        // Act
        var response = await client.PostAsJsonAsync("api/recovery", newObject);
        var returnObject = await response.Content.ReadFromJsonAsync<Recovery>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal(AuditStatus.Todo, returnObject.State.AuditStatus);
        Assert.Null(returnObject.Record.UpdateDate);

        return returnObject;
    }

    public static async Task<Inquiry> CreateInquiryAsync(BackendFixtureFactory factory)
    {
        // Arrange
        var inquiry = new InquiryFaker()
            .RuleFor(f => f.Attribution.Reviewer, f => Guid.Parse("21c403fe-45fc-4106-9551-3aada1bbdec3"))
            .RuleFor(f => f.Attribution.Contractor, f => 10)
            .Generate();
        using var client = factory.CreateClient(OrganizationRole.Writer);

        // Act
        var response = await client.PostAsJsonAsync("api/inquiry", inquiry);
        var returnObject = await response.Content.ReadFromJsonAsync<Inquiry>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal(AuditStatus.Todo, returnObject.State.AuditStatus);
        Assert.Null(returnObject.Record.UpdateDate);

        return returnObject;
    }

    public static async Task<Incident> CreateIncidentAsync(BackendFixtureFactory factory)
    {
        // Arrange
        var incident = new IncidentFaker()
            .RuleFor(f => f.Address, f => "gfm-87ca83c36c904f7e8ad84724bffd2df1")
            .Generate();
        using var client = factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("api/incident", incident);
        var returnObject = await response.Content.ReadFromJsonAsync<Incident>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.StartsWith("FIR", returnObject.Id, StringComparison.InvariantCulture);
        Assert.Equal(AuditStatus.Todo, returnObject.AuditStatus);

        return returnObject;
    }

    public static async Task DeleteRecoveryAsync(BackendFixtureFactory factory, Recovery recovery)
    {
        // Arrange
        using var client = factory.CreateClient(OrganizationRole.Superuser);

        // Act
        var response = await client.DeleteAsync($"api/recovery/{recovery.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    public static async Task DeleteInquiryAsync(BackendFixtureFactory factory, Inquiry inquiry)
    {
        // Arrange
        using var client = factory.CreateClient(OrganizationRole.Superuser);

        // Act
        var response = await client.DeleteAsync($"api/inquiry/{inquiry.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    public static async Task DeleteIncidentAsync(BackendFixtureFactory factory, Incident incident)
    {
        // Arrange
        using var client = factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"api/incident/{incident.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    public static async Task<RecoverySample> CreateRecoverySampleAsync(BackendFixtureFactory factory, Recovery recovery)
    {
        // Arrange
        using var client = factory.CreateClient(OrganizationRole.Writer);
        var newObject = new RecoverySampleFaker()
            .RuleFor(f => f.Building, f => "gfm-f53334d806ab4ab386e8df29111add21")
            // .RuleFor(f => f.Contractor, f => Guid.Parse("62af863e-2021-4438-a5ea-730ed3db9eda"))
            .Generate();

        // Act
        var response = await client.PostAsJsonAsync($"api/recovery/{recovery.Id}/sample", newObject);
        var returnObject = await response.Content.ReadFromJsonAsync<RecoverySample>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal(recovery.Id, returnObject.Recovery);
        Assert.Equal(newObject.Building, returnObject.Building);

        return returnObject;
    }

    public static async Task<InquirySample> CreateInquirySampleAsync(BackendFixtureFactory factory, Inquiry inquiry)
    {
        using var client = factory.CreateClient(OrganizationRole.Writer);
        var newObject = new InquirySampleFaker()
            .RuleFor(f => f.Address, f => "gfm-2687feed6a624636b70700cd374fbd17")
            .Generate();

        // Act
        var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/sample", newObject);
        var returnObject = await response.Content.ReadFromJsonAsync<InquirySample>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal(inquiry.Id, returnObject.Inquiry);
        Assert.Equal(newObject.Address, returnObject.Address);

        return returnObject;
    }
}
