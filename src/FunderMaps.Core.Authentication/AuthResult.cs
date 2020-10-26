namespace FunderMaps.Core.Authentication
{
    /// <summary>
    ///     Represents the result authentication operation.
    /// </summary>
    public enum AuthResult
    {
        /// <summary>
        ///     Successful operation.
        /// </summary>
        Success = 0,

        /// <summary>
        ///     Failed operation.
        /// </summary>
        Failed = 1,

        /// <summary>
        ///     Operation failed because the
        ///     user was locked out.
        /// </summary>
        LockedOut = 2,

        /// <summary>
        ///     Operation failed because the user
        ///     is not allowed to sign-in.
        /// </summary>
        NotAllowed = 3,
    }
}
