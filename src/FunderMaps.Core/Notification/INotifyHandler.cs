using System.Threading.Tasks;

namespace FunderMaps.Core.Notification
{
    /// <summary>
    ///     Notification handler interface.
    /// </summary>
    public interface INotifyHandler
    {
        /// <summary>
        ///     Handle the incoming notification.
        /// </summary>
        /// <param name="context">Notification context.</param>
        ValueTask Handle(NotifyContext context);
    }
}
