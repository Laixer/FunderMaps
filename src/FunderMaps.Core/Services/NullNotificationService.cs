using FunderMaps.Core.Interfaces;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Notification service.
    /// </summary>
    internal class NullNotificationService : INotificationService
    {
        /// <summary>
        ///     Notify by email.
        /// </summary>
        /// <param name="address">Array of recipients.</param>
        /// <param name="content">Message content.</param>
        public ValueTask NotifyByEmailAsync(string[] address, string content)
        {
            return new ValueTask();
        }
    }
}
