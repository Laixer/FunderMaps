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
    public class OrganizationUserTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public OrganizationUserTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateOrganizationUserFromSessionReturnOrganizationUser()
        {
            // Arrange
            var newOrganizationUser = new OrganizationUserPasswordDtoFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.OrganizationRole = OrganizationRole.Superuser;
                })
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/organization/user", newOrganizationUser);
            var returnObject = await response.Content.ReadFromJsonAsync<UserDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
        }

        [Fact]
        public async Task GetAllOrganizationUserFromSessionReturnAllOrganizationUser()
        {
            // Arrange
            var sessionUser = new UserFaker().Generate();
            var sessionOrganization = new OrganizationFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User = sessionUser;
                    options.Organization = sessionOrganization;
                })
                .WithDataStoreList(new[]
                {
                    new UserRecord { User = sessionUser },
                    new UserRecord { User = organizationUser1 },
                })
                .WithDataStoreItem(sessionOrganization)
                .WithDataStoreList(new[]
                {
                    new OrganizationUserRecord
                    {
                        UserId = sessionUser.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser1.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                })
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/organization/user");
            var returnList = await response.Content.ReadFromJsonAsync<List<UserDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, returnList.Count);
        }

        [Fact]
        public async Task UpdateOrganizationUserFromSessionReturnNoContent()
        {
            // Arrange
            var newOrganizationUser = new UserFaker().Generate();
            var sessionUser = new UserFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var sessionOrganization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User = sessionUser;
                    options.Organization = sessionOrganization;
                    options.OrganizationRole = OrganizationRole.Superuser;
                })
                .WithDataStoreList(new[]
                {
                    new UserRecord { User = sessionUser },
                    new UserRecord { User = organizationUser1 },
                })
                .WithDataStoreItem(sessionOrganization)
                .WithDataStoreList(new[]
                {
                    new OrganizationUserRecord
                    {
                        UserId = sessionUser.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser1.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                })
                .CreateClient();

            // Act
            var response = await client.PutAsJsonAsync($"api/organization/user/{organizationUser1.Id}", newOrganizationUser);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrganizationUserFromSessionReturnNoContent()
        {
            // Arrange
            var sessionUser = new UserFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var sessionOrganization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User = sessionUser;
                    options.Organization = sessionOrganization;
                    options.OrganizationRole = OrganizationRole.Superuser;
                })
                .WithDataStoreList(new List<UserRecord>
                {
                    new UserRecord { User = sessionUser },
                    new UserRecord { User = organizationUser1 },
                })
                .WithDataStoreItem(sessionOrganization)
                .WithDataStoreList(new List<OrganizationUserRecord>
                {
                    new OrganizationUserRecord
                    {
                        UserId = sessionUser.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser1.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                })
                .CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/organization/user/{organizationUser1.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Reader)]
        public async Task CreateOrganizationUserFromSessionReturnForbidden(OrganizationRole organizationRole)
        {
            // Arrange
            var newOrganizationUser = new UserFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.OrganizationRole = organizationRole;
                })
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/organization/user", newOrganizationUser);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Reader)]
        public async Task UpdateOrganizationUserFromSessionReturnForbidden(OrganizationRole organizationRole)
        {
            // Arrange
            var newOrganizationUser = new UserFaker().Generate();
            var sessionUser = new UserFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var sessionOrganization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User = sessionUser;
                    options.Organization = sessionOrganization;
                    options.OrganizationRole = organizationRole;
                })
                .WithDataStoreList(new[]
                {
                    new UserRecord { User = sessionUser },
                    new UserRecord { User = organizationUser1 },
                })
                .WithDataStoreItem(sessionOrganization)
                .WithDataStoreList(new[]
                {
                    new OrganizationUserRecord
                    {
                        UserId = sessionUser.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser1.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                })
                .CreateClient();

            // Act
            var response = await client.PutAsJsonAsync($"api/organization/user/{organizationUser1.Id}", newOrganizationUser);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Reader)]
        public async Task DeleteOrganizationUserFromSessionReturnForbidden(OrganizationRole organizationRole)
        {
            // Arrange
            var sessionUser = new UserFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var sessionOrganization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User = sessionUser;
                    options.Organization = sessionOrganization;
                    options.OrganizationRole = organizationRole;
                })
                .WithDataStoreList(new List<UserRecord>
                {
                    new UserRecord { User = sessionUser },
                    new UserRecord { User = organizationUser1 },
                })
                .WithDataStoreItem(sessionOrganization)
                .WithDataStoreList(new List<OrganizationUserRecord>
                {
                    new OrganizationUserRecord
                    {
                        UserId = sessionUser.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser1.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = new Bogus.Faker().PickRandom<OrganizationRole>(),
                    },
                })
                .CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/organization/user/{organizationUser1.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
