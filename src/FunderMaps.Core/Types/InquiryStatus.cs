using System;

namespace FunderMaps.Core.Types
{
    /// <summary>
    /// Inquiry status.
    /// </summary>
    [Obsolete]
    public enum InquiryStatus
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
