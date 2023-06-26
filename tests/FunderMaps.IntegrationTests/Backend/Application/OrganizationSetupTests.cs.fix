using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationSetupTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationSetupTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task OrganizationSetupLifeCycle()
        {
            var organizationProposal = await ApplicationStub.CreateProposalAsync(Factory);
            var organizationSetup = await ApplicationStub.CreateOrganizationAsync(Factory, organizationProposal);
            var organization = await ApplicationStub.GetOrganizationAsync(Factory, organizationProposal);
            await TestStub.LoginAsync(Factory, organizationSetup.Email, organizationSetup.Password);
            await ApplicationStub.DeleteOrganizationAsync(Factory, organization);
        }
    }
}
