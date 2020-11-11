using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace FunderMaps.BatchNode
{
    /// <summary>
    ///     Application entry.
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     Application entry point.
        /// </summary>
        /// <param name="args">Commandline arguments.</param>
        public static Task Main(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        // Setup HTTP/2 endpoint as default protocol.
                        options.ConfigureEndpointDefaults(options =>
                        {
                            options.Protocols = HttpProtocols.Http2;
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .Build()
                .RunAsync();
    }
}
