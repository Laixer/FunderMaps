using System;

namespace TdmClient.Auth
{
    /// <summary>
    /// OAuth 1.0 session.
    /// </summary>
    public interface IOAuthSession
    {
        /// <summary>
        /// OAuth version property.
        /// </summary>
        string Version { get; set; }

        /// <summary>
        /// OAuth callback url property.
        /// </summary>
        string CallbackUrl { get; set; }

        /// <summary>
        /// OAuth verifier property.
        /// </summary>
        string Verifier { get; set; }

        /// <summary>
        /// OAuth token secret property.
        /// </summary>
        string TokenSecret { get; set; }

        /// <summary>
        /// OAuth token property.
        /// </summary>
        string Token { get; set; }

        /// <summary>
        /// OAuth consumer secret property.
        /// </summary>
        string ConsumerSecret { get; set; }

        /// <summary>
        /// OAuth consumer key property.
        /// </summary>
        string ConsumerKey { get; set; }

        /// <summary>
        /// OAuth realm property.
        /// </summary>
        string Realm { get; set; }

        /// <summary>
        /// Token valid absolute time.
        /// </summary>
        DateTime ValidUntil { get; set; }

        /// <summary>
        /// Session state.
        /// </summary>
        OAuthSessionState State { get; }
    }
}
