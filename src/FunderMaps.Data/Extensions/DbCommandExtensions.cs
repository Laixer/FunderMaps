using FunderMaps.Core.Exceptions;
using System;
using System.Data.Common;
using System.Globalization;
using System.Threading.Tasks;

namespace FunderMaps.Data.Extensions
{
    internal static class DbCommandExtensions
    {
        /// <summary>
        ///     Add parameter with key and value to command.
        /// </summary>
        /// <remarks>
        ///     Will send a null if the value is null.
        /// </remarks>
        /// <param name="command">The command to extend.</param>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">Value.</param>
        /// <returns>See <see cref="DbParameter"/>.</returns>
        public static DbParameter AddParameterWithValue(this DbCommand command, string parameterName, object value)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;
            if (value is string && string.IsNullOrEmpty(value as string))
            {
                parameter.Value = DBNull.Value;
            }
            command.Parameters.Add(parameter);
            return parameter;
        }

        // FUTURE: Do not depend on Npgsql. Too npgsql specific.
        public static DbParameter AddJsonParameterWithValue(this DbCommand command, string parameterName, object value)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var parameter = new Npgsql.NpgsqlParameter(parameterName, NpgsqlTypes.NpgsqlDbType.Jsonb)
            {
                Value = value ?? DBNull.Value
            };
            command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        ///     Executes the query and returns the first column of the first row as integer.
        /// </summary>
        /// <param name="command">The command to extend.</param>
        /// <returns>Integer.</returns>
        public static async ValueTask<int> ExecuteScalarIntAsync(this DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return Convert.ToInt32(await command.ExecuteScalarEnsureRowAsync(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Executes the query and returns the first column of the first row as unsigned integer.
        /// </summary>
        /// <param name="command">The command to extend.</param>
        /// <returns>Unsigned integer.</returns>
        public static async ValueTask<uint> ExecuteScalarUnsignedIntAsync(this DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return Convert.ToUInt32(await command.ExecuteScalarEnsureRowAsync(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Executes the query and returns the first column of the first row as long integer.
        /// </summary>
        /// <param name="command">The command to extend.</param>
        /// <returns>Long integer.</returns>
        public static async ValueTask<long> ExecuteScalarLongAsync(this DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return Convert.ToInt64(await command.ExecuteScalarEnsureRowAsync(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Executes the query and returns the first column of the first row as unsigned long integer.
        /// </summary>
        /// <param name="command">The command to extend.</param>
        /// <returns>Unsigned long integer.</returns>
        public static async ValueTask<ulong> ExecuteScalarUnsignedLongAsync(this DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return Convert.ToUInt64(await command.ExecuteScalarEnsureRowAsync(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Execute command and ensure success.
        /// </summary>
        /// <param name="command">The command to extend.</param>
        /// <returns>Scalar result.</returns>
        public static async ValueTask<object> ExecuteScalarEnsureRowAsync(this DbCommand command)
        {
            var result = await command.ExecuteScalarAsync();
            if (result == null)
            {
                throw new EntityNotFoundException();
            }
            return result;
        }

        /// <summary>
        ///     Execute command and ensure success.
        /// </summary>
        /// <param name="command">The command to extend.</param>
        /// <returns><see cref="DbDataReader"/>.</returns>
        public static async ValueTask<DbDataReader> ExecuteReaderAsyncEnsureRowAsync(this DbCommand command)
        {
            DbDataReader reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows)
            {
                throw new EntityNotFoundException();
            }
            return reader;
        }

        /// <summary>
        ///     Execute command without requiring success.
        /// </summary>
        /// <param name="command">The command to extend.</param>
        /// <returns><see cref="DbDataReader"/>.</returns>
        public static async ValueTask<DbDataReader> ExecuteReaderCanHaveZeroRowsAsync(this DbCommand command)
            => await command.ExecuteReaderAsync();

        /// <summary>
        ///     Execute command and ensure success.
        /// </summary>
        /// <param name="command">The command to extend.</param>
        public static async ValueTask ExecuteNonQueryEnsureAffectedAsync(this DbCommand command)
        {
            int affected = await command.ExecuteNonQueryAsync();
            if (affected <= 0)
            {
                throw new EntityNotFoundException();
            }
        }
    }
}
