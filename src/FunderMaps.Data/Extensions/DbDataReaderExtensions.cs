using System;
using System.Data.Common;

namespace FunderMaps.Data.Extensions
{
    /// <summary>
    ///     DbDataReader extensions.
    /// </summary>
    internal static class DbDataReaderExtensions
    {
        /// <summary>
        ///     Return value as integer.
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
        ///     Return value as unsigned integer.
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

        /// <summary>
        ///     Return value as nullable integer.
        /// </summary>
        /// <param name="reader">Input reader to extend.</param>
        /// <param name="ordinal">Column ordinal.</param>
        /// <returns>Value as nullable integer.</returns>
        public static int? GetSafeInt(this DbDataReader reader, int ordinal)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetInt(ordinal);
        }

        /// <summary>
        ///     Return value as nullable double.
        /// </summary>
        /// <param name="reader">Input reader to extend.</param>
        /// <param name="ordinal">Column ordinal.</param>
        /// <returns>Value as nullable double.</returns>
        public static double? GetSafeDouble(this DbDataReader reader, int ordinal)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetDouble(ordinal);
        }

        /// <summary>
        ///     Return value as nullable float.
        /// </summary>
        /// <param name="reader">Input reader to extend.</param>
        /// <param name="ordinal">Column ordinal.</param>
        /// <returns>Value as nullable float.</returns>
        public static float? GetSafeFloat(this DbDataReader reader, int ordinal)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetFloat(ordinal);
        }

        /// <summary>
        ///     Return value as nullable string.
        /// </summary>
        /// <param name="reader">Input reader to extend.</param>
        /// <param name="ordinal">Column ordinal.</param>
        /// <returns>Value as nullable string.</returns>
        public static string GetSafeString(this DbDataReader reader, int ordinal)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        /// <summary>
        ///     Return value as nullable datetime.
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
        ///     Return value as nullable boolean.
        /// </summary>
        /// <param name="reader">Input reader to extend.</param>
        /// <param name="ordinal">Column ordinal.</param>
        /// <returns>Boolean or null.</returns>
        public static bool? GetSafeBoolean(this DbDataReader reader, int ordinal)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return reader.IsDBNull(ordinal) ? null : (bool?)reader.GetBoolean(ordinal);
        }

        /// <summary>
        ///     Return value as nullable decimal.
        /// </summary>
        /// <param name="reader">Input reader to extend.</param>
        /// <param name="ordinal">Column ordinal.</param>
        /// <returns>decimal or null.</returns>
        public static decimal? GetSafeDecimal(this DbDataReader reader, int ordinal)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return reader.IsDBNull(ordinal) ? null : (decimal?)reader.GetDecimal(ordinal);
        }

        /// <summary>
        ///     Return value as nullable <typeparamref name="TFieldType"/>.
        /// </summary>
        /// <typeparam name="TFieldType">Type to return value to.</typeparam>
        /// <param name="reader">Input reader to extend.</param>
        /// <param name="ordinal">Column ordinal.</param>
        /// <returns>Value or null.</returns>
        public static TFieldType GetSafeFieldValue<TFieldType>(this DbDataReader reader, int ordinal)
            where TFieldType : class
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return reader.IsDBNull(ordinal) ? null : reader.GetFieldValue<TFieldType>(ordinal);
        }
    }
}
