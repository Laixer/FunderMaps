using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace FunderMaps.Webservice
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
