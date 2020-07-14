using FunderMaps.Data.Exceptions;
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

        // TODO: Do not depend on Npgsql.
        // TODO: Refactor.
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

        public static async ValueTask<int> ExecuteScalarIntAsync(this DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return Convert.ToInt32(await command.ExecuteScalarAsync().ConfigureAwait(false), CultureInfo.InvariantCulture);
        }

        public static async ValueTask<uint> ExecuteScalarUnsignedIntAsync(this DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return Convert.ToUInt32(await command.ExecuteScalarAsync().ConfigureAwait(false), CultureInfo.InvariantCulture);
        }

        public static async ValueTask<long> ExecuteScalarLongAsync(this DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return Convert.ToInt64(await command.ExecuteScalarAsync().ConfigureAwait(false), CultureInfo.InvariantCulture);
        }

        public static async ValueTask<ulong> ExecuteScalarUnsignedLongAsync(this DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return Convert.ToUInt64(await command.ExecuteScalarAsync().ConfigureAwait(false), CultureInfo.InvariantCulture);
        }

        public static async ValueTask<DbDataReader> ExecuteReaderAsyncEnsureRowAsync(this DbCommand command)
        {
            var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
            if (!reader.HasRows)
            {
                throw new NullRowException();
            }
            return reader;
        }

        public static async ValueTask ExecuteNonQueryEnsureAffectedAsync(this DbCommand command)
        {
            int affected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            if (affected <= 0)
            {
                throw new NullAffectedException();
            }
        }
    }
}
