using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FunderMaps.Cli.Drivers
{
    internal abstract class CommandDriver
    {
        protected static Task ResolveSelfScope<TDriver>(IServiceProvider services, Func<TDriver, Task> shadowFunc)
        {
            var scopeFactory = services.GetRequiredService<IServiceScopeFactory>();
            using var serviceScope = scopeFactory.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;
            TDriver driver = serviceProvider.GetService<TDriver>();
            return shadowFunc(driver);
        }
    }
}
