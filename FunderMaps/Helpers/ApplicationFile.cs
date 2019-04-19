using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace FunderMaps.Helpers
{
    public class ApplicationFile
    {
        public string FileName { get; }
        public string Name { get; }
        public string ContentType { get; }
        public long Size { get; }
        public string Extension { get => Path.GetExtension(Name); }

        public ApplicationFile(string name)
        {
            Name = name;
        }

        public ApplicationFile(string name, string newName)
        {
            FileName = newName;
            Name = name;
        }

        public ApplicationFile(IFormFile formFile)
        {
            Name = formFile.FileName;
            FileName = GenerateUniqueFileName(Extension);
            ContentType = formFile.ContentType.ToLower().Trim();
            Size = formFile.Length;
        }

        /// <summary>
        /// Check if file is empty
        /// </summary>
        /// <returns></returns>
        public bool Empty() => Size == 0;

        /// <summary>
        /// Generate a unique name for the file.
        /// </summary>
        /// <param name="extension">File extension.</param>
        /// <returns>Unique filename.</returns>
        public static string GenerateUniqueFileName(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException();
            }

            if (extension[0] == '.')
            {
                extension = extension.Substring(1);
            }

            return $"{Guid.NewGuid()}.{extension}";
        }
    }
}
