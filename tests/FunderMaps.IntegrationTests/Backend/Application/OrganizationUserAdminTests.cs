using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using FunderMaps.Testing.Repositories;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationUserAdminTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public OrganizationUserAdminTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateOrganizationUserReturnOrganizationUser()
        {
            // Arrange
            var newOrganizationUser = new OrganizationUserPasswordDtoFaker().Generate();
            var organization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User.Role = ApplicationRole.Administrator;
                })
                .WithAuthenticationStores()
                .WithDataStoreItem(organization)
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user", newOrganizationUser);
            var returnObject = await response.Content.ReadFromJsonAsync<UserDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
        }

        [Fact]
        public async Task GetAllOrganizationUserReturnAllOrganizationUser()
        {
            // Arrange
            var organization = new OrganizationFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var organizationUser2 = new UserFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User.Role = ApplicationRole.Administrator;
                })
                .WithDataStoreList(new[]
                {
                    new UserRecord { User = organizationUser1 },
                    new UserRecord { User = organizationUser2 },
                })
                .WithDataStoreItem(organization)
                .WithDataStoreList(new[]
                {
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser1.Id,
                        OrganizationId = organization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser2.Id,
                        OrganizationId = organization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                })
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/admin/organization/{organization.Id}/user");
            var returnList = await response.Content.ReadFromJsonAsync<List<UserDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, returnList.Count);
        }

        [Fact]
        public async Task UpdateOrganizationUserReturnNoContent()
        {
            // Arrange
            var newOrganizationUser = new UserDtoFaker().Generate();
            var organization = new OrganizationFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User.Role = ApplicationRole.Administrator;
                })
                .WithDataStoreItem(new UserRecord { User = organizationUser1 })
                .WithDataStoreItem(organization)
                .WithDataStoreItem(new OrganizationUserRecord
                {
                    UserId = organizationUser1.Id,
                    OrganizationId = organization.Id,
                    OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                })
                .CreateClient();

            // Act
            var response = await client.PutAsJsonAsync($"api/admin/organization/{organization.Id}/user/{organizationUser1.Id}", newOrganizationUser);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

//
        [Fact]
        public async Task ChangeOrganizationUserRoleReturnNoContent()
        {
            // Arrange
            var newOrganizationUserRole = new ChangeOrganizationRoleDtoFaker().Generate();
            var organization = new OrganizationFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User.Role = ApplicationRole.Administrator;
                })
                .WithDataStoreItem(new UserRecord { User = organizationUser1 })
                .WithDataStoreItem(organization)
                .WithDataStoreItem(new OrganizationUserRecord
                {
                    UserId = organizationUser1.Id,
                    OrganizationId = organization.Id,
                    OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                })
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user/{organizationUser1.Id}/change-organization-role", newOrganizationUserRole);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ChangeOrganizationUserPasswordReturnNoContent()
        {
            // Arrange
            var newOrganizationUserPassword = new ChangePasswordDtoFaker().Generate();
            var organization = new OrganizationFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User.Role = ApplicationRole.Administrator;
                })
                .WithDataStoreItem(new UserRecord { User = organizationUser1 })
                .WithDataStoreItem(organization)
                .WithDataStoreItem(new OrganizationUserRecord
                {
                    UserId = organizationUser1.Id,
                    OrganizationId = organization.Id,
                    OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                })
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user/{organizationUser1.Id}/change-password", newOrganizationUserPassword);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
//

        [Fact]
        public async Task DeleteOrganizationUserReturnNoContent()
        {
            // Arrange
            var organization = new OrganizationFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User.Role = ApplicationRole.Administrator;
                })
                .WithDataStoreItem(new UserRecord { User = organizationUser1 })
                .WithDataStoreItem(organization)
                .WithDataStoreItem(new OrganizationUserRecord
                {
                    UserId = organizationUser1.Id,
                    OrganizationId = organization.Id,
                    OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                })
                .CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}/user/{organizationUser1.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task CreateOrganizationUserReturnForbidden(ApplicationRole applicationRole)
        {
            // Arrange
            var newOrganizationUser = new UserFaker().Generate();
            var organization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User.Role = applicationRole;
                })
                .WithAuthenticationStores()
                .WithDataStoreItem(organization)
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user", newOrganizationUser);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task GetAllOrganizationUserReturnForbidden(ApplicationRole applicationRole)
        {
            // Arrange
            var organization = new OrganizationFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var organizationUser2 = new UserFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User.Role = applicationRole;
                })
                .WithDataStoreList(new[]
                {
                    new UserRecord { User = organizationUser1 },
                    new UserRecord { User = organizationUser2 },
                })
                .WithDataStoreItem(organization)
                .WithDataStoreList(new[]
                {
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser1.Id,
                        OrganizationId = organization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser2.Id,
                        OrganizationId = organization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                })
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/admin/organization/{organization.Id}/user");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task UpdateOrganizationUserReturnForbidden(ApplicationRole applicationRole)
        {
            // Arrange
            var newOrganizationUser = new UserFaker().Generate();
            var organization = new OrganizationFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User.Role = applicationRole;
                })
                .WithDataStoreItem(new UserRecord { User = organizationUser1 })
                .WithDataStoreItem(organization)
                .WithDataStoreItem(new OrganizationUserRecord
                {
                    UserId = organizationUser1.Id,
                    OrganizationId = organization.Id,
                    OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                })
                .CreateClient();

            // Act
            var response = await client.PutAsJsonAsync($"api/admin/organization/{organization.Id}/user/{organizationUser1.Id}", newOrganizationUser);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task DeleteOrganizationUserReturnForbidden(ApplicationRole applicationRole)
        {
            // Arrange
            var organization = new OrganizationFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User.Role = applicationRole;
                })
                .WithDataStoreItem(new UserRecord { User = organizationUser1 })
                .WithDataStoreItem(organization)
                .WithDataStoreItem(new OrganizationUserRecord
                {
                    UserId = organizationUser1.Id,
                    OrganizationId = organization.Id,
                    OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                })
                .CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}/user/{organizationUser1.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
