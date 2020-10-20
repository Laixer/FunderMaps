using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FunderMaps.AspNetCore.DataAnnotations
{
    /// <summary>
    ///     Form file mime type validation attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class FormFileAttribute : ValidationAttribute
    {
        /// <summary>
        ///     Gets or sets the allowed file mime types.
        /// </summary>
        public string[] AllowedFileMimes { get; set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public FormFileAttribute(string[] mimeTypes)
        {
            AllowedFileMimes = mimeTypes;
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public FormFileAttribute(string mimeTypes)
        {
            AllowedFileMimes = mimeTypes.Split(',').Select(s => s.Trim()).ToArray();
        }

        /// <summary>
        ///     Returns true if the file is allowed and not empty.
        /// </summary>
        /// <param name="value">The value to test for validity.</param>
        /// <returns><c>true</c> means the <paramref name="value" /> is valid</returns>
        public override bool IsValid(object value)
        {
            return value is null
                || (value is IFormFile file && AllowedFileMimes.Contains(file.ContentType.ToLower()) && file.Length > 0);
        }

        /// <summary>
        ///     Override of <see cref="ValidationAttribute.FormatErrorMessage" />
        /// </summary>
        /// <remarks>This override exists to provide a formatted message describing the invalid field.</remarks>
        /// <param name="name">The user-visible name to include in the formatted message.</param>
        /// <returns>A string describing the invalid field.</returns>
        public override string FormatErrorMessage(string name)
            => $"The input file type is not allowed.";
    }
}