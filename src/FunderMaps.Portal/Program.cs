using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FunderMaps.Portal
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
            => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "FunderMaps.AspNetCore");
                })
                .Build()
                .RunAsync();
    }
}
