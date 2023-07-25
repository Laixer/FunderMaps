using FunderMaps.Core.Storage;

namespace FunderMaps.Core.Interfaces;

/// <summary>
///     Store a file contents in a data store.
/// </summary>
public interface IBlobStorageService : IServiceHealthCheck
{
    /// <summary>
    ///     Retrieve file access link as uri.
    /// </summary>
    /// <param name="containerName">Storage container.</param>
    /// <param name="fileName">File name.</param>
    /// <param name="hoursValid">How long the link is valid in hours.</param>
    /// <returns>The generated link.</returns>
    Task<Uri> GetAccessLinkAsync(string containerName, string fileName, double hoursValid);

    /// <summary>
    ///     Upload an object to the bucket.
    /// </summary>
    /// <param name="fileName">The file name.</param>
    /// <param name="filePath">The file path.</param>
    /// <param name="storageObject">Storage object settings.</param>
    /// <returns>See <see cref="ValueTask"/>.</returns>
    Task StoreFileAsync(string fileName, string filePath, StorageObject? storageObject = null);

    // FUTURE: Refactor
    /// <summary>
    ///     Stores a file in Amazon S3.
    /// </summary>
    /// <param name="containerName">The container name.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="contentType">The content type.</param>
    /// <param name="stream">See <see cref="Stream"/>.</param>
    /// <param name="storageObject">Storage object settings.</param>
    /// <returns>See <see cref="ValueTask"/>.</returns>
    Task StoreFileAsync(string containerName, string fileName, string contentType, Stream stream, StorageObject? storageObject = null);

    // FUTURE: Refactor
    /// <summary>
    ///     Stores a directory in Amazon S3.
    /// </summary>
    /// <param name="directoryName">Directory name at the destination including prefix paths.</param>
    /// <param name="directoryPath">Source directory.</param>
    /// <param name="storageObject">Storage object settings.</param>
    /// <returns>See <see cref="ValueTask"/>.</returns>
    Task StoreDirectoryAsync(string directoryName, string directoryPath, StorageObject? storageObject = null);
}
