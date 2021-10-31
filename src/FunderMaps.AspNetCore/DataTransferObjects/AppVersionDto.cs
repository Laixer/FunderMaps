namespace FunderMaps.AspNetCore.DataTransferObjects;

/// <summary>
///     Application version DTO.
/// </summary>
public sealed record AppVersionDto
{
    /// <summary>
    ///     Application name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    ///     Application version.
    /// </summary>
    public string Version { get; init; }

    /// <summary>
    ///     Application version commit.
    /// </summary>
    public string Commit { get; init; }
}
