namespace FunderMaps.Core.Options;

/// <summary>
///     Options for the blob storage service.
/// </summary>
public sealed record S3StorageOptions
{
    /// <summary>
    ///     Configuration section key.
    /// </summary>
    public const string Section = "BlobStorage"; // TODO: Rename to S3Storage

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
