using FunderMaps.Core.Services;
using FunderMaps.Webservice.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice
{
    /// <summary>
    ///     Regression tests for the startup services.
    /// </summary>
    public class StartupTests : IClassFixture<AuthWebserviceWebApplicationFactory>
    {
        private readonly AuthWebserviceWebApplicationFactory _factory;

        /// <summary>
        ///     Create new instance and setup the test data.
        /// </summary>
        public StartupTests(AuthWebserviceWebApplicationFactory factory)
            => _factory = factory;

        [Fact]
        public void RegressionServicesResolveProductTrackingService()
        {
            // Arrange.
            using var scope = _factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IProductService>();

            // Assert.
            Assert.IsType<ProductTrackingService>(service);
        }
    }
}
