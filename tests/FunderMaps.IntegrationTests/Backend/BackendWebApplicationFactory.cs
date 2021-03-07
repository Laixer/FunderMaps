using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.InputModels;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using FunderMaps.WebApi;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.Extensions.DependencyInjection;

namespace FunderMaps.IntegrationTests.Backend
{
    public class BackendWebApplicationFactory : CustomWebApplicationFactory<Startup> // TODO: remove
    {
    }

    public class AuthBackendWebApplicationFactory : AuthWebApplicationFactory<Startup> // TODO": Change inherit
    {
        public SignInSecurityTokenDto AuthToken { get; private set; }
        public UserPair Superuser { get; private set; }
        public UserPair Verifier { get; private set; }
        public UserPair Writer { get; private set; }
        public UserPair Reader { get; private set; }
        public OrganizationDto Organization { get; private set; }

        private List<OrganizationDto> organizationTrackList = new();

        public record UserPair
        {
            public UserDto User { get; init; }
            public string Password { get; init; }
        }

        public class BackendWebApplicationFactory : CustomWebApplicationFactory<Startup>
        {
        }

        public class AdminWebApplicationFactory : AuthWebApplicationFactory<Startup>
        {
        }

        protected override void ConfigureTestServices(IServiceCollection services) // TODO: Remove
        {
            //
        }

        public override async Task InitializeAsync()
        {
            {
                var (organizationSetup, organization, user) = await CreateOrganizationAsync();
                Organization = organization;
                Superuser = new UserPair
                {
                    User = user,
                    Password = organizationSetup.Password,
                };
            }

            {
                var (organizationUserPassword, user) = await CreateUserAsync(Organization, OrganizationRole.Verifier);
                Verifier = new UserPair
                {
                    User = user,
                    Password = organizationUserPassword.Password,
                };
            }

            {
                var (organizationUserPassword, user) = await CreateUserAsync(Organization, OrganizationRole.Writer);
                Writer = new UserPair
                {
                    User = user,
                    Password = organizationUserPassword.Password,
                };
            }

            {
                var (organizationUserPassword, user) = await CreateUserAsync(Organization, OrganizationRole.Reader);
                Reader = new UserPair
                {
                    User = user,
                    Password = organizationUserPassword.Password,
                };
            }

            AuthToken = await SignInAsync();
        }

        public async virtual Task<(OrganizationSetupDto, OrganizationDto, UserDto)> CreateOrganizationAsync(bool track = true)
        {
            using var administratorClient = CreateAdminClient();

            var organizationProposal = new OrganizationProposalDtoFaker().Generate();
            organizationProposal = await administratorClient.PostAsJsonGetFromJsonAsync<OrganizationProposalDto, OrganizationProposalDto>("api/organization/proposal", organizationProposal);

            using var publicClient = CreateUnauthorizedClient();

            var organizationSetup = new OrganizationSetupDtoFaker().Generate();
            await publicClient.PostAsJsonAsync($"api/organization/{organizationProposal.Id}/setup", organizationSetup);

            var organization = await administratorClient.GetFromJsonAsync<OrganizationDto>($"api/admin/organization/{organizationProposal.Id}");
            if (track)
            {
                organizationTrackList.Add(organization);
            }

            var organizationUsers = await administratorClient.GetFromJsonAsync<List<UserDto>>($"api/admin/organization/{organization.Id}/user");
            return (organizationSetup, organization, organizationUsers.First());
        }

        public async virtual Task<(OrganizationUserPasswordDto, UserDto)> CreateUserAsync(OrganizationDto organization, OrganizationRole role)
        {
            using var client = CreateAdminClient();

            var organizationUserPassword = new OrganizationUserPasswordDtoFaker().Generate();
            organizationUserPassword.OrganizationRole = role;
            var user = await client.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>($"api/admin/organization/{organization.Id}/user", organizationUserPassword);
            return (organizationUserPassword, user);
        }

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

        protected async virtual Task TeardownOrganization()
        {
            using var administratorClient = CreateAdminClient();
            foreach (var organization in organizationTrackList)
            {
                foreach (var user in await administratorClient.GetFromJsonAsync<IList<OrganizationUserDto>>($"api/admin/organization/{organization.Id}/user"))
                {
                    await administratorClient.DeleteAsync($"api/admin/organization/{organization.Id}/user/{user.Id}");
                }
                await administratorClient.DeleteAsync($"api/admin/organization/{organization.Id}");
            }
        }

        public HttpClient CreateAdminClient()
            => new AdminWebApplicationFactory()
                .ConfigureAuthentication(options => options.User.Role = ApplicationRole.Administrator)
                .WithAuthenticationStores()
                .CreateClient();

        public HttpClient CreateUnauthorizedClient()
            => new BackendWebApplicationFactory()
                .CreateClient();

        public override async Task DisposeAsync()
        {
            await TeardownOrganization();
        }
    }
}
