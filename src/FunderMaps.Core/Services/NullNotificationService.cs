using FunderMaps.Core.Interfaces;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Dummpy notification service.
    /// </summary>
    internal class NullNotificationService : INotificationService
    {
        /// <summary>
        ///     Notify by email.
        /// </summary>
        /// <param name="address">Array of recipients.</param>
        /// <param name="content">Message content.</param>
        /// <param name="subject">Message subject.</param>
        public Task NotifyByEmailAsync(string[] address, string content, string subject)
        {
            return Task.CompletedTask;
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
