using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Bogus;

namespace FunderMaps.IntegrationTests
{
    /// <summary>
    ///     File upload content helper.
    /// </summary>
    public class FileUploadContent : MultipartFormDataContent, IDisposable
    {
        private Faker faker = new Faker();

        private HttpContent byteContent;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public FileUploadContent(string mediaType, string fileExtension)
        {
            byteContent = new ByteArrayContent(faker.Random.Bytes(1024));
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
