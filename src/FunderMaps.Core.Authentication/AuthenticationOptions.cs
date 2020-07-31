namespace FunderMaps.Core.Authentication
{
    /// <summary>
    ///     Represents all the options you can use to configure the authentication system.
    /// </summary>
    public class AuthenticationOptions
    {
        /// <summary>
        ///     Gets or sets the <see cref="PasswordOptions"/> for the identity system.
        /// </summary>
        /// <value>
        ///     The <see cref="PasswordOptions"/> for the identity system.
        /// </value>
        public PasswordOptions Password { get; set; } = new PasswordOptions();

        /// <summary>
        ///     Gets or sets the <see cref="LockoutOptions"/> for the identity system.
        /// </summary>
        /// <value>
        ///     The <see cref="LockoutOptions"/> for the identity system.
        /// </value>
        public LockoutOptions Lockout { get; set; } = new LockoutOptions();

        /// <summary>
        ///     Gets or sets the <see cref="SignInOptions"/> for the identity system.
        /// </summary>
        /// <value>
        ///     The <see cref="SignInOptions"/> for the identity system.
        /// </value>
        public SignInOptions SignIn { get; set; } = new SignInOptions();
    }
}
