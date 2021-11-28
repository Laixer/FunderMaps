namespace FunderMaps.Core.Types;

/// <summary>
///     Audit status.
/// </summary>
public enum AuditStatus
{
    /// <summary>
    ///     Needs to be done.
    /// </summary>
    Todo = 0,

    /// <summary>
    ///     Pending.
    /// </summary>
    Pending = 1,

    /// <summary>
    ///     Done.
    /// </summary>
    Done = 2,

    // FUTURE: Remove.
    /// <summary>
    ///     Discarded.
    /// </summary>
    Discarded = 3,

    /// <summary>
    ///     Pending review.
    /// </summary>
    PendingReview = 4,

    /// <summary>
    ///     Rejected.
    /// </summary>
    Rejected = 5,
}
