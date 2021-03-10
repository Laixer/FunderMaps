using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.IntegrationTests
{
    public record UserPair
    {
        public UserDto User { get; init; }
        public string Password { get; init; }
    }

    public class SetupFunderMapsWebApplicationFactory<TStartup> : FunderMapsWebApplicationFactory<TStartup>
        where TStartup : class
    {
        private HttpClient _httpClient;

        public OrganizationDto Organization { get; private set; }
        public UserPair Superuser { get; private set; }
        public UserPair Verifier { get; private set; }
        public UserPair Writer { get; private set; }
        public UserPair Reader { get; private set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public SetupFunderMapsWebApplicationFactory(HttpClient httpClient)
            => _httpClient = httpClient;

        /// <summary>
        ///     Called immediately after the class has been created, before it is used.
        /// </summary>
        public override async Task InitializeAsync()
        {
            (Organization, Superuser) = await CreateOrganizationAsync();

            Verifier = await CreateUserAsync(Organization, OrganizationRole.Verifier);
            Writer = await CreateUserAsync(Organization, OrganizationRole.Writer);
            Reader = await CreateUserAsync(Organization, OrganizationRole.Reader);
        }

        /// <summary>
        ///     Create organization with superuser.
        /// </summary>
        protected async virtual Task<(OrganizationDto, UserPair)> CreateOrganizationAsync()
        {
            var organizationProposal = new OrganizationProposalDtoFaker().Generate();
            organizationProposal = await _httpClient.PostAsJsonGetFromJsonAsync<OrganizationProposalDto, OrganizationProposalDto>("api/organization/proposal", organizationProposal);

            var organizationSetup = new OrganizationSetupDtoFaker().Generate();
            await _httpClient.PostAsJsonAsync($"api/organization/{organizationProposal.Id}/setup", organizationSetup);

            var organization = await _httpClient.GetFromJsonAsync<OrganizationDto>($"api/admin/organization/{organizationProposal.Id}");

            var organizationUsers = await _httpClient.GetFromJsonAsync<List<UserDto>>($"api/admin/organization/{organization.Id}/user");

            return (organization, new UserPair
            {
                User = organizationUsers.First(),
                Password = organizationSetup.Password,
            });
        }

        /// <summary>
        ///     Create organization user with organization specified role.
        /// </summary>
        protected async virtual Task<UserPair> CreateUserAsync(OrganizationDto organization, OrganizationRole role)
        {
            var organizationUserPassword = new OrganizationUserPasswordDtoFaker()
                .RuleFor(f => f.Role, f => ApplicationRole.User)
                .RuleFor(f => f.OrganizationRole, f => role)
                .Generate();
            var user = await _httpClient.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>($"api/admin/organization/{organization.Id}/user", organizationUserPassword);

            return new UserPair
            {
                User = user,
                Password = organizationUserPassword.Password,
            };
        }

        /// <summary>
        ///     Remove the created objects.
        /// </summary>
        // protected async virtual Task TeardownOrganization()
        // {
        // foreach (var user in await _httpClient.GetFromJsonAsync<IList<OrganizationUserDto>>($"api/admin/organization/{Organization.Id}/user"))
        // {
        //     await _httpClient.DeleteAsync($"api/admin/organization/{Organization.Id}/user/{user.Id}");
        // }
        // await _httpClient.DeleteAsync($"api/admin/organization/{Organization.Id}");
        // }
    }
}
