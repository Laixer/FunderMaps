using FunderMaps.Core.Utils;
using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Extensions
{
    /// <summary>
    /// Enum extensions.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Convert enum option to member name based on defined attributes.
        /// </summary>
        /// <param name="enumObject"><see cref="Enum"/>.</param>
        /// <returns>Name of member as string.</returns>
        public static string ToMemberName(this Enum enumObject)
        {
            if (enumObject == null)
            {
                throw new ArgumentNullException(nameof(enumObject));
            }

            var attribute = EnumHelper.AttributeSingleOrDefault<EnumMemberAttribute>(enumObject);
            if (attribute != null)
            {
                return attribute.Value;
            }

            return enumObject.ToString().ToSnakeCase();
        }
    }
}
