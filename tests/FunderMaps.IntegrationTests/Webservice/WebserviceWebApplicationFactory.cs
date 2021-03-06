using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.InputModels;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Backend;
using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.Webservice;

namespace FunderMaps.IntegrationTests.Webservice
{
    public class AuthWebserviceWebApplicationFactory : CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient administratorBackendAppClient;
        private readonly HttpClient publicBackendAppClient;

        public SignInSecurityTokenDto AuthToken { get; private set; }
        public OrganizationProposalDto OrganizationProposal { get; private set; }
        public OrganizationSetupDto OrganizationSetup { get; private set; }
        public OrganizationDto Organization { get; private set; }

        public class WebserviceWebApplicationFactory : CustomWebApplicationFactory<Startup>
        {
        }

        public AuthWebserviceWebApplicationFactory()
        {
            administratorBackendAppClient = new AuthBackendWebApplicationFactory()
                .ConfigureAuthentication(options => options.User.Role = ApplicationRole.Administrator)
                .WithAuthenticationStores()
                .CreateClient();

            publicBackendAppClient = new BackendWebApplicationFactory()
                .CreateClient();

            OrganizationProposal = new OrganizationProposalDtoFaker().Generate();
            OrganizationSetup = new OrganizationSetupDtoFaker().Generate();

            ConfigureOrganizationAsync().Wait();
            SignInAsync().Wait();
        }

        protected async virtual Task ConfigureOrganizationAsync()
        {
            OrganizationProposal = await administratorBackendAppClient.PostAsJsonGetFromJsonAsync<OrganizationProposalDto, OrganizationProposalDto>("api/organization/proposal", OrganizationProposal);

            await publicBackendAppClient.PostAsJsonAsync($"api/organization/{OrganizationProposal.Id}/setup", OrganizationSetup);

            Organization = await administratorBackendAppClient.GetFromJsonAsync<OrganizationDto>($"api/admin/organization/{OrganizationProposal.Id}");
        }

        protected async virtual Task SignInAsync()
        {
            AuthToken = await publicBackendAppClient.PostAsJsonGetFromJsonAsync<SignInSecurityTokenDto, SignInInputModel>("api/auth/signin", new()
            {
                Email = OrganizationSetup.Email,
                Password = OrganizationSetup.Password,
            });
        }

        protected async virtual Task TeardownOrganization()
        {
            foreach (var user in await administratorBackendAppClient.GetFromJsonAsync<IList<OrganizationUserDto>>($"api/admin/organization/{Organization.Id}/user"))
            {
                await administratorBackendAppClient.DeleteAsync($"api/admin/organization/{Organization.Id}/user/{user.Id}");
            }
            await administratorBackendAppClient.DeleteAsync($"api/admin/organization/{Organization.Id}");
        }

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken.Token);
        }

        public HttpClient CreateUnauthorizedClient()
            => new WebserviceWebApplicationFactory()
                .CreateClient();

        protected override void Dispose(bool disposing)
        {
            TeardownOrganization().Wait();

            publicBackendAppClient.Dispose();
            administratorBackendAppClient.Dispose();

            base.Dispose(disposing);
        }
    }
}
