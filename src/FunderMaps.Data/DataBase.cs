using FunderMaps.Core.Interfaces;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System.Threading.Tasks;

namespace FunderMaps.Data
{
    /// <summary>
    ///     Data abstract base class.
    /// </summary>
    /// <remarks>
    ///     Most repositories will be able to inherit from this base.
    /// </remarks>
    internal abstract class DataBase
    {
        /// <summary>
        ///     Data provider interface.
        /// </summary>
        public DbProvider DbProvider { get; }

        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        protected DataBase(DbProvider dbProvider) => DbProvider = dbProvider;

        /// <summary>
        ///     Runs the SQL command and return an unsigned long value.
        /// </summary>
        /// <param name="cmdText">SQL query.</param>
        /// <returns>Return value as ulong.</returns>
        public async ValueTask<ulong> ExecuteScalarUnsignedLongCommandAsync(string cmdText)
        {
            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(cmdText, connection);
            return await cmd.ExecuteScalarUnsignedLongAsync();
        }

        // FUTURE: Maybe to npgsql specific.
        /// <summary>
        ///     Convert navigation to query.
        /// </summary>
        /// <param name="cmdText">SQL query.</param>
        /// <param name="navigation">Navigation instance of type <see cref="INavigation"/>.</param>
        protected static void ConstructNavigation(ref string cmdText, INavigation navigation)
        {
            // TODO: SECURITY: HACK: This is 100% textbook SQLi.
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
