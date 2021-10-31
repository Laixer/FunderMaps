using System;

namespace FunderMaps.AspNetCore.DataTransferObjects;

/// <summary>
///     Blob resource access link DTO.
/// </summary>
public record BlobAccessLinkDto
{
    /// <summary>
    ///     Blob resource access link.
    /// </summary>
    public Uri AccessLink { get; init; }
}
