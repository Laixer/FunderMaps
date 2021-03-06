using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace FunderMaps.BatchNode
{
    /// <summary>
    ///     Application entry.
    /// </summary>
    public static class Program
    {
        /// <summary>
        ///     Application entry point.
        /// </summary>
        /// <param name="args">Commandline arguments.</param>
        public static Task Main(string[] args)
            => CreateHostBuilder(args).Build().RunAsync();

        /// <summary>
        ///     Build a host and run the application.
        /// </summary>
        /// <remarks>
        ///     The signature of this method should not be changed.
        ///     External tooling expects this function be present.
        /// </remarks>
        /// <param name="args">Commandline arguments.</param>
        /// <returns>See <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    // Configure FunderMaps services.
                    services.AddFunderMapsCoreServices();
                    services.AddFunderMapsInfrastructureServices();
                    services.AddFunderMapsDataServices("FunderMapsConnection");

                    // Add the task scheduler.
                    services.AddHostedService<TimedHostedService>();
                });
    }
}
