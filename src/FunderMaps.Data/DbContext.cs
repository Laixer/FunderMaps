using FunderMaps.Core.Exceptions;
using FunderMaps.Data.Providers;
using System.Data.Common;
using System.Runtime.ExceptionServices;

namespace FunderMaps.Data;

/// <summary>
///     Database context.
/// </summary>
internal class DbContext : IAsyncDisposable
{
    private readonly DbProvider _dbProvider;
    private readonly Core.AppContext _appContext;
    private readonly DbConnection _dbConnection;
    private readonly DbCommand _dbCommand;
    private DbDataReader? _dbDataReader;

    /// <summary>
    ///     Construct a new database context.
    /// </summary>
    public DbContext(DbProvider dbProvider, Core.AppContext appContext, string cmdText)
    {
        _dbProvider = dbProvider;
        _appContext = appContext; // TODO: Remove appContext dependency.

        _dbConnection = _dbProvider.ConnectionScope();
        _dbCommand = _dbProvider.CreateCommand(cmdText, _dbConnection);
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

        _dbProvider.HandleException(edi);
    }

    /// <summary>
    ///     Initialize database context.
    /// </summary>
    /// <remarks>
    ///     The database connection scope is rarely ever opened. Thus
    ///     we'll return an explicit valuetask.
    /// </remarks>
    /// <param name="cmdText">The text of the query.</param>
    public static async ValueTask<DbContext> OpenSessionAsync(DbProvider dbProvider, Core.AppContext appContext, string cmdText)
    {
        var context = new DbContext(dbProvider, appContext, cmdText);
        await context.OpenAsync();
        return context;
    }

    public async Task OpenAsync() => await _dbConnection.OpenAsync(_appContext.CancellationToken);

    /// <summary>
    ///     Add parameter with key and value to command.
    /// </summary>
    /// <remarks>
    ///     Sets a database null value if the object value is null.
    /// </remarks>
    /// <param name="parameterName">Parameter name.</param>
    /// <param name="value">Parameter value.</param>
    public void AddParameterWithValue(string parameterName, object? value)
    {
        var parameter = _dbCommand.CreateParameter();

        parameter.ParameterName = parameterName;
        parameter.Value = value ?? DBNull.Value;

        if (value is string && string.IsNullOrEmpty(value as string))
        {
            parameter.Value = DBNull.Value;
        }

        _dbCommand.Parameters.Add(parameter);
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
            _dbDataReader = await _dbCommand.ExecuteReaderAsync(_appContext.CancellationToken);
            if (!_dbDataReader.HasRows && hasRowsGuard)
            {
                throw new EntityNotFoundException();
            }

            if (readAhead)
            {
                await _dbDataReader.ReadAsync(_appContext.CancellationToken);
            }

            return _dbDataReader;
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
            _dbDataReader = await _dbCommand.ExecuteReaderAsync(_appContext.CancellationToken);
            if (!_dbDataReader.HasRows && hasRowsGuard)
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
        while (await _dbDataReader.ReadAsync(_appContext.CancellationToken))
        {
            yield return _dbDataReader;
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
            int affected = await _dbCommand.ExecuteNonQueryAsync(_appContext.CancellationToken);
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

    // TODO: Replace resultGuard with a default value.
    /// <summary>
    ///     Execute command and return scalar result.
    /// </summary>
    /// <param name="resultGuard">Throw if no result was returned.</param>
    public async Task<TResult> ScalarAsync<TResult>(bool resultGuard = true)
        where TResult : new()
    {
        try
        {
            var result = await _dbCommand.ExecuteScalarAsync(_appContext.CancellationToken);
            if (result is null)
            {
                if (resultGuard)
                {
                    throw new EntityNotFoundException();
                }
                return new TResult();
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
        if (_dbDataReader is not null)
        {
            await _dbDataReader.DisposeAsync();
        }

        await _dbCommand.DisposeAsync();
        await _dbConnection.CloseAsync();
    }
}
