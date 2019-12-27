namespace TdmClient.Auth
{
    /// <summary>
    /// Session state.
    /// </summary>
    public enum OAuthSessionState
    {
        /// <summary>
        /// No tokens present.
        /// </summary>
        NoToken,

        /// <summary>
        /// Token is expired.
        /// </summary>
        TokenExpired,

        /// <summary>
        /// Token is valid.
        /// </summary>
        TokenValid,
    }
}
