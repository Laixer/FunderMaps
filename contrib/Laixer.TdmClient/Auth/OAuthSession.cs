using OAuth;
using System;

namespace TdmClient.Auth;

/// <summary>
/// OAuth 1.0 session.
/// </summary>
internal class OAuthSession : OAuthRequest, IOAuthSession
{
    private const int tokenValidInMinutes = 60 * 8;

    /// <summary>
    /// Session state.
    /// </summary>
    public OAuthSessionState State { get => DetermineState(); }

    /// <summary>
    /// Token valid absolute time.
    /// </summary>
    public DateTime ValidUntil { get; set; }

    /// <summary>
    /// OAuth token property.
    /// </summary>
    public override string Token
    {
        get => base.Token; set
        {
            base.Token = value;
            ValidUntil = DateTime.UtcNow.AddMinutes(tokenValidInMinutes);
        }
    }

    /// <summary>
    /// OAuth consumer secret property.
    /// </summary>
    public override string TokenSecret
    {
        get => base.TokenSecret; set
        {
            base.TokenSecret = value;
            ValidUntil = DateTime.UtcNow.AddMinutes(tokenValidInMinutes);
        }
    }

    /// <summary>
    /// Determine session state.
    /// </summary>
    /// <returns><see cref="OAuthSessionState"/>.</returns>
    private OAuthSessionState DetermineState()
    {
        if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(TokenSecret))
        {
            return OAuthSessionState.NoToken;
        }
        else if (DateTime.UtcNow >= ValidUntil)
        {
            return OAuthSessionState.TokenExpired;
        }

        // FUTURE: Place any additional session checks here.

        return OAuthSessionState.TokenValid;
    }
}
