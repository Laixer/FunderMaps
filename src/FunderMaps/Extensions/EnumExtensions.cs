using FunderMaps.Core.Extensions;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace FunderMaps.Extensions
{
    public static class EnumExtensions
    {
        // TODO: Move to core.

        public static string ToMemberName(this Enum enumObject)
        {
            if (enumObject == null)
            {
                throw new ArgumentNullException(nameof(enumObject));
            }

            // TODO: Generalize
            var attribute = enumObject.GetType()
                .GetMember(enumObject.ToString())
                .Where(member => member.MemberType == MemberTypes.Field)
                .FirstOrDefault()
                .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .Cast<EnumMemberAttribute>()
                .SingleOrDefault();

            if (attribute != null)
            {
                return attribute.Value;
            }

            return enumObject.ToString().ToSnakeCase();
        }
    }
}
