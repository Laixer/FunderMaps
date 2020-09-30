using System.Threading.Tasks;
using FunderMaps.Core;
using FunderMaps.Core.Interfaces;
using FunderMaps.Data.Providers;

namespace FunderMaps.Data
{
    /// <summary>
    ///     Data abstract base class.
    /// </summary>
    /// <remarks>
    ///     Repositories will be able to inherit from this base.
    /// </remarks>
    internal abstract class DataBase
    {
        /// <summary>
        ///     Data provider interface.
        /// </summary>
        public DbProvider DbProvider { get; set; }

        /// <summary>
        ///     Application context.
        /// </summary>
        public AppContext AppContext { get; set; }

        /// <summary>
        ///     Create the database context.
        /// </summary>
        public async ValueTask<DbContext> DbContextFactory(string cmdText)
        {
            var context = new DbContext()
            {
                DbProvider = DbProvider,
                AppContext = AppContext,
            };
            await context.InitializeAsync(cmdText);
            return context;
        }

        // FUTURE: Maybe to npgsql specific.
        /// <summary>
        ///     Convert navigation to query.
        /// </summary>
        /// <param name="cmdText">SQL query.</param>
        /// <param name="navigation">Navigation instance of type <see cref="INavigation"/>.</param>
        protected static void ConstructNavigation(ref string cmdText, INavigation navigation)
        {
            // FUTURE: Can we improve stability and readability here?
            if (!string.IsNullOrEmpty(navigation.SortColumn))
            {
                cmdText += $"\r\n ORDER BY {navigation.SortColumn} {(navigation.SortOrder == SortOrder.Ascending ? "ASC" : "DESC")}";
            }

            if (navigation.Offset != 0)
            {
                cmdText += $"\r\n OFFSET {navigation.Offset}";
            }

            if (navigation.Limit != 0)
            {
                cmdText += $"\r\n LIMIT {navigation.Limit}";
            }
        }
    }
}
