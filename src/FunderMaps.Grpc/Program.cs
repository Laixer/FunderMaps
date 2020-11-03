using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FunderMaps.Grpc
{
    /// <summary>
    ///     Application entry class.
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     Application entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args) 
            => CreateHostBuilder(args).Build().Run();

        /// <summary>
        ///     Create a host builder using the command line args.
        /// </summary>
        /// <param name="args">Command line args</param>
        /// <returns>Host builder instance.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
