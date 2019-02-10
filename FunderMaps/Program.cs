using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace FunderMaps
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await WebHost
                .CreateDefaultBuilder<Startup>(args)
                .Build()
                .RunAsync();
        }
    }
}
