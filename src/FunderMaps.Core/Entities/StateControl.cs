using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Types;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Entity state control.
    /// </summary>
    public abstract class StateControl : AttributionControl
    {
        /// <summary>
        ///     Enitity status.
        /// </summary>
        public AuditStatus AuditStatus { get; set; }

        /// <summary>
        ///     Is write allowed in entry state.
        /// </summary>
        /// <returns><c>True</c> if allowed.</returns>
        public bool AllowWrite => AuditStatus == AuditStatus.Todo || AuditStatus == AuditStatus.Pending;

        /// <summary>
        ///     Move to next state.
        /// </summary>
        /// <remarks>
        ///     Not ever state can move to another without intervention.
        /// </remarks>
        public void MoveNext()
        {
            switch (AuditStatus)
            {
                case AuditStatus.Todo:
                    AuditStatus = AuditStatus.Pending;
                    break;
                case AuditStatus.Pending:
                    AuditStatus = AuditStatus.PendingReview;
                    break;
            }
        }

        /// <summary>
        ///     Move state to pending.
        /// </summary>
        public void TransitionToPending()
        {
            if (AuditStatus != AuditStatus.Todo || AuditStatus != AuditStatus.Rejected)
            {
                throw new FunderMapsCoreException("Cannot set status from current state"); // TODO: state exception
            }
            AuditStatus = AuditStatus.Pending;
        }

        /// <summary>
        ///     Move state to review.
        /// </summary>
        public void TransitionToReview()
        {
            if (AuditStatus != AuditStatus.Pending)
            {
                throw new FunderMapsCoreException("Cannot set status from current state"); // TODO: state exception
            }
            AuditStatus = AuditStatus.PendingReview;
        }

        /// <summary>
        ///     Move state to done.
        /// </summary>
        public void TransitionToDone()
        {
            if (AuditStatus != AuditStatus.PendingReview)
            {
                throw new FunderMapsCoreException("Cannot set status from current state"); // TODO: state exception
            }
            AuditStatus = AuditStatus.Done;
        }

        /// <summary>
        ///     Move state to rejected.
        /// </summary>
        public void TransitionToRejected()
        {
            if (AuditStatus != AuditStatus.PendingReview)
            {
                throw new FunderMapsCoreException("Cannot set status from current state"); // TODO: state exception
            }
            AuditStatus = AuditStatus.Rejected;
        }

        /// <summary>
        ///     Move state to discarded.
        /// </summary>
        public void TransitionToDiscarded()
        {
            if (AuditStatus == AuditStatus.Done)
            {
                throw new FunderMapsCoreException("Cannot set status from current state"); // TODO: state exception
            }
            AuditStatus = AuditStatus.Discarded;
        }
    }
}
