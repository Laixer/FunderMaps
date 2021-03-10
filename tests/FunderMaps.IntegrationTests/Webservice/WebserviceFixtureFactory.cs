using System.Net.Http;
using System.Threading.Tasks;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice
{
    public class WebserviceFixtureFactory : IAsyncLifetime
    {
        private SetupFunderMapsWebApplicationFactory<FunderMaps.Webservice.Startup> setupAppClient;
        private AuthFunderMapsWebApplicationFactory<FunderMaps.Webservice.Startup> superuserAppClient;
        private AuthFunderMapsWebApplicationFactory<FunderMaps.Webservice.Startup> verifierAppClient;
        private AuthFunderMapsWebApplicationFactory<FunderMaps.Webservice.Startup> writerAppClient;
        private AuthFunderMapsWebApplicationFactory<FunderMaps.Webservice.Startup> readerAppClient;

        public OrganizationDto Organization => setupAppClient.Organization;
        public UserPair Superuser => setupAppClient.Superuser;
        public UserPair Verifier => setupAppClient.Verifier;
        public UserPair Writer => setupAppClient.Writer;
        public UserPair Reader => setupAppClient.Reader;

        public async Task InitializeAsync()
        {
            using HttpClient adminClient = new AdminWebApplicationFactory<FunderMaps.WebApi.Startup>().CreateClient();
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
            => new AdminWebApplicationFactory<FunderMaps.Webservice.Startup>()
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
            => new FunderMapsWebApplicationFactory<FunderMaps.Webservice.Startup>()
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
