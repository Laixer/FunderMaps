namespace FunderMaps.Core.DataTransferObjects;

// TODO: Move to core?
/// <summary>
///     Blob resource access link DTO.
/// </summary>
public record BlobAccessLinkDto
{
    /// <summary>
    ///     Blob resource access link.
    /// </summary>
    public Uri? AccessLink { get; init; }
}
