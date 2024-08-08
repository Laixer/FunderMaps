using FunderMaps.Core.Entities;

namespace FunderMaps.WebApi.DataTransferObjects;

/// <summary>
///     Represents a data transfer object for a report.
/// </summary>
public record ReportDto
{
    /// <summary>
    ///     Gets or sets the list of incidents.
    /// </summary>
    public List<Incident> Incidents { get; init; } = [];

    /// <summary>
    ///     Gets or sets the list of inquiries.
    /// </summary>
    public List<Inquiry> Inquiries { get; init; } = [];

    /// <summary>
    ///     Gets or sets the list of inquiry samples.
    /// </summary>
    public List<InquirySample> InquirySamples { get; init; } = [];

    /// <summary>
    ///     Gets or sets the list of recoveries.
    /// </summary>
    public List<Recovery> Recoveries { get; init; } = [];

    /// <summary>
    ///     Gets or sets the list of recovery samples.
    /// </summary>
    public List<RecoverySample> RecoverySamples { get; init; } = [];
}
