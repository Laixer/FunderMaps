using System;
using System.IO;

namespace FunderMaps.Core.Helpers
{
    /// <summary>
    ///     File operations.
    /// </summary>
    public class FileWrapper
    {
        /// <summary>
        ///     File name in storage.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     File name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     File content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     File size.
        /// </summary>
        public ulong Size { get; set; }

        /// <summary>
        ///     File extension.
        /// </summary>
        public string Extension => Path.GetExtension(Name);

        /// <summary>
        ///     Check if file is empty
        /// </summary>
        /// <returns><c>True</c> if the file is empty.</returns>
        public bool IsEmpty => Size == 0;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="name">File name.</param>
        public FileWrapper(string name)
            => Name = name;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="name">File name.</param>
        /// <param name="newName">New file name.</param>
        public FileWrapper(string name, string newName)
        {
            FileName = newName;
            Name = name;
        }

        /// <summary>
        ///     Generate a unique file name.
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
