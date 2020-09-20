using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FunderMaps.Core.DataAnnotations
{
    /// <summary>
    ///     Secure URL over array validator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ArrayUrlAttribute : ValidationAttribute
    {
        /// <summary>
        ///     Returns true if the value is a valid https url.
        /// </summary>
        /// <param name="value">The value to test for validity.</param>
        /// <returns><c>true</c> means the <paramref name="value" /> is valid</returns>
        public override bool IsValid(object value)
        {
            var val = value is null || value is string[] str && str.All(u => u.StartsWith("https://", StringComparison.InvariantCulture));
            return val;
        }

        /// <summary>
        ///     Override of <see cref="ValidationAttribute.FormatErrorMessage" />
        /// </summary>
        /// <remarks>This override exists to provide a formatted message describing the invalid field.</remarks>
        /// <param name="name">The user-visible name to include in the formatted message.</param>
        /// <returns>A string describing the invalid field.</returns>
        public override string FormatErrorMessage(string name)
            => $"The {name} field is not a valid fully-qualified https URL.";
    }
}
