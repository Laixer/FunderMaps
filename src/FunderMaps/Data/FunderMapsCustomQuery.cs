using Laixer.Identity.Dapper;

namespace FunderMaps.Data
{
    /// <summary>
    /// Set the custom queries for FunderMapsUser and FunderMapsRole.
    /// </summary>
    public class FunderMapsCustomQuery : ICustomQueryRepository
    {
        /// <summary>
        /// Configure custom queries.
        /// </summary>
        /// <param name="queryRepository">Exiting query repository.</param>
        public void Configure(IQueryRepository queryRepository)
        {
            queryRepository.GetRolesAsync = $@"
                SELECT role
                FROM application.user
                WHERE id=@Id";
        }
    }
}
