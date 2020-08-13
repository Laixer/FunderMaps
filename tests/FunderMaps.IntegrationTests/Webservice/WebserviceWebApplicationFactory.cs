using FunderMaps.Webservice;

namespace FunderMaps.IntegrationTests.Webservice
{
    public class WebserviceWebApplicationFactory : CustomWebApplicationFactory<Startup>
    {
    }

    public class AuthWebserviceWebApplicationFactory : AuthWebApplicationFactory<Startup>
    {
    }
}
