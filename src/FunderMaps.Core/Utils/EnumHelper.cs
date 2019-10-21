using System;
using System.Linq;
using System.Reflection;

namespace FunderMaps.Core.Utils
{
    /// <summary>
    /// Utilities for enums.
    /// </summary>
    internal static class EnumHelper
    {
        /// <summary>
        /// Get attribute from enum value.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute to search for.</typeparam>
        /// <param name="enumObject">Enum to find attributes on.</param>
        /// <returns>Instance of <typeparamref name="TAttribute"/> or null.</returns>
        public static TAttribute AttributeSingleOrDefault<TAttribute>(Enum enumObject)
            where TAttribute : Attribute
            => enumObject.GetType()
                .GetMember(enumObject.ToString())
                .Where(member => member.MemberType == MemberTypes.Field)
                .FirstOrDefault()
                .GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>()
                .SingleOrDefault();
    }
}
