using System;
using System.IO;

namespace FunderMaps.Core.Helpers
{
    public class ApplicationFile
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public string Extension { get => Path.GetExtension(Name); }

        /// <summary>
        /// Create new instance of application file.
        /// </summary>
        /// <param name="name">File name.</param>
        public ApplicationFile(string name) => Name = name;

        /// <summary>
        /// Create new instance of application file.
        /// </summary>
        /// <param name="name">File name.</param>
        /// <param name="newName">New file name.</param>
        public ApplicationFile(string name, string newName)
        {
            FileName = newName;
            Name = name;
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
                throw new ArgumentNullException(nameof(extension));
            }

            // Remove the dot from extension.
            if (extension[0] == '.')
            {
                extension = extension.Substring(1);
            }

            return $"{Guid.NewGuid()}.{extension}";
        }
    }
}
