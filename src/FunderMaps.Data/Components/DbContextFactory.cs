using System.Threading.Tasks;
using FunderMaps.Data;
using FunderMaps.Data.Providers;

namespace FunderMaps.Core.Components
{
    /// <summary>
    ///     Default <see cref="DbContext"/> factory.
    /// </summary>
    internal class DbContextFactory
    {
        private readonly AppContext _appContext;
        private readonly DbProvider _dbProvider;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public DbContextFactory(AppContext appContext, DbProvider dbProvider)
        {
            _appContext = appContext ?? throw new System.ArgumentNullException(nameof(appContext));
            _dbProvider = dbProvider ?? throw new System.ArgumentNullException(nameof(dbProvider));
        }

        /// <summary>
        ///     Create the <see cref="DbContext"/>.
        /// </summary>
        public virtual DbContext CreateAsync()
            => new()
            {
                DbProvider = _dbProvider,
                AppContext = _appContext,
            };

        /// <summary>
        ///     Create and initialize the <see cref="DbContext"/>.
        /// </summary>
        /// <param name="cmdText">The text of the query.</param>
        public virtual async ValueTask<DbContext> CreateAsync(string cmdText)
        {
            DbContext context = CreateAsync();
            await context.InitializeAsync(cmdText);
            return context;
        }
    }
}
