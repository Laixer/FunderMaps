namespace FunderMaps.Core.Notification
{
    /// <summary>
    ///     Notification handler status.
    /// </summary>
    public enum NotifyContextStatus
    {
        /// <summary>
        ///     Notify was submitted to handler.
        /// </summary>
        Submitted,

        /// <summary>
        ///     Notification was handled with success.
        /// </summary>
        Success,

        /// <summary>
        ///     Notification was handled with fail.
        /// </summary>
        Failed,

        /// <summary>
        ///     Notification was not handled.
        /// </summary>
        NotHandled
    }

    /// <summary>
    ///     Notification context.
    /// </summary>
    public class NotifyContext
    {
        /// <summary>
        ///     Envelope.
        /// </summary>
        public Envelope Envelope { get; set; }

        /// <summary>
        ///     Handler status.
        /// </summary>
        public NotifyContextStatus Status { get; private set; } = NotifyContextStatus.Submitted;

        /// <summary>
        ///     Set handler status.
        /// </summary>
        public void SetStatus(NotifyContextStatus status)
            => Status = Status == NotifyContextStatus.Submitted
                ? status
                : Status;
    }
}
