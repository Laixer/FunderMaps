using System.Text.Json;

namespace System.Net.Http.Json;

/// <summary>
///     HttpClient Json extensions.
/// </summary>
public static class HttpClientJsonExtensions
{
    /// <summary>
    ///     Submit object as json data and deserialize return data as object.
    /// </summary>
    /// <typeparam name="TReturnValue">Type of return object.</typeparam>
    /// <typeparam name="TSubmitValue">Type of submit object.</typeparam>
    /// <param name="client">Instance of <see cref="HttpClient"/> to extend.</param>
    /// <param name="requestUri">Remote uri.</param>
    /// <param name="value">Object to submit</param>
    /// <param name="options">Optional <see cref="JsonSerializerOptions"/>.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns></returns>
    public static async Task<TReturnValue?> PostAsJsonGetFromJsonAsync<TReturnValue, TSubmitValue>(this HttpClient client,
        string requestUri,
        TSubmitValue value,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsJsonAsync(requestUri, value, options, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TReturnValue>(options, cancellationToken);
    }
}
