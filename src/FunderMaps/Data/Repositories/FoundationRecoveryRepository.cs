using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunderMaps.Interfaces;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;
using Dapper;
using FunderMaps.Providers;

namespace FunderMaps.Data.Repositories
{
    public class FoundationRecoveryRepository : EfRepository<FisDbContext, FoundationRecovery>, IFoundationRecoveryRepository
    {
        private readonly FisDbContext _context; // something?
        private readonly DbProvider _dbProvider;

        public FoundationRecoveryRepository(FisDbContext context, DbProvider dbProvider)
            : base(context)
        {
            _context = context;
            _dbProvider = dbProvider; // this one is used as the connetion database
        }

        /// <summary>
        /// Get the amount of recovery reports based on the organization id.
        /// </summary>
        /// <param name="org_id"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(int org_id)
        {
            using (var connection = _dbProvider.ConnectionScope())
            {
                var sql = @"
                            SELECT COUNT(id) 
                            FROM report.foundation_recovery
                            WHERE attribution = @Attribution";

                // Parameters for the query
                var parameters = new { Attribution = org_id };

                return await connection.ExecuteScalarAsync<int>(sql, parameters);
            }
        }


        /// <summary>
        /// Admin function. Returns ALL the records
        /// </summary>
        /// <param name="navigation">The navigation paramters for offsetting en limitingg</param>
        /// <returns></returns>
        public async Task<IReadOnlyList<FoundationRecovery>> ListAllAsync(Navigation navigation)
        {
            using (var connection = _dbProvider.ConnectionScope())
            {
                var sql = @"
                            SELECT * 
                            FROM report.foundation_recovery 
                            WHERE delete_date is NULL
                            ORDER BY create_date DESC
                            OFFSET @Offset
                            LIMIT @Limit";

                // Store the result.
                var result = await connection.QueryAsync<FoundationRecovery>(sql, navigation);

                // Check if there is something returned
                if (result.Count() == 0)
                {
                    // If the count equals 0, we then return null
                    return null;
                }
                // return the complete list of stuff that the query has returned
                return result.ToArray();
            }
        }

        /// <summary>
        /// Admin function. Returns ALL the records
        /// </summary>
        /// <param name="org_id">The id of the organization</param>
        /// <param name="navigation">The navigation paramters for offsetting en limitingg</param>
        /// <returns></returns>
        public async Task<IReadOnlyList<FoundationRecovery>> ListAllAsync(int org_id, Navigation navigation)
        {
            using (var connection = _dbProvider.ConnectionScope())
            {
                var sql = @"
                            SELECT * 
                            FROM report.foundation_recovery 
                            WHERE attribution = @Id 
                            AND delete_date is NULL
                            ORDER BY create_date DESC
                            OFFSET @Offset
                            LIMIT @Limit";

                // Parameters
                var parameters = new { Id = org_id, navigation.Limit, navigation.Offset };

                // Store the result
                var result = await connection.QueryAsync<FoundationRecovery>(sql, parameters);

                // Check if there is something returned
                if (result.Count() == 0)
                {
                    // If the count equals 0, we then return null
                    return null;
                }
                // return the complete list of stuff that the query has returned
                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates the delete date of a record
        /// </summary>
        /// <param name="input">entitiy to delete</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(FoundationRecovery input)
        {
            using(var connection = _dbProvider.ConnectionScope())
            {
                string sql = @"
                        UPDATE report.foundation_recovery
                        SET delete_date = CURRENT_TIMESTAMP
                        WHERE id = @Id
                        AND delete_date IS NULL";

                // yeet to the database
                return await connection.ExecuteAsync(sql, input);
            }
        }
    }
}
