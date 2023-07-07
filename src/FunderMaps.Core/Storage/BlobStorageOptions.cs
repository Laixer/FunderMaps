namespace FunderMaps.Core.Storage;

/// <summary>
///     Options for the blob storage service.
/// </summary>
public sealed record BlobStorageOptions
{
    /// <summary>
    ///     Configuration section key.
    /// </summary>
    public const string Section = "BlobStorage";

    /// <summary>
    ///     Base service uri for blob storage service.
    /// </summary>
    public string? ServiceUri { get; set; }

    /// <summary>
    ///     Name of the blob storage.
    /// </summary>
    public string? BucketName { get; set; }

    /// <summary>
    ///     Public access key.
    /// </summary>
    public string? AccessKeyId { get; set; }

    /// <summary>
    ///     Private secret key.
    /// </summary>
    public string? SecretKey { get; set; }
}
