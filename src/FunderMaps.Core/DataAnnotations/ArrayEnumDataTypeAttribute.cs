using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.DataAnnotations
{
    /// <summary>
    ///     Enum data type validator over array.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ArrayEnumDataTypeAttribute : ValidationAttribute
    {
        /// <summary>
        ///     Gets or sets the enumeration type.
        /// </summary>
        public Type EnumType { get; }

        private EnumDataTypeAttribute validator;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ArrayEnumDataTypeAttribute(Type enumType)
        {
            validator = new EnumDataTypeAttribute(enumType);
            EnumType = enumType;
        }

        /// <summary>
        ///     Returns true if the array is all valid enum types.
        /// </summary>
        /// <param name="value">The value to test for validity.</param>
        /// <returns><c>true</c> means the <paramref name="value" /> is valid.</returns>
        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            if (value is IEnumerable enumerableValue)
            {
                foreach (var item in enumerableValue)
                {
                    if (!validator.IsValid(item))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     Override of <see cref="ValidationAttribute.FormatErrorMessage" />
        /// </summary>
        /// <remarks>This override exists to provide a formatted message describing the invalid field.</remarks>
        /// <param name="name">The user-visible name to include in the formatted message.</param>
        /// <returns>A string describing the invalid field.</returns>
        public override string FormatErrorMessage(string name)
            => validator.FormatErrorMessage(name);
    }
}
