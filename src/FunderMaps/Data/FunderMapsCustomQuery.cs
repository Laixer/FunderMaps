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
            queryRepository.AddToRoleAsync = $@"
                UPDATE application.user
                SET role = @Role
                WHERE id=@Id";

            queryRepository.GetRolesAsync = $@"
                SELECT role
                FROM application.user
                WHERE id=@Id";

            queryRepository.GetUsersInRoleAsync = $@"
                SELECT *
                FROM application.user
                WHERE role=@Role";

            queryRepository.RemoveFromRoleAsync = $@"
                UPDATE application.user
                SET role = 'user'
                WHERE id=@Id";
        }
    }
}
