using System.Threading.Tasks;

namespace FunderMaps.Core.Notification
{
    /// <summary>
    ///     Notify service.
    /// </summary>
    public interface INotifyService
    {
        /// <summary>
        ///     Notify by means of contacting.
        /// </summary>
        /// <param name="envelope">Envelope containing the notification.</param>
        Task DispatchNotify(Envelope envelope);
    }
}
