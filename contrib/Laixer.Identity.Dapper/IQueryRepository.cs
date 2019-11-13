namespace Laixer.Identity.Dapper
{
    public interface IQueryRepository
    {
        #region IUserStore
        string CreateAsync { get; set; }
        string UpdateAsync { get; set; }
        string DeleteAsync { get; set; }
        string FindByIdAsync { get; set; }
        string FindByNameAsync { get; set; }
        #endregion

        #region IUserEmailStore
        string FindByEmailAsync { get; set; }
        #endregion

        #region IUserRoleStore
        string AddToRoleAsync { get; set; }
        string GetRolesAsync { get; set; }
        string GetUsersInRoleAsync { get; set; }
        string RemoveFromRoleAsync { get; set; }
        #endregion
    }
}
