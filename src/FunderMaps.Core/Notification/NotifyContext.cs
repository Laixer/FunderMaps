namespace FunderMaps.Core.Notification
{
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
