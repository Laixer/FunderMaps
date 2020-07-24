using System;
using System.Data.Common;

namespace FunderMaps.Data.Extensions
{
    /// <summary>
    /// DbDataReader extensions.
    /// </summary>
    internal static class DbDataReaderExtensions
    {
        /// <summary>
        /// Return value as integer.
        /// </summary>
        /// <param name="reader">Input reader to extend.</param>
        /// <param name="ordinal">Column ordinal.</param>
        /// <returns>Value as integer.</returns>
        public static int GetInt(this DbDataReader reader, int ordinal)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return reader.GetInt32(ordinal);
        }

        /// <summary>
        /// Return value as unsigned integer.
        /// </summary>
        /// <param name="reader">Input reader to extend.</param>
        /// <param name="ordinal">Column ordinal.</param>
        /// <returns>Value as integer.</returns>
        public static uint GetUInt(this DbDataReader reader, int ordinal)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return (uint)reader.GetInt(ordinal);
        }

        //TODO: Rename SafeGetString -> GetSafeString
        /// <summary>
        /// Return value as nullable string.
        /// </summary>
        /// <param name="reader">Input reader to extend.</param>
        /// <param name="ordinal">Column ordinal.</param>
        /// <returns>Value as nullable string.</returns>
        public static string SafeGetString(this DbDataReader reader, int ordinal)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        /// <summary>
        /// Return value as nullable datetime.
        /// </summary>
        /// <param name="reader">Input reader to extend.</param>
        /// <param name="ordinal">Column ordinal.</param>
        /// <returns>Datetime or null.</returns>
        public static DateTime? GetSafeDateTime(this DbDataReader reader, int ordinal)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return reader.IsDBNull(ordinal) ? null : (DateTime?)reader.GetDateTime(ordinal);
        }

        /// <summary>
        /// Return value as nullable <typeparamref name="Type"/>.
        /// </summary>
        /// <typeparam name="Type">Type to return value to.</typeparam>
        /// <param name="reader">Input reader to extend.</param>
        /// <param name="ordinal">Column ordinal.</param>
        /// <returns>Value or null.</returns>
        public static Type GetSafeFieldValue<Type>(this DbDataReader reader, int ordinal)
            where Type : class
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return reader.IsDBNull(ordinal) ? null : reader.GetFieldValue<Type>(ordinal);
        }
    }
}
