﻿using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Repository for testing.
    /// </summary>
    internal sealed class TestRepository : DbServiceBase, ITestRepository
    {
        /// <summary>
        ///     Check if backend is online.
        /// </summary>
        /// <remarks>
        ///     Explicit check on result, not all commands are submitted
        ///     to the database.
        /// </remarks>
        public async Task<bool> IsAlive() // TODO: Rename to IsAliveAsync
        {
            var sql = @"SELECT 1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            return await context.ScalarAsync<int>() == 1;
        }
    }
}
