using FunderMaps.Data.Providers;

namespace FunderMaps.Data.Components;

internal class DbContextFactory(Core.AppContext appContext, DbProvider dbProvider)
{
    public DbProvider DbProvider => dbProvider;

    public virtual ValueTask<DbContext> CreateAsync(string cmdText)
        => DbContext.OpenSessionAsync(dbProvider, appContext, cmdText);
}
