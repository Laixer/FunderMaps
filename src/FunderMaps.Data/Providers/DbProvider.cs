using Microsoft.Extensions.Options;
using System.Data.Common;
using System.Runtime.ExceptionServices;

namespace FunderMaps.Data.Providers;

/// <summary>
///     Database provider.
/// </summary>
public abstract class DbProvider
{
    protected readonly DbProviderOptions _options;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public DbProvider(IOptions<DbProviderOptions> options)
        => _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

    /// <summary>
    ///     Create a new connection instance.
    /// </summary>
    /// <returns><see cref="DbConnection"/> instance.</returns>
    public abstract DbConnection ConnectionScope();

    /// <summary>
    ///     Get the connection as URI.
    /// </summary>
    public abstract string ConnectionUri { get; }

    /// <summary>
    ///     Create command on the database connection.
    /// </summary>
    /// <param name="cmdText">The text of the query.</param>
    /// <param name="connection">Database connection, see <see cref="DbConnection"/>.</param>
    /// <returns>See <see cref="DbCommand"/>.</returns>
    public virtual DbCommand CreateCommand(string cmdText, DbConnection connection)
    {
        if (connection is null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        var cmd = connection.CreateCommand();
        cmd.CommandText = cmdText;
        return cmd;
    }

    /// <summary>
    ///     Handle database exception.
    /// </summary>
    /// <param name="edi">Captured exception.</param>
    internal virtual void HandleException(ExceptionDispatchInfo edi) => edi.Throw();
}
