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
        Task DispatchNotifyAsync(Envelope envelope);

        /// <summary>
        ///     Notify by means of contacting.
        /// </summary>
        /// <param name="taskName">Name of the task to handle the job.</param>
        /// <param name="envelope">Envelope containing the notification.</param>
        Task DispatchNotifyAsync(string taskName, Envelope envelope);
    }
}
