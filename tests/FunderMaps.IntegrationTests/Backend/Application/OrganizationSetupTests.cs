using System.Threading.Tasks;
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
            var organizationProposal = await TestStub.CreateProposalAsync(Factory);
            var organizationSetup = await TestStub.CreateOrganizationAsync(Factory, organizationProposal);
            var organization = await TestStub.GetOrganizationAsync(Factory, organizationProposal);

            await TestStub.LoginAsync(Factory, organizationSetup.Email, organizationSetup.Password);
            await TestStub.RemoveOrganizationAsync(Factory, organization);
        }
    }
}
