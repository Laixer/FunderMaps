using FunderMaps.AspNetCore.DataTransferObjects;
using System.Net.Http.Headers;

namespace FunderMaps.IntegrationTests
{
    public class AuthFunderMapsWebApplicationFactory<TStartup> : FunderMapsWebApplicationFactory<TStartup>
        where TStartup : class
    {
        private HttpClient _httpClient;
        private string _username;
        private string _password;

        public SignInSecurityTokenDto AuthToken { get; private set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthFunderMapsWebApplicationFactory(HttpClient httpClient, string username, string password)
        {
            _httpClient = httpClient;
            _username = username;
            _password = password;
        }

        /// <summary>
        ///     Called immediately after the class has been created, before it is used.
        /// </summary>
        public override async Task InitializeAsync()
            => AuthToken = await SignInAsync();

        /// <summary>
        ///     Create a user session for the given user.
        /// </summary>
        protected async virtual Task<SignInSecurityTokenDto> SignInAsync()
            => await _httpClient.PostAsJsonGetFromJsonAsync<SignInSecurityTokenDto, SignInDto>("api/auth/signin", new()
            {
                Email = _username,
                Password = _password,
            });

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken.Token);
        }
    }
}
