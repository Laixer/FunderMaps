namespace Laixer.Identity.Dapper.Internal
{
    /// <summary>
    /// Standard query repository containing all the query properties.
    /// </summary>
    internal class DefaultQueryRepository : IQueryRepository
    {
        #region IUserStore
        public string CreateAsync { get; set; }
        public string UpdateAsync { get; set; }
        public string DeleteAsync { get; set; }
        public string FindByIdAsync { get; set; }
        public string FindByNameAsync { get; set; }
        #endregion

        #region IUserEmailStore
        public string FindByEmailAsync { get; set; }
        #endregion

        #region IUserRoleStore
        public string AddToRoleAsync { get; set; }
        public string GetRolesAsync { get; set; }
        public string GetUsersInRoleAsync { get; set; }
        public string RemoveFromRoleAsync { get; set; }
        #endregion
    }
}
