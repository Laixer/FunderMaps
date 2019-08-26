namespace Laixer.Identity.Dapper.Internal
{
    /// <summary>
    /// Standard query repository containing all the query properties.
    /// </summary>
    internal class DefaultQueryRepository : IQueryRepository
    {
        #region IUserStore
        public string CreateAsync { get; set; }
        public string DeleteAsync { get; set; }
        public string FindByIdAsync { get; set; }
        public string FindByNameAsync { get; set; }
        public string GetNormalizedUserNameAsync { get; set; }
        public string GetUserIdAsync { get; set; }
        public string GetUserNameAsync { get; set; }
        public string SetNormalizedUserNameAsync { get; set; }
        public string SetUserNameAsync { get; set; }
        public string UpdateAsync { get; set; }
        #endregion

        #region IUserEmailStore
        public string FindByEmailAsync { get; set; }
        public string GetEmailAsync { get; set; }
        public string GetEmailConfirmedAsync { get; set; }
        public string GetNormalizedEmailAsync { get; set; }
        public string SetEmailAsync { get; set; }
        public string SetEmailConfirmedAsync { get; set; }
        public string SetNormalizedEmailAsync { get; set; }
        #endregion

        #region IUserRoleStore
        public string GetRolesAsync { get; set; }
        #endregion

        #region IUserPasswordStore
        public string GetPasswordHashAsync { get; set; }
        public string SetPasswordHashAsync { get; set; }
        #endregion

        #region IUserSecurityStampStore
        public string GetSecurityStampAsync { get; set; }
        public string SetSecurityStampAsync { get; set; }
        #endregion
    }
}
