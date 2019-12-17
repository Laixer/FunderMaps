namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Report status.
    /// </summary>
    public enum ReportStatus
    {
        /// <summary>
        /// Needs to be done.
        /// </summary>
        Todo,

        /// <summary>
        /// Pending.
        /// </summary>
        Pending,

        /// <summary>
        /// Done.
        /// </summary>
        Done,

        /// <summary>
        /// Discarded.
        /// </summary>
        Discarded,

        /// <summary>
        /// Pending review.
        /// </summary>
        PendingReview,

        /// <summary>
        /// Rejected.
        /// </summary>
        Rejected,
    }
}
