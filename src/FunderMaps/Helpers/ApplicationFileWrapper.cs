using FunderMaps.Core.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunderMaps.Helpers
{
    /// <summary>
    /// Application file wrapper.
    /// </summary>
    public class ApplicationFileWrapper
    {
        private readonly IEnumerable<string> _allowedFileTypes;

        /// <summary>
        /// Application file.
        /// </summary>
        public ApplicationFile File { get; }

        /// <summary>
        /// Indicates the file is either valid or not.
        /// </summary>
        public bool IsValid { get => string.IsNullOrEmpty(Error); }

        /// <summary>
        /// Validation error, if any.
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// Wrapper around IFormFIle to Application file.
        /// </summary>
        /// <param name="formFile">See <see cref="IFormFile"/>.</param>
        /// <param name="allowedFileTypes">List of allowed mime types.</param>
        public ApplicationFileWrapper(IFormFile formFile, IEnumerable<string> allowedFileTypes = null)
        {
            if (formFile == null)
            {
                throw new ArgumentNullException(nameof(formFile));
            }

            _allowedFileTypes = allowedFileTypes;

            File = new ApplicationFile(formFile.FileName)
            {
                ContentType = formFile.ContentType.ToLower().Trim(),
                Size = formFile.Length,
            };

            File.FileName = ApplicationFile.GenerateUniqueFileName(File.Extension);

            CheckIfValid();
        }

        /// <summary>
        /// Check if submitted file is valid.
        /// </summary>
        private void CheckIfValid()
        {
            // Mark empty document as invalid
            if (File.Empty())
            {
                Error = "file content is empty";
            }

            // Check if content type is allowed
            if (_allowedFileTypes.Count() > 0 && !_allowedFileTypes.Contains(File.ContentType))
            {
                Error = "file content type is not allowed";
            }
        }
    }
}
