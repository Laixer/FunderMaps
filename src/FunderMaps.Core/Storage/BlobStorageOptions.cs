﻿namespace FunderMaps.Core.Storage;

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
    public Uri? ServiceUri { get; set; }

    /// <summary>
    ///     Name of the blob storage.
    /// </summary>
    public string? BlobStorageName { get; set; }

    /// <summary>
    ///     Public access key.
    /// </summary>
    public string? AccessKey { get; set; }

    /// <summary>
    ///     Private secret key.
    /// </summary>
    public string? SecretKey { get; set; }
}