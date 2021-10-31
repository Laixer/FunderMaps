namespace FunderMaps.Infrastructure.Storage;

/// <summary>
///     Options for the mapbox service.
/// </summary>
public sealed record MapboxOptions
{
    /// <summary>
    ///     Account name.
    /// </summary>
    public string Account { get; init; }

    /// <summary>
    ///     Access token.
    /// </summary>
    public string AccessToken { get; init; }
}
