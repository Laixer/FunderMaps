using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Testing.Faker;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationUserAdminTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationUserAdminTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task OrganizationUserLifeCycle()
        {
            var organizationProposal = await ApplicationStub.CreateProposalAsync(Factory);
            var organizationSetup = await ApplicationStub.CreateOrganizationAsync(Factory, organizationProposal);
            var organization = await ApplicationStub.GetOrganizationAsync(Factory, organizationProposal);
            var organizationUser = await ApplicationStub.CreateOrganizationUserAsync(Factory, organization);
            await TestStub.LoginAsync(Factory, organizationUser.Email, organizationUser.Password);

            {
                // Arrange
                using var client = Factory.CreateAdminClient();

                // Act
                var response = await client.GetAsync($"api/admin/organization/{organization.Id}/user");
                var returnList = await response.Content.ReadFromJsonAsync<List<UserDto>>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(returnList.Count >= 1);
            }

            {
                // Arrange
                using var client = Factory.CreateAdminClient();

                // Act
                var response = await client.PutAsJsonAsync($"api/admin/organization/{organization.Id}/user/{organizationUser.Id}", new UserDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            await ApplicationStub.DeleteOrganizationUserAsync(Factory, organization, organizationUser);
            await ApplicationStub.DeleteOrganizationAsync(Factory, organization);
        }

        [Fact]
        public async Task OrganizationUserLifeCycleChangeRole()
        {
            var organizationProposal = await ApplicationStub.CreateProposalAsync(Factory);
            var organizationSetup = await ApplicationStub.CreateOrganizationAsync(Factory, organizationProposal);
            var organization = await ApplicationStub.GetOrganizationAsync(Factory, organizationProposal);
            var organizationUser = await ApplicationStub.CreateOrganizationUserAsync(Factory, organization);
            await TestStub.LoginAsync(Factory, organizationUser.Email, organizationUser.Password);

            {
                // Arrange
                using var client = Factory.CreateAdminClient();

                // Act
                var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user/{organizationUser.Id}/change-organization-role", new ChangeOrganizationRoleDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            await ApplicationStub.DeleteOrganizationUserAsync(Factory, organization, organizationUser);
            await ApplicationStub.DeleteOrganizationAsync(Factory, organization);
        }

        [Fact]
        public async Task OrganizationUserLifeCycleChangePassword()
        {
            var organizationProposal = await ApplicationStub.CreateProposalAsync(Factory);
            var organizationSetup = await ApplicationStub.CreateOrganizationAsync(Factory, organizationProposal);
            var organization = await ApplicationStub.GetOrganizationAsync(Factory, organizationProposal);
            var organizationUser = await ApplicationStub.CreateOrganizationUserAsync(Factory, organization);
            await TestStub.LoginAsync(Factory, organizationUser.Email, organizationUser.Password);

            {
                // Arrange
                using var client = Factory.CreateAdminClient();
                var newObject = new ChangePasswordDtoFaker().Generate();

                // Act
                var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user/{organizationUser.Id}/change-password", newObject);

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                await TestStub.LoginAsync(Factory, organizationUser.Email, newObject.NewPassword);
            }

            await ApplicationStub.DeleteOrganizationUserAsync(Factory, organization, organizationUser);
            await ApplicationStub.DeleteOrganizationAsync(Factory, organization);
        }

        [Fact]
        public async Task OrganizationUserLifeCycleForbidden()
        {
            var organizationProposal = await ApplicationStub.CreateProposalAsync(Factory);
            var organizationSetup = await ApplicationStub.CreateOrganizationAsync(Factory, organizationProposal);
            var organization = await ApplicationStub.GetOrganizationAsync(Factory, organizationProposal);
            var organizationUser = await ApplicationStub.CreateOrganizationUserAsync(Factory, organization);
            await TestStub.LoginAsync(Factory, organizationUser.Email, organizationUser.Password);

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user", new OrganizationUserPasswordDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/admin/organization/{organization.Id}/user");

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.PutAsJsonAsync($"api/admin/organization/{organization.Id}/user/{organizationUser.Id}", new UserDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}/user/{organizationUser.Id}");

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            await ApplicationStub.DeleteOrganizationUserAsync(Factory, organization, organizationUser);
            await ApplicationStub.DeleteOrganizationAsync(Factory, organization);
        }
    }
}
