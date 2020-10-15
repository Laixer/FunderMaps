using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    // FUTURE: Collect all INotify services and determine which can handle the notification

    /// <summary>
    ///     Notification service.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        ///     Notify by email.
        /// </summary>
        /// <param name="address">Recipient mail adresses.</param>
        /// <param name="content">Message content.</param>
        /// <param name="subject">Message subject.</param>
        public abstract Task NotifyByEmailAsync(string[] address, string content, string subject);
    }
}
