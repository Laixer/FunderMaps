using FunderMaps.Data.Providers;

namespace FunderMaps.Data.Components;

/// <summary>
///     Default <see cref="DbContext"/> factory.
/// </summary>
internal class DbContextFactory(Core.AppContext appContext, DbProvider dbProvider)
{
    public DbProvider DbProvider => dbProvider;

    /// <summary>
    ///     Create and initialize the <see cref="DbContext"/>.
    /// </summary>
    /// <param name="cmdText">The text of the query.</param>
    public virtual ValueTask<DbContext> CreateAsync(string cmdText)
        => DbContext.OpenSessionAsync(dbProvider, appContext, cmdText);
}
