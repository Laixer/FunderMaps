using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Interfaces;
using FunderMaps.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Sample repository.
    /// </summary>
    public class MapRepository : IMapRepository
    {
        private DbProvider _dbProvider;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public MapRepository(DbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<AddressPoint>> GetByOrganizationIdAsync(Guid orgId)
        {
            using (var connection = _dbProvider.ConnectionScope())
            {
                var sql = @"
                    SELECT addr.street_name,
                           addr.building_number,
                           samp.report,
                           st_x(addr.geopoint) AS x,
                           st_y(addr.geopoint) AS y,
                           st_z(addr.geopoint) AS z
                    FROM   application.sample AS samp
                           INNER JOIN application.address AS addr ON samp.address = addr.id
                           INNER JOIN application.report AS reprt ON samp.report = reprt.id
                           INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                    WHERE  addr.geopoint IS NOT NULL
                           AND (attr.owner = @Owner
                                OR reprt.access_policy = 'public')";

                var result = await connection.QueryAsync<AddressPoint>(sql, new { Owner = orgId });
                if (result.Count() == 0)
                {
                    return null;
                }

                return result.ToArray();
            }
        }
    }
}
