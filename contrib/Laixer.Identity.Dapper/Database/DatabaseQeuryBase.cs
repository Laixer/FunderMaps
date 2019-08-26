using System.Data;

namespace Laixer.Identity.Dapper.Database
{
    internal abstract class DatabaseQeuryBase : IDatabaseDriver
    {
        public abstract IDbConnection GetDbConnection();

        #region IUserStore
        public string CreateAsync { get; protected set; }
        public string DeleteAsync { get; protected set; }
        public string FindByIdAsync { get; protected set; }
        public string FindByNameAsync { get; protected set; }
        public string GetNormalizedUserNameAsync { get; protected set; }
        public string GetUserIdAsync { get; protected set; }
        public string GetUserNameAsync { get; protected set; }
        public string SetNormalizedUserNameAsync { get; protected set; }
        public string SetUserNameAsync { get; protected set; }
        public string UpdateAsync { get; protected set; }
        #endregion

        #region IUserEmailStore
        public string FindByEmailAsync { get; protected set; }
        public string GetEmailAsync { get; protected set; }
        public string GetEmailConfirmedAsync { get; protected set; }
        public string GetNormalizedEmailAsync { get; protected set; }
        public string SetEmailAsync { get; protected set; }
        public string SetEmailConfirmedAsync { get; protected set; }
        public string SetNormalizedEmailAsync { get; protected set; }
        #endregion

        #region IUserPasswordStore
        public string GetPasswordHashAsync { get; protected set; }
        public string SetPasswordHashAsync { get; protected set; }
        #endregion

        #region IUserSecurityStampStore
        public string GetSecurityStampAsync { get; protected set; }
        public string SetSecurityStampAsync { get; protected set; }
        #endregion
    }
}
