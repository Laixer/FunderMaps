namespace TdmClient.Auth
{
    /// <summary>
    /// Context for <see cref="OAuth1Authenticator"/>.
    /// </summary>
    internal class OAuth1Context
    {
        /// <summary>
        /// OAuth consumer key property.
        /// </summary>
        public string ConsumerKey { get; set; }

        /// <summary>
        /// OAuth consumer secret property.
        /// </summary>
        public string ConsumerSecret { get; set; }

        /// <summary>
        /// Endpoint url for request token.
        /// </summary>
        public string RequestTokenEndpoint { get; internal set; }

        /// <summary>
        /// Endpoint url for access token.
        /// </summary>
        public string AccessTokenEndpoint { get; internal set; }
    }
}
