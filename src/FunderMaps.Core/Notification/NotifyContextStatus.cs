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
}
