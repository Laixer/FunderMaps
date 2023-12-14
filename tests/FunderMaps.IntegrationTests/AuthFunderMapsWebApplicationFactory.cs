using System.Net.Http.Headers;
using FunderMaps.Core.DataTransferObjects;

namespace FunderMaps.IntegrationTests;

/// <summary>
///     Create new instance.
/// </summary>
public class AuthFunderMapsWebApplicationFactory<TStartup>(HttpClient httpClient, string username, string password) : FunderMapsWebApplicationFactory<TStartup>
    where TStartup : class
{
    public SignInSecurityTokenDto AuthToken { get; private set; } = default!;

    /// <summary>
    ///     Called immediately after the class has been created, before it is used.
    /// </summary>
    public override async Task InitializeAsync()
        => AuthToken = await SignInAsync() ?? throw new InvalidOperationException("Failed to sign in.");

    /// <summary>
    ///     Create a user session for the given user.
    /// </summary>
    protected async virtual Task<SignInSecurityTokenDto?> SignInAsync()
        => await httpClient.PostAsJsonGetFromJsonAsync<SignInSecurityTokenDto, SignInDto>("api/auth/signin", new()
        {
            Email = username,
            Password = password,
        });

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken.Token);
    }
}
