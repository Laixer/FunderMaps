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

        return new ReportDto
        {
            Incidents = await incidentRepository.ListAllByBuildingIdAsync(building.Id).ToListAsync(),
            Inquiries = await inquiryRepository.ListAllByBuildingIdAsync(pagination.Navigation, TenantId, building.Id).ToListAsync(),
            InquirySamples = await inquirySampleRepository.ListAllByBuildingIdAsync(building.Id).ToListAsync(),
            Recoveries = await recoveryRepository.ListAllByBuildingIdAsync(pagination.Navigation, TenantId, building.Id).ToListAsync(),
            RecoverySamples = await recoverySampleRepository.ListAllByBuildingIdAsync(building.Id).ToListAsync(),
        };
    }
}
