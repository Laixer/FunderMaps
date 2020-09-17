using FunderMaps.WebApi;

namespace FunderMaps.IntegrationTests.Backend
{
    public class BackendWebApplicationFactory : CustomWebApplicationFactory<Startup>
    {
    }

    public class AuthBackendWebApplicationFactory : AuthWebApplicationFactory<Startup>
    {
    }
}
