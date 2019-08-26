using System.Data;

namespace Laixer.Identity.Dapper.Database
{
    /// <summary>
    /// Interface for database drivers.
    /// </summary>
    public interface IDatabaseDriver
    {
        /// <summary>
        /// Get the database connection.
        /// </summary>
        /// <returns>Instance of <see cref="IDbConnection"/>.</returns>
        IDbConnection GetDbConnection();

        #region IUserStore
        string CreateAsync { get; }
        string DeleteAsync { get; }
        string FindByIdAsync { get; }
        string FindByNameAsync { get; }
        string GetNormalizedUserNameAsync { get; }
        string GetUserIdAsync { get; }
        string GetUserNameAsync { get; }
        string SetNormalizedUserNameAsync { get; }
        string SetUserNameAsync { get; }
        string UpdateAsync { get; }
        #endregion

        #region IUserEmailStore
        string FindByEmailAsync { get; }
        string GetEmailAsync { get; }
        string GetEmailConfirmedAsync { get; }
        string GetNormalizedEmailAsync { get; }
        string SetEmailAsync { get; }
        string SetEmailConfirmedAsync { get; }
        string SetNormalizedEmailAsync { get; }
        #endregion

        #region IUserRoleStore
        string GetRolesAsync { get; }
        #endregion

        #region IUserPasswordStore
        string GetPasswordHashAsync { get; }
        string SetPasswordHashAsync { get; }
        #endregion

        #region IUserSecurityStampStore
        string GetSecurityStampAsync { get; }
        string SetSecurityStampAsync { get; }
        #endregion
    }
}
