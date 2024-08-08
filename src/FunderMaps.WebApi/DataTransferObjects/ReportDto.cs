using FunderMaps.Core.Entities;

namespace FunderMaps.WebApi.DataTransferObjects;

public record ReportDto
{
    public List<Incident> Incidents { get; init; } = [];
    public List<Inquiry> Inquiries { get; init; } = [];
    public List<InquirySample> InquirySamples { get; init; } = [];
    public List<Recovery> Recoveries { get; init; } = [];
    public List<RecoverySample> RecoverySamples { get; init; } = [];
}
