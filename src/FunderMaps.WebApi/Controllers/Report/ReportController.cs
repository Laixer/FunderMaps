using FunderMaps.Core.Controllers;
using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Report;

public record ReportDto
{
    public List<Incident> Incidents { get; init; } = [];
    public List<Inquiry> Inquiries { get; init; } = [];
    public List<InquirySample> InquirySamples { get; init; } = [];
    public List<Recovery> Recoveries { get; init; } = [];
    public List<RecoverySample> RecoverySamples { get; init; } = [];
}

[Route("api/report")]
public sealed class ReportController(
    IIncidentRepository incidentRepository,
    IInquiryRepository inquiryRepository,
    IInquirySampleRepository inquirySampleRepository,
    IRecoveryRepository recoveryRepository,
    IRecoverySampleRepository recoverySampleRepository,
    GeocoderTranslation geocoderTranslation) : FunderMapsController
{
    // GET: api/report/{id}
    /// <summary>
    ///    Return all inquiries by building id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ReportDto> GetAsync(string id, [FromQuery] PaginationDto pagination)
    {
        var building = await geocoderTranslation.GetBuildingIdAsync(id);

        var report = new ReportDto();

        await foreach (var incident in incidentRepository.ListAllByBuildingIdAsync(building.Id))
        {
            report.Incidents.Add(incident);
        }

        await foreach (var inquiry in inquiryRepository.ListAllByBuildingIdAsync(pagination.Navigation, TenantId, building.Id))
        {
            report.Inquiries.Add(inquiry);
        }

        // TODO: Filter by tenantId
        await foreach (var item in inquirySampleRepository.ListAllByBuildingIdAsync(building.Id))
        {
            report.InquirySamples.Add(item);
        }

        await foreach (var recovery in recoveryRepository.ListAllByBuildingIdAsync(pagination.Navigation, TenantId, building.Id))
        {
            report.Recoveries.Add(recovery);
        }

        // TODO: Filter by tenantId
        await foreach (var item in recoverySampleRepository.ListAllByBuildingIdAsync(building.Id))
        {
            report.RecoverySamples.Add(item);
        }

        return report;
    }
}
