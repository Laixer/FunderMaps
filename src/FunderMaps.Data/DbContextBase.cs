﻿using System.Threading.Tasks;
using FunderMaps.Core;
using FunderMaps.Core.Abstractions;
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
    internal abstract class DbContextBase : AppServiceBase
    {
        /// <summary>
        ///     Data provider interface.
        /// </summary>
        public DbProvider DbProvider { get; set; }

        // FUTURE: Move into DI. This becomes the DbContext DbContext
        /// <summary>
        ///     Create the database context.
        /// </summary>
        public virtual async ValueTask<DbContext> DbContextFactory(string cmdText)
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
        /// <param name="alias">Datasource alias.</param>
        protected static void ConstructNavigation(ref string cmdText, INavigation navigation, string alias = null)
        {
            const string lineFeed = "\r\n";

            // FUTURE: Can we improve stability and readability here?
            if (!string.IsNullOrEmpty(navigation.SortColumn))
            {
                var column = alias != null ? $"{alias}.{navigation.SortColumn}" : navigation.SortColumn;
                cmdText += $"{lineFeed} ORDER BY {column} {(navigation.SortOrder == SortOrder.Ascending ? "ASC" : "DESC")}";
            }

            if (navigation.Offset != 0)
            {
                cmdText += $"{lineFeed} OFFSET {navigation.Offset}";
            }

            if (navigation.Limit != 0)
            {
                cmdText += $"{lineFeed} LIMIT {navigation.Limit}";
            }
        }
    }
}