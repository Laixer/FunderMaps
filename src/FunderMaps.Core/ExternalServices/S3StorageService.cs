using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Options;
using FunderMaps.Core.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Core.ExternalServices;

/// <summary>
///     Amazon S3 implementation of <see cref="IBlobStorageService"/>.
/// </summary>
/// <remarks>
///     This creates an <see cref="IAmazonS3"/> client once in its constructor.
///     Register this service as a singleton if dependency injection is used.
/// </remarks>
internal class S3StorageService : IBlobStorageService
{
    private readonly S3StorageOptions _options;
    private readonly ILogger<S3StorageService> _logger;
    private readonly IAmazonS3 _s3Client;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public S3StorageService(IOptions<S3StorageOptions> options, ILogger<S3StorageService> logger)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var clientConfig = new AmazonS3Config();
        if (!string.IsNullOrEmpty(_options.ServiceUri))
        {
            clientConfig.ServiceURL = _options.ServiceUri;
        }

        _s3Client = new AmazonS3Client(_options.AccessKeyId, _options.SecretKey, clientConfig);
    }

    /// <summary>
    ///     Gets an access uri for a given file.
    /// </summary>
    /// <param name="containerName">The container name.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="hoursValid">How many hours the link should be valid.</param>
    /// <returns>Access <see cref="Uri"/>.</returns>
    public Task<Uri> GetAccessLinkAsync(string containerName, string fileName, double hoursValid)
    {
        var url = _s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
        {
            BucketName = _options.BucketName,
            Key = string.IsNullOrEmpty(containerName) ? fileName : $"{containerName}/{fileName}",
            Expires = DateTime.UtcNow.AddHours(hoursValid)
        });

        return Task.FromResult(new Uri(url));
    }

    /// <summary>
    ///     Upload an object to the bucket.
    /// </summary>
    /// <remarks
    ///     This method is can accept a file with any size.
    /// </remarks>
    /// <param name="fileName">The file name.</param>
    /// <param name="filePath">The file path.</param>
    /// <param name="storageObject">Storage object settings.</param>
    /// <returns>See <see cref="ValueTask"/>.</returns>
    public async Task StoreFileAsync(string fileName, string filePath, StorageObject? storageObject)
    {
        var request = new TransferUtilityUploadRequest
        {
            BucketName = _options.BucketName,
            FilePath = filePath,
            Key = fileName,
        };

        if (storageObject is not null)
        {
            request.CannedACL = storageObject.IsPublic ? S3CannedACL.PublicRead : S3CannedACL.Private;
            request.Headers.ContentType = storageObject.ContentType ?? request.Headers.ContentType;
            request.Headers.CacheControl = storageObject.CacheControl ?? request.Headers.CacheControl;
            request.Headers.ContentDisposition = storageObject.ContentDisposition ?? request.Headers.ContentDisposition;
            request.Headers.ContentEncoding = storageObject.ContentEncoding ?? request.Headers.ContentEncoding;
        }

        using var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(request);
    }

    /// <summary>
    ///     Stores a file.
    /// </summary>
    /// <remarks
    ///     This method is can accept a file with any size.
    /// </remarks>
    /// <param name="containerName">The container name.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="contentType">The content type.</param>
    /// <param name="stream">See <see cref="Stream"/>.</param>
    /// <param name="storageObject">Storage object settings.</param>
    /// <returns>See <see cref="ValueTask"/>.</returns>
    public async Task StoreFileAsync(string containerName, string fileName, string contentType, Stream stream, StorageObject? storageObject)
    {
        var request = new TransferUtilityUploadRequest
        {
            BucketName = _options.BucketName,
            ContentType = contentType,
            Key = string.IsNullOrEmpty(containerName) ? fileName : $"{containerName}/{fileName}",
            InputStream = stream,
        };

        if (storageObject is not null)
        {
            request.CannedACL = storageObject.IsPublic ? S3CannedACL.PublicRead : S3CannedACL.Private;
            request.Headers.ContentType = storageObject.ContentType ?? request.Headers.ContentType;
            request.Headers.CacheControl = storageObject.CacheControl ?? request.Headers.CacheControl;
            request.Headers.ContentDisposition = storageObject.ContentDisposition ?? request.Headers.ContentDisposition;
            request.Headers.ContentEncoding = storageObject.ContentEncoding ?? request.Headers.ContentEncoding;
        }

        using var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(request);
    }

    /// <summary>
    ///     Stores a directory.
    /// </summary>
    /// <param name="directoryName">Directory name at the destination including prefix paths.</param>
    /// <param name="directoryPath">Source directory.</param>
    /// <param name="storageObject">Storage object settings.</param>
    /// <returns>See <see cref="ValueTask"/>.</returns>
    public async Task StoreDirectoryAsync(string directoryName, string directoryPath, StorageObject? storageObject)
    {
        var request = new TransferUtilityUploadDirectoryRequest()
        {
            BucketName = _options.BucketName,
            Directory = directoryPath,
            KeyPrefix = directoryName,
            SearchOption = SearchOption.AllDirectories,
            UploadFilesConcurrently = true,
        };

        request.UploadDirectoryFileRequestEvent += (sender, uploadDirectoryRequest) =>
        {
            uploadDirectoryRequest.UploadRequest.CannedACL = (storageObject?.IsPublic ?? false) ? S3CannedACL.PublicRead : S3CannedACL.Private;
            uploadDirectoryRequest.UploadRequest.Headers.ContentType = storageObject?.ContentType ?? uploadDirectoryRequest.UploadRequest.Headers.ContentType;
            uploadDirectoryRequest.UploadRequest.Headers.CacheControl = storageObject?.CacheControl ?? uploadDirectoryRequest.UploadRequest.Headers.CacheControl;
            uploadDirectoryRequest.UploadRequest.Headers.ContentDisposition = storageObject?.ContentDisposition ?? uploadDirectoryRequest.UploadRequest.Headers.ContentDisposition;
            uploadDirectoryRequest.UploadRequest.Headers.ContentEncoding = storageObject?.ContentEncoding ?? uploadDirectoryRequest.UploadRequest.Headers.ContentEncoding;
        };

        using var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadDirectoryAsync(request);
    }

    /// <summary>
    ///     Test the Amazon S3 service backend.
    /// </summary>
    public async Task HealthCheck()
        => await _s3Client.GetBucketVersioningAsync(_options.BucketName);
}
