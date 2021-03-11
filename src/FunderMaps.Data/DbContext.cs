using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using FunderMaps.Core;
using FunderMaps.Core.Exceptions;
using FunderMaps.Data.Providers;

namespace FunderMaps.Data
{
    /// <summary>
    ///     Database context.
    /// </summary>
    internal class DbContext : IAsyncDisposable
    {
        /// <summary>
        ///     Data provider interface.
        /// </summary>
        public DbProvider DbProvider { get; set; }

        /// <summary>
        ///     Application context.
        /// </summary>
        public Core.AppContext AppContext { get; set; }

        /// <summary>
        ///     Database connection.
        /// </summary>
        public DbConnection Connection { get; private set; }

        /// <summary>
        ///     Database command.
        /// </summary>
        public DbCommand Command { get; private set; }

        private DbDataReader reader;

        /// <summary>
        ///     Database resultset reader.
        /// </summary>
        /// <remarks>
        ///     Resultsets are expensive so if a new reader is set on 
        ///     this context then dispose the current resultset right away.
        /// </remarks>
        public DbDataReader Reader
        {
            get => reader;
            set
            {
                reader?.Dispose();
                reader = value;
            }
        }

        /// <summary>
        ///     Dispatch the exception to the databse provider.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method should not be called directly but only in the context
        ///         of a caught exception. The exception block should still re-throw
        ///         the exception since this handler *cannot* guarantee end of execution.
        ///     </para>
        ///     <para>
        ///         The exception is captured first before its send to a remote
        ///         handler. The <see cref="ExceptionDispatchInfo"/> preserves
        ///         the stacktrace and other location properties when the exception
        ///         is re-thrown.
        ///     </para>
        /// </remarks>
        /// <param name="edi">Captured exception.</param>
        private void HandleException(ExceptionDispatchInfo edi)
        {
            // FUTURE: Log debug

            DbProvider.HandleException(edi);
        }

        /// <summary>
        ///     Initialize database context.
        /// </summary>
        /// <remarks>
        ///     The database connection scope is rarely ever opened. Thus
        ///     we'll return an explicit valuetask.
        /// </remarks>
        /// <param name="cmdText">The text of the query.</param>
        public async ValueTask InitializeAsync(string cmdText)
        {
            Connection = await DbProvider.OpenConnectionScopeAsync(AppContext.CancellationToken);
            Command = DbProvider.CreateCommand(cmdText, Connection);
        }

        /// <summary>
        ///     Add parameter with key and value to command.
        /// </summary>
        /// <remarks>
        ///     Sets a database null value if the object value is null.
        /// </remarks>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">Parameter value.</param>
        public void AddParameterWithValue(string parameterName, object value)
        {
            var parameter = Command.CreateParameter();

            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;

            if (value is string && string.IsNullOrEmpty(value as string))
            {
                parameter.Value = DBNull.Value;
            }

            Command.Parameters.Add(parameter);
        }

        // FUTURE: Do not depend on Npgsql. Too npgsql specific.
        /// <summary>
        ///     Add parameter with key and json value to command.
        /// </summary>
        /// <remarks>
        ///     Sets a database null value if the object value is null.
        /// </remarks>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">Parameter value.</param>
        public void AddJsonParameterWithValue(string parameterName, object value)
        {
            Npgsql.NpgsqlParameter parameter = new(parameterName, NpgsqlTypes.NpgsqlDbType.Jsonb)
            {
                Value = value ?? DBNull.Value
            };

            Command.Parameters.Add(parameter);
        }

        /// <summary>
        ///     Execute command and return a reader.
        /// </summary>
        /// <param name="readAhead">Read to first row.</param>
        /// <param name="hasRowsGuard">Throw if no rows returned.</param>
        public async Task<DbDataReader> ReaderAsync(bool readAhead = true, bool hasRowsGuard = true)
        {
            try
            {
                Reader = await Command.ExecuteReaderAsync(AppContext.CancellationToken);
                if (!Reader.HasRows && hasRowsGuard)
                {
                    throw new EntityNotFoundException();
                }

                if (readAhead)
                {
                    await Reader.ReadAsync(AppContext.CancellationToken);
                }

                return Reader;
            }
            catch (DbException exception)
            {
                HandleException(ExceptionDispatchInfo.Capture(exception));
                throw;
            }
        }

        /// <summary>
        ///     Execute command and return a reader per row.
        /// </summary>
        /// <param name="hasRowsGuard">Throw if no rows returned.</param>
        public async IAsyncEnumerable<DbDataReader> EnumerableReaderAsync(bool hasRowsGuard = false)
        {
            try
            {
                Reader = await Command.ExecuteReaderAsync(AppContext.CancellationToken);
                if (!Reader.HasRows && hasRowsGuard)
                {
                    throw new EntityNotFoundException();
                }
            }
            catch (DbException exception)
            {
                HandleException(ExceptionDispatchInfo.Capture(exception));
                throw;
            }

            // NOTE: An unfortunate consequence of the yield return is the incapability
            //       of running inside a try-catch block. It should be rare for an exception
            //       to occur after the command has been executed, but not impossible.
            while (await Reader.ReadAsync(AppContext.CancellationToken))
            {
                yield return Reader;
            }
        }

        /// <summary>
        ///     Execute command.
        /// </summary>
        /// <param name="affectedGuard">Throw if no rows were affected.</param>
        public async Task NonQueryAsync(bool affectedGuard = true)
        {
            try
            {
                int affected = await Command.ExecuteNonQueryAsync(AppContext.CancellationToken);
                if (affected <= 0 && affectedGuard)
                {
                    throw new EntityNotFoundException();
                }
            }
            catch (DbException exception)
            {
                HandleException(ExceptionDispatchInfo.Capture(exception));
                throw;
            }
        }

        /// <summary>
        ///     Execute command and return scalar result.
        /// </summary>
        /// <param name="resultGuard">Throw if no result was returned.</param>
        public async Task<TResult> ScalarAsync<TResult>(bool resultGuard = true)
        {
            try
            {
                var result = await Command.ExecuteScalarAsync(AppContext.CancellationToken);
                if (result is null)
                {
                    if (resultGuard)
                    {
                        throw new EntityNotFoundException();
                    }
                    return default;
                }

                return (TResult)result;
            }
            catch (DbException exception)
            {
                HandleException(ExceptionDispatchInfo.Capture(exception));
                throw;
            }
        }

        /// <summary>
        ///     Dispose unmanaged resources.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (Reader is not null)
            {
                await Reader.DisposeAsync();
            }

            await Command.DisposeAsync();
            await Connection.DisposeAsync();
        }
    }
}
