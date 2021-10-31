using OAuth;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using TdmClient.Exceptions;
using TdmClient.Extensions;

namespace TdmClient.Auth;

/// <summary>
/// OAuth 1.0 authenticator for RestSharp.
/// </summary>
/// <remarks><see cref="IAuthenticator"/>.</remarks>
internal class OAuth1Authenticator : IAuthenticator
{
    private readonly OAuth1Context context;
    private readonly IOAuthSession authSession = new OAuthSession();

    private readonly object stateLock = new object();

    /// <summary>
    /// Create new instance.
    /// </summary>
    /// <param name="oAuth1Context">Context, see <see cref="OAuth1Context"/>.</param>
    public OAuth1Authenticator(OAuth1Context oAuth1Context) => context = oAuth1Context;

    /// <summary>
    /// Negotiate long term resource access tokens.
    /// </summary>
    private async Task SetAccessTokenAsync()
    {
        string tempToken;
        string tempTokenSecret;
        string tempVerifier;

        await Task.Yield();

        // Check before advance to next step.
        if (string.IsNullOrEmpty(context.ConsumerKey) || string.IsNullOrEmpty(context.ConsumerSecret))
        {
            throw new AuthenticationException($"{nameof(context.ConsumerKey)} or {nameof(context.ConsumerSecret)} not set");
        }

        // Step 1: Fetch temporary token.
        using (HttpClientHandler httpClientHandler = new HttpClientHandler())
        {
            httpClientHandler.AllowAutoRedirect = false;

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                var authRequest = OAuthRequest.ForRequestToken(context.ConsumerKey, context.ConsumerSecret);
                authRequest.Method = "POST";
                authRequest.RequestUrl = context.RequestTokenEndpoint;
                authRequest.CallbackUrl = "https://example.org/verification";

                httpClient.DefaultRequestHeaders.Add("Authorization", authRequest.GetAuthorizationHeader());

                using (var result = await httpClient.PostAsync(authRequest.RequestUrl, null).ConfigureAwait(false))
                {
                    result.EnsureSuccessStatusCode();

                    var resultQueryString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var queryPart = HttpUtility.ParseQueryString(resultQueryString);

                    tempToken = queryPart.Get("oauth_token");
                    tempTokenSecret = queryPart.Get("oauth_token_secret");
                }
            }
        }

        // Check before advance to next step.
        if (string.IsNullOrEmpty(tempToken) || string.IsNullOrEmpty(tempTokenSecret))
        {
            throw new AuthenticationException($"{nameof(tempToken)} or {nameof(tempTokenSecret)} not set");
        }

        // Step 2: Redirect to callback and intercept validation code.
        // NOTE: This step is supposed to be handled by the resource owner (user) according to the
        //       OAuth 1.0 specification. The NVM documentation makes mention of 2-legged OAuth
        //       but no example is given. The 3-legged authentication handler server is tricked in
        //       believing a resource owner confirmed the resource claim (see Step 2 OAuth 1.0).
        //       This works because no callback was registered by the authentication handler server.
        //       ** THIS CAN CHANGE ANY TIME ON THE REMOTE SERVER, IN WHICH CASE THIS STEP MIGHT FAIL **
        using (HttpClientHandler httpClientHandler = new HttpClientHandler())
        {
            httpClientHandler.AllowAutoRedirect = false;

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                Uri ExtractLocationUri(HttpResponseHeaders keyValues)
                {
                    if (keyValues.Contains("Location"))
                    {
                        return new Uri(keyValues.GetValues("Location").FirstOrDefault());
                    }
                    else if (keyValues.Contains("location"))
                    {
                        return new Uri(keyValues.GetValues("location").FirstOrDefault());
                    }

                    return null;
                }

                using (var redirectCallbackUrl = await httpClient.GetAsync($"{context.RequestTokenEndpoint}?oauth_token={tempToken}").ConfigureAwait(false))
                {
                    if (redirectCallbackUrl.StatusCode != System.Net.HttpStatusCode.Found)
                    {
                        throw new AuthenticationException("Callback redirect status other than found");
                    }

                    var redirectLocation = ExtractLocationUri(redirectCallbackUrl.Headers);
                    if (redirectLocation == null)
                    {
                        throw new AuthenticationException("Redirect did not contain uri location");
                    }

                    var queryPart = HttpUtility.ParseQueryString(redirectLocation.Query);

                    tempVerifier = queryPart.Get("oauth_verifier");
                }
            }
        }

        // Check before advance to next step.
        if (string.IsNullOrEmpty(tempVerifier))
        {
            throw new AuthenticationException($"{nameof(tempVerifier)} not set");
        }

        // Step 3: Use validation code to retrieve long lived token.
        using (HttpClientHandler httpClientHandler = new HttpClientHandler())
        {
            httpClientHandler.AllowAutoRedirect = false;

            using (var httpClient = new HttpClient())
            {
                var authRequest = OAuthRequest.ForAccessToken(context.ConsumerKey, context.ConsumerSecret, tempToken, tempTokenSecret);
                authRequest.Method = "POST";
                authRequest.RequestUrl = context.AccessTokenEndpoint;
                authRequest.Verifier = tempVerifier;

                httpClient.DefaultRequestHeaders.Add("Authorization", authRequest.GetAuthorizationHeader());

                using (var result = await httpClient.PostAsync(authRequest.RequestUrl, null).ConfigureAwait(false))
                {
                    result.EnsureSuccessStatusCode();

                    var resultQueryString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var queryPart = HttpUtility.ParseQueryString(resultQueryString);

                    authSession.Token = queryPart.Get("oauth_token");
                    authSession.TokenSecret = queryPart.Get("oauth_token_secret");
                }
            }
        }

        // Check before return.
        if (authSession.State != OAuthSessionState.TokenValid)
        {
            throw new AuthenticationException("Current token is invalid");
        }
    }

    public void Authenticate(IRestClient client, IRestRequest request)
    {
        int errorCount = 0;
        Exception lastException = null;

        // Prevent data race on auth session. Multiple instances can access the same
        // authentication state at the same time.
        lock (stateLock)
        {
            for (int i = 0; i < 5; ++i)
            {
                try
                {
                    if (authSession.State == OAuthSessionState.NoToken ||
                        authSession.State == OAuthSessionState.TokenExpired)
                    {
                        // NOTE: This operation does not need to sync back state to the context so the task
                        //       can be blocking-awaited. Exceptions are caught either way.
                        SetAccessTokenAsync().Wait();
                    }

                    var authRequest = OAuthRequest.ForProtectedResource("GET",
                         context.ConsumerKey,
                         context.ConsumerSecret,
                         authSession.Token,
                         authSession.TokenSecret);
                    authRequest.RequestUrl = new Uri(client.BaseUrl, request.Resource).ToString();

                    var parameterDictionary = new Dictionary<string, string>();
                    foreach (var parameter in request.Parameters)
                    {
                        parameterDictionary.Add(parameter.Name, parameter.Value.ToString());
                    }

                    request.AddQueryParameterRange(HttpUtility.ParseQueryString(authRequest.GetAuthorizationQuery(parameterDictionary)));

                    lastException = null;
                    break;
                }
                catch (AggregateException e)
                {
                    ++errorCount;
                    lastException = e.InnerException;
                }
            }
        }

        // Rethrow the exception and let the authentication fail.
        if (lastException != null)
        {
            throw lastException;
        }
    }
}
