using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.InputModels;
namespace FunderMaps.IntegrationTests
{
    public class AuthFunderMapsWebApplicationFactory<TStartup> : FunderMapsWebApplicationFactory<TStartup>
        where TStartup : class
    {
        private HttpClient _httpClient;
        private UserPair _user;

        public SignInSecurityTokenDto AuthToken { get; private set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthFunderMapsWebApplicationFactory(HttpClient httpClient, UserPair user)
        {
            _httpClient = httpClient;
            _user = user;
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
            => await _httpClient.PostAsJsonGetFromJsonAsync<SignInSecurityTokenDto, SignInInputModel>("api/auth/signin", new()
            {
                Email = _user.User.Email,
                Password = _user.Password,
            });

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken.Token);
        }
    }
}
