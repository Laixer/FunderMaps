using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace FunderMaps
{
    /// <summary>
    /// Application entry.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Commandline arguments.</param>
        public static async Task Main(string[] args)
            => await CreateHostBuilder(args).Build().RunAsync();

        /// <summary>
        /// Build a webhost and run the application.
        /// </summary>
        /// <remarks>
        /// The signature of this method cannot be changed since it's
        /// expected by external tooling using this assembly.
        /// </remarks>
        /// <param name="args">Commandline arguments.</param>
        /// <returns><see cref="IWebHostBuilder"/></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                //webBuilder.UseKestrel(c => c.AddServerHeader = false);
                webBuilder.UseStartup<Startup>();
            });
    }
}
