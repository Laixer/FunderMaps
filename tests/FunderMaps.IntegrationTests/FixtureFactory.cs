using System.Net.Http;
using System.Threading.Tasks;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using Xunit;

namespace FunderMaps.IntegrationTests
{
    public abstract class FixtureFactory<TSetup, TLocal> : IAsyncLifetime
        where TSetup : class
        where TLocal : class
    {
        private SetupFunderMapsWebApplicationFactory<TLocal> setupAppClient;
        private AuthFunderMapsWebApplicationFactory<TLocal> superuserAppClient;
        private AuthFunderMapsWebApplicationFactory<TLocal> verifierAppClient;
        private AuthFunderMapsWebApplicationFactory<TLocal> writerAppClient;
        private AuthFunderMapsWebApplicationFactory<TLocal> readerAppClient;

        public OrganizationDto Organization => setupAppClient.Organization;
        public UserPair Superuser => setupAppClient.Superuser;
        public UserPair Verifier => setupAppClient.Verifier;
        public UserPair Writer => setupAppClient.Writer;
        public UserPair Reader => setupAppClient.Reader;

        public async Task InitializeAsync()
        {
            using HttpClient adminClient = CreateSetupClient();
            setupAppClient = new(adminClient);

            await setupAppClient.InitializeAsync();

            using HttpClient client = CreateUnauthorizedClient();
            superuserAppClient = new(client, Superuser);
            verifierAppClient = new(client, Verifier);
            writerAppClient = new(client, Writer);
            readerAppClient = new(client, Reader);

            await superuserAppClient.InitializeAsync();
            await verifierAppClient.InitializeAsync();
            await writerAppClient.InitializeAsync();
            await readerAppClient.InitializeAsync();
        }

        public HttpClient CreateAdminClient()
            => new AdminWebApplicationFactory<TLocal>()
                .CreateClient();

        public HttpClient CreateSetupClient()
            => new AdminWebApplicationFactory<TSetup>()
                .CreateClient();

        public HttpClient CreateClient(OrganizationRole role = OrganizationRole.Reader)
            => role switch
            {
                OrganizationRole.Superuser => superuserAppClient.CreateClient(),
                OrganizationRole.Verifier => verifierAppClient.CreateClient(),
                OrganizationRole.Writer => writerAppClient.CreateClient(),
                _ => readerAppClient.CreateClient(),
            };

        public HttpClient CreateUnauthorizedClient()
            => new FunderMapsWebApplicationFactory<TLocal>()
                .CreateClient();

        public async Task DisposeAsync()
        {
            await readerAppClient.DisposeAsync();
            await writerAppClient.DisposeAsync();
            await verifierAppClient.DisposeAsync();
            await superuserAppClient.DisposeAsync();
            await setupAppClient.DisposeAsync();
        }
    }
}
