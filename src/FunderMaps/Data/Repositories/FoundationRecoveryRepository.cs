using Dapper;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;
using FunderMaps.Interfaces;
using FunderMaps.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Foundation recovery repository.
    /// </summary>
    public class FoundationRecoveryRepository : RepositoryBase<FoundationRecovery, int>, IFoundationRecoveryRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public FoundationRecoveryRepository(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

        public override Task<FoundationRecovery> GetByIdAsync(int id)
        {
            // TODO:

            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Admin function. Returns ALL the records.
        /// </summary>
        /// <param name="navigation">The navigation paramters for offsetting en limiting.</param>
        /// <returns>List of records.</returns>
        public override async Task<IReadOnlyList<FoundationRecovery>> ListAllAsync(Navigation navigation)
        {
            var sql = @"
                SELECT * 
                FROM report.foundation_recovery 
                WHERE delete_date is NULL
                ORDER BY create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<FoundationRecovery>(sql, navigation));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Admin function. Returns ALL the records.
        /// </summary>
        /// <param name="org_id">The id of the organization.</param>
        /// <param name="navigation">The navigation paramters for offsetting en limiting.</param>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<FoundationRecovery>> ListAllAsync(int org_id, Navigation navigation)
        {
            var sql = @"
                SELECT * 
                FROM report.foundation_recovery 
                WHERE attribution = @Owner 
                AND delete_date is NULL
                ORDER BY create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<FoundationRecovery>(sql, new { Owner = org_id, navigation.Offset, navigation.Limit }));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        public override Task<FoundationRecovery> AddAsync(FoundationRecovery entity)
        {
            // TODO:

            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Updates the delete date of a record.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public override Task DeleteAsync(FoundationRecovery entity)
        {
            string sql = @"
                UPDATE report.foundation_recovery
                SET delete_date = CURRENT_TIMESTAMP
                WHERE id = @Id
                AND delete_date IS NULL";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }

        public override Task UpdateAsync(FoundationRecovery entity)
        {
            throw new System.NotImplementedException();
        }

        public override Task<uint> CountAsync()
        {
            // TODO:

            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Get the amount of recovery reports based on the organization id.
        /// </summary>
        /// <param name="org_id"></param>
        /// <returns></returns>
        public Task<uint> CountAsync(int org_id)
        {
            var sql = @"
                SELECT COUNT(*)
                FROM report.foundation_recovery
                WHERE attribution = @Attribution";

            return RunSqlCommand(async cnn => await cnn.ExecuteScalarAsync<uint>(sql, new { Attribution = org_id }));
        }
    }
}
