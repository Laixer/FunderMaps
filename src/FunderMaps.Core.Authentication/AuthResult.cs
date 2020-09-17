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
        Success,

        /// <summary>
        ///     Failed operation.
        /// </summary>
        Failed,

        /// <summary>
        ///     Operation failed because the
        ///     user was locked out.
        /// </summary>
        LockedOut,

        /// <summary>
        ///     Operation failed because the user
        ///     is not allowed to sign-in.
        /// </summary>
        NotAllowed,
    }
}
