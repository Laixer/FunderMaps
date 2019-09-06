namespace Laixer.Identity.Dapper
{
    public interface IQueryRepository
    {
        #region IUserStore
        string CreateAsync { get; set; }
        string DeleteAsync { get; set; }
        string FindByIdAsync { get; set; }
        string FindByNameAsync { get; set; }
        string GetNormalizedUserNameAsync { get; set; }
        string GetUserIdAsync { get; set; }
        string GetUserNameAsync { get; set; }
        string SetNormalizedUserNameAsync { get; set; }
        string SetUserNameAsync { get; set; }
        string UpdateAsync { get; set; }
        #endregion

        #region IUserEmailStore
        string FindByEmailAsync { get; set; }
        string GetEmailAsync { get; set; }
        string GetEmailConfirmedAsync { get; set; }
        string GetNormalizedEmailAsync { get; set; }
        string SetEmailAsync { get; set; }
        string SetEmailConfirmedAsync { get; set; }
        string SetNormalizedEmailAsync { get; set; }
        #endregion

        #region IUserRoleStore
        string AddToRoleAsync { get; set; }
        string GetRolesAsync { get; set; }
        string GetUsersInRoleAsync { get; set; }
        string RemoveFromRoleAsync { get; set; }
        #endregion

        #region IUserPasswordStore
        string GetPasswordHashAsync { get; set; }
        string SetPasswordHashAsync { get; set; }
        #endregion

        #region IUserSecurityStampStore
        string GetSecurityStampAsync { get; set; }
        string SetSecurityStampAsync { get; set; }
        #endregion
    }
}
