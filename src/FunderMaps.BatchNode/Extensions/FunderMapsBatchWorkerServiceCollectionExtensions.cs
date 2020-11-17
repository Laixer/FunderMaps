using FunderMaps.Core.Threading;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Provides extension methods for services from this assembly.
    /// </summary>
    public static class FunderMapsBatchWorkerServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds batch job to the task component.
        /// </summary>
        public static IServiceCollection AddBatchJob<TBatchJob>(this IServiceCollection services)
        {
            services.AddTransient(typeof(TBatchJob));
            services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(BackgroundTask), typeof(TBatchJob)));

            return services;
        }
    }
}
