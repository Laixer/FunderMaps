namespace FunderMaps.Core.ExternalServices.Mapbox;

/// <summary>
///     Options for the Mapbox service.
/// </summary>
public sealed record MapboxOptions
{
    /// <summary>
    ///     Configuration section key.
    /// </summary>
    public const string Section = "Mapbox";

    /// <summary>
    ///     Mapbox API key.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    ///     Mapbox account.
    /// </summary>
    public string? Account { get; set; }
}
