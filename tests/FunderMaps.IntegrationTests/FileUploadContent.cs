using System.Net.Http;
using System.Net.Http.Headers;
using Bogus;

namespace FunderMaps.IntegrationTests
{
    /// <summary>
    ///     File upload content helper.
    /// </summary>
    public class FileUploadContent : MultipartFormDataContent
    {
        private Faker faker = new();

        private HttpContent byteContent;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public FileUploadContent(string mediaType, string fileExtension, int? byteContentLength = null)
        {
            var contentLength = byteContentLength ?? faker.Random.Int(1, 1024);

            byteContent = new ByteArrayContent(faker.Random.Bytes(contentLength));
            byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mediaType);

            // Add the byte content to the form data.
            Add(byteContent, "input", faker.System.FileName(fileExtension));
        }

        protected override void Dispose(bool disposing)
        {
            byteContent.Dispose();
            base.Dispose(disposing);
        }
    }
}
