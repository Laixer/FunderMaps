namespace FunderMaps.Core.ExternalServices.FunderMaps;

/// <summary>
///     Options for the open AI service.
/// </summary>
public sealed record FunderMapsOptions
{
    /// <summary>
    ///     Configuration section key.
    /// </summary>
    public const string Section = "FunderMaps";

    /// <summary>
    ///     FunderMaps base URL.
    /// </summary>
    public string? BaseUrl { get; set; }

    /// <summary>
    ///     FunderMaps email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     FunderMaps password.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    ///     FunderMaps API key.
    /// </summary>
    public string? ApiKey { get; set; }
}
