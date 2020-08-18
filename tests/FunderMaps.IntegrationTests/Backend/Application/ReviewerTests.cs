using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.IntegrationTests.Repositories;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    // FUTURE: navigation test

    public class ReviewerTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public ReviewerTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData(OrganizationRole.Superuser)]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        public async Task GetAllReviewerReturnAllReviewer(OrganizationRole role)
        {
            // Arrange
            var sessionUser = new UserFaker().Generate();
            var sessionOrganization = new OrganizationFaker().Generate();
            var organizationUser1 = new UserFaker().Generate();
            var organizationUser2 = new UserFaker().Generate();
            var organizationUser3 = new UserFaker().Generate();
            var organizationUser4 = new UserFaker().Generate();
            var client = _factory
                .WithAuthentication(options =>
                {
                    options.User = sessionUser;
                    options.Organization = sessionOrganization;
                    options.OrganizationRole = role;
                })
                .WithDataStoreList(new[]
                {
                    new UserRecord { User = sessionUser },
                    new UserRecord { User = organizationUser1 },
                    new UserRecord { User = organizationUser2 },
                    new UserRecord { User = organizationUser3 },
                    new UserRecord { User = organizationUser4 },
                })
                .WithDataStoreList(sessionOrganization)
                .WithDataStoreList(new[]
                {
                    new OrganizationUserRecord
                    {
                        UserId = sessionUser.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = role,
                    },
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser1.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = OrganizationRole.Verifier,
                    },
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser2.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = OrganizationRole.Verifier,
                    },
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser3.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = OrganizationRole.Superuser,
                    },
                    new OrganizationUserRecord
                    {
                        UserId = organizationUser4.Id,
                        OrganizationId = sessionOrganization.Id,
                        OrganizationRole = OrganizationRole.Writer,
                    },
                })
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/reviewer");
            var returnList = await response.Content.ReadFromJsonAsync<List<ReviewerDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, returnList.Count);
            Assert.True(response.Headers.CacheControl.Public);
            Assert.NotNull(response.Headers.CacheControl.MaxAge);
        }
    }
}
