using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace FunderMaps
{
    /// <summary>
    /// Application entry.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Commandline arguments.</param>
        public static async Task Main(string[] args)
            => await BuildWebHost(args).RunAsync();

        /// <summary>
        /// Build a webhost and run the application.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args)
            => WebHost.CreateDefaultBuilder(args)
                .UseKestrel(c => c.AddServerHeader = false)
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .Build();
    }
}
