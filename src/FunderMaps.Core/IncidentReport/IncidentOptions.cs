namespace FunderMaps.Core.IncidentReport;

// TODO: Move into FunderMaps.Core.Options
/// <summary>
///     Incident options.
/// </summary>
public sealed record IncidentOptions
{
    /// <summary>
    ///     Configuration section key.
    /// </summary>
    public const string Section = "Incident";

    /// <summary>
    ///     Incident client identifier.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    ///     The notification recipients.
    /// </summary>
    public List<string> Recipients { get; set; } = new();
}
