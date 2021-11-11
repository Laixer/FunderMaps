using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.DataAnnotations
{
    /// <summary>
    ///     Incident validation attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IncidentAttribute : ValidationAttribute
    {
        /// <summary>
        ///     Returns true if the value starts with incident identifier.
        /// </summary>
        /// <param name="value">The value to test for validity.</param>
        /// <returns><c>true</c> means the <paramref name="value" /> is valid.</returns>
        public override bool IsValid(object value)
            => value is null || (value is string str && str.StartsWith("FIR", StringComparison.InvariantCulture));

        /// <summary>
        ///     Override of <see cref="ValidationAttribute.FormatErrorMessage" />
        /// </summary>
        /// <remarks>This override exists to provide a formatted message describing the invalid field.</remarks>
        /// <param name="name">The user-visible name to include in the formatted message.</param>
        /// <returns>A string describing the invalid field.</returns>
        public override string FormatErrorMessage(string name)
            => $"The {name} field is not a valid incident identifier.";
    }
}
