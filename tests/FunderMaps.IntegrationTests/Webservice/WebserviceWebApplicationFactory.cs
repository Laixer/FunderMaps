using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.InputModels;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Backend;
using FunderMaps.Webservice;
using static FunderMaps.IntegrationTests.Backend.AuthBackendWebApplicationFactory;

namespace FunderMaps.IntegrationTests.Webservice
{
    public class AuthWebserviceWebApplicationFactory : CustomWebApplicationFactory<Startup>
    {
        private readonly AuthBackendWebApplicationFactory backendAppClient = new();

        public SignInSecurityTokenDto AuthToken { get; private set; }
        public UserPair Superuser => backendAppClient.Superuser;
        public UserPair Verifier => backendAppClient.Verifier;
        public UserPair Writer => backendAppClient.Writer;
        public UserPair Reader => backendAppClient.Reader;
        public OrganizationDto Organization { get; private set; }

        public class WebserviceWebApplicationFactory : CustomWebApplicationFactory<Startup>
        {
        }

        public class AdminWebserviceWebApplicationFactory : AuthWebApplicationFactory<Startup>
        {
        }

        public AuthWebserviceWebApplicationFactory()
        {
        }

        public override async Task InitializeAsync()
        {
            await backendAppClient.InitializeAsync();

            AuthToken = await SignInAsync();
        }

        // protected async virtual Task ConfigureOrganizationAsync()
        // {
        //     OrganizationProposal = await administratorBackendAppClient.PostAsJsonGetFromJsonAsync<OrganizationProposalDto, OrganizationProposalDto>("api/organization/proposal", OrganizationProposal);

        //     await publicBackendAppClient.PostAsJsonAsync($"api/organization/{OrganizationProposal.Id}/setup", OrganizationSetup);

        //     Organization = await administratorBackendAppClient.GetFromJsonAsync<OrganizationDto>($"api/admin/organization/{OrganizationProposal.Id}");
        // }

        protected async virtual Task<SignInSecurityTokenDto> SignInAsync()
        {
            using var publicClient = CreateUnauthorizedClient();

            return await publicClient.PostAsJsonGetFromJsonAsync<SignInSecurityTokenDto, SignInInputModel>("api/auth/signin", new()
            {
                Email = Superuser.User.Email,
                Password = Superuser.Password,
            });
        }

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken.Token);
        }

        public HttpClient CreateAdminClient()
            => new AdminWebserviceWebApplicationFactory()
                .ConfigureAuthentication(options => options.User.Role = ApplicationRole.Administrator)
                .WithAuthenticationStores()
                .CreateClient();

        public HttpClient CreateUnauthorizedClient()
            => new WebserviceWebApplicationFactory()
                .CreateClient();

        protected override void Dispose(bool disposing)
        {
            backendAppClient.Dispose();

            base.Dispose(disposing);
        }

        public override async Task DisposeAsync()
        {
            await backendAppClient.DisposeAsync();
        }
    }
}
