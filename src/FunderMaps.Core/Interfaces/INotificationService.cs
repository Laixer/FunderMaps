using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Notification service.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        ///     Notify by email.
        /// </summary>
        /// <param name="address">Recipient mail adresses.</param>
        /// <returns></returns>
        public abstract ValueTask NotifyByEmailAsync(string[] address);
    }
}
