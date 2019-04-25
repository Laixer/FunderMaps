using System;
using System.Text.RegularExpressions;

namespace FunderMaps.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToUnderscore(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("message", nameof(str));
            }

            return Regex.Replace(str, "(?<=[a-z])([A-Z0-9])", "_$0", RegexOptions.Compiled).ToLower();
        }
    }
}
