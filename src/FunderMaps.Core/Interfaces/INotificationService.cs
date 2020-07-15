using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Notification service.
    /// </summary>
    public interface INotificationService
    {
        public abstract ValueTask NotifyByEmailAsync(string[] address);
    }
}
