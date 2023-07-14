using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    /// <summary>
    ///     Teststub for all application tests.
    /// </summary>
    public static class ApplicationStub
    {
        // public static async Task<OrganizationProposalDto> CreateProposalAsync(BackendFixtureFactory factory)
        // {
        //     // Arrange
        //     using var client = factory.CreateAdminClient();
        //     var newObject = new OrganizationProposalDtoFaker().Generate();

        //     // Act
        //     var response = await client.PostAsJsonAsync("api/organization/proposal", newObject);
        //     var returnObject = await response.Content.ReadFromJsonAsync<OrganizationProposalDto>();

        //     // Assert
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //     Assert.Equal(newObject.Name, returnObject.Name);

        //     return returnObject;
        // }

        // public static async Task<OrganizationSetupDto> CreateOrganizationAsync(BackendFixtureFactory factory, OrganizationProposalDto organization)
        // {
        //     // Arrange
        //     using var client = factory.CreateUnauthorizedClient();
        //     var newObject = new OrganizationSetupDtoFaker().Generate();

        //     // Act
        //     var response = await client.PostAsJsonAsync($"api/organization/{organization.Id}/setup", newObject);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        //     return newObject;
        // }

        // public static async Task<OrganizationDto> GetOrganizationAsync(BackendFixtureFactory factory, OrganizationProposalDto organization)
        // {
        //     // Arrange
        //     using var client = factory.CreateAdminClient();

        //     // Act
        //     var response = await client.GetAsync($"api/admin/organization/{organization.Id}");
        //     var returnObject = await response.Content.ReadFromJsonAsync<OrganizationDto>();

        //     // Assert
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //     Assert.Equal(organization.Id, returnObject.Id);

        //     return returnObject;
        // }

        public static async Task DeleteOrganizationAsync(BackendFixtureFactory factory, OrganizationDto organization)
        {
            // Arrange
            using var client = factory.CreateAdminClient();

            // Act
            var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        public static async Task<OrganizationUserPasswordDto> CreateOrganizationUserAsync(BackendFixtureFactory factory, OrganizationDto organization)
        {
            // Arrange
            using var client = factory.CreateAdminClient();
            var newObject = new OrganizationUserPasswordDtoFaker().Generate();

            // Act
            var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user", newObject);
            var returnObject = await response.Content.ReadFromJsonAsync<UserDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
            Assert.Equal(newObject.GivenName, returnObject.GivenName);
            Assert.Equal(newObject.LastName, returnObject.LastName);
            Assert.Equal(newObject.Email, returnObject.Email);
            Assert.Equal(newObject.Avatar, returnObject.Avatar);
            Assert.Equal(newObject.JobTitle, returnObject.JobTitle);
            Assert.Equal(newObject.PhoneNumber, returnObject.PhoneNumber);
            Assert.Equal(newObject.Role, returnObject.Role);

            return newObject with
            {
                Id = returnObject.Id
            };
        }

        public static async Task DeleteOrganizationUserAsync(BackendFixtureFactory factory, OrganizationDto organization, UserDto user)
        {
            // Arrange
            using var client = factory.CreateAdminClient();

            // Act
            var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}/user/{user.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
