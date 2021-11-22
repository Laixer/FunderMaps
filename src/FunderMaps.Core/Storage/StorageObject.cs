namespace FunderMaps.Core.Storage;

/// <summary>
///     Storage object settings.
/// </summary>
public record StorageObject
{
    /// <summary>
    ///     Specifies if the object is public accessable.
    /// </summary>
    public bool IsPublic { get; set; } = false;

    /// <summary>
    ///     The content type of the object.
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    ///     Specifies caching behavior along the request/reply chain.
    /// </summary>
    public string CacheControl { get; set; }

    /// <summary>
    ///     Specifies presentational form for the object.
    /// </summary>
    public string ContentDisposition { get; set; }

    /// <summary>
    ///     Specifies what content encodings have been applied to the object and thus what
    ///     decoding mechanisms must be applied to obtain the media-type referenced by the
    ///     Content-Type header field.
    /// </summary>
    public string ContentEncoding { get; set; }
}
