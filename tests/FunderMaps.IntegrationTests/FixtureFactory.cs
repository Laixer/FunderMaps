using System.Net.Http;
using System.Threading.Tasks;
using FunderMaps.Core.Types;
using Xunit;

namespace FunderMaps.IntegrationTests
{
    public abstract class FixtureFactory<TStartup> : IAsyncLifetime
        where TStartup : class
    {
        private AuthFunderMapsWebApplicationFactory<TStartup> adminAppClient;
        private AuthFunderMapsWebApplicationFactory<TStartup> superuserAppClient;
        private AuthFunderMapsWebApplicationFactory<TStartup> verifierAppClient;
        private AuthFunderMapsWebApplicationFactory<TStartup> writerAppClient;
        private AuthFunderMapsWebApplicationFactory<TStartup> readerAppClient;

        public async Task InitializeAsync()
        {
            using HttpClient client = CreateUnauthorizedClient();
            adminAppClient = new(client, "admin@fundermaps.com", "fundermaps");
            superuserAppClient = new(client, "Javier40@yahoo.com", "fundermaps");
            verifierAppClient = new(client, "Freda@contoso.com", "fundermaps");
            writerAppClient = new(client, "patsy@contoso.com", "fundermaps");
            readerAppClient = new(client, "lester@contoso.com", "fundermaps");

            await adminAppClient.InitializeAsync();
            await superuserAppClient.InitializeAsync();
            await verifierAppClient.InitializeAsync();
            await writerAppClient.InitializeAsync();
            await readerAppClient.InitializeAsync();
        }

        public HttpClient CreateAdminClient()
            => adminAppClient.CreateClient();

        public HttpClient CreateClient(OrganizationRole role = OrganizationRole.Reader)
            => role switch
            {
                OrganizationRole.Superuser => superuserAppClient.CreateClient(),
                OrganizationRole.Verifier => verifierAppClient.CreateClient(),
                OrganizationRole.Writer => writerAppClient.CreateClient(),
                _ => readerAppClient.CreateClient(),
            };

        public HttpClient CreateUnauthorizedClient()
            => new FunderMapsWebApplicationFactory<TStartup>()
                .CreateClient();

        public async Task DisposeAsync()
        {
            await readerAppClient.DisposeAsync();
            await writerAppClient.DisposeAsync();
            await verifierAppClient.DisposeAsync();
            await superuserAppClient.DisposeAsync();
            await adminAppClient.DisposeAsync();
        }
    }
}
