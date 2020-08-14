using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Types;
using System;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Entity state control.
    /// </summary>
    public abstract class StateControl<TEntity, TEntryIdentifier> : AttributionControl<TEntity, TEntryIdentifier>
        where TEntity : class
        where TEntryIdentifier : IEquatable<TEntryIdentifier>, IComparable<TEntryIdentifier>
    {
        protected StateControl(Func<TEntity, TEntryIdentifier> entryPrimaryKey) : base(entryPrimaryKey)
        {
        }

        /// <summary>
        ///     Enitity status.
        /// </summary>
        public AuditStatus AuditStatus { get; set; } = AuditStatus.Todo;

        /// <summary>
        ///     Is write allowed in entry state.
        /// </summary>
        /// <returns><c>True</c> if allowed.</returns>
        public bool AllowWrite => AuditStatus == AuditStatus.Todo || AuditStatus == AuditStatus.Pending;

        /// <summary>
        ///     Move to next state.
        /// </summary>
        /// <remarks>
        ///     Not every state can transition from one state to
        ///     another without intervention.
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
        ///     Move state to todo.
        /// </summary>
        /// <remarks>
        ///     Can move to this state from:
        ///     <list type="bullet">
        ///         <item>Pending</item>
        ///     </list>
        /// </remarks>
        public void TransitionToTodo()
        {
            if (AuditStatus != AuditStatus.Pending)
            {
                throw new StateTransitionException(AuditStatus, AuditStatus.Todo);
            }
            AuditStatus = AuditStatus.Todo;
        }

        /// <summary>
        ///     Move state to pending.
        /// </summary>
        /// <remarks>
        ///     Can move to this state from:
        ///     <list type="bullet">
        ///         <item>Todo</item>
        ///         <item>Pending</item>
        ///         <item>Rejected</item>
        ///     </list>
        /// </remarks>
        public void TransitionToPending()
        {
            if (AuditStatus != AuditStatus.Todo
                && AuditStatus != AuditStatus.Rejected
                && AuditStatus != AuditStatus.Pending)
            {
                throw new StateTransitionException(AuditStatus, AuditStatus.Pending);
            }
            AuditStatus = AuditStatus.Pending;
        }

        /// <summary>
        ///     Move state to review.
        /// </summary>
        /// <remarks>
        ///     Can move to this state from:
        ///     <list type="bullet">
        ///         <item>Pending</item>
        ///     </list>
        /// </remarks>
        public void TransitionToReview()
        {
            if (AuditStatus != AuditStatus.Pending)
            {
                throw new StateTransitionException(AuditStatus, AuditStatus.PendingReview);
            }
            AuditStatus = AuditStatus.PendingReview;
        }

        /// <summary>
        ///     Move state to done.
        /// </summary>
        /// <remarks>
        ///     Can move to this state from:
        ///     <list type="bullet">
        ///         <item>PendingReview</item>
        ///     </list>
        /// </remarks>
        public void TransitionToDone()
        {
            if (AuditStatus != AuditStatus.PendingReview)
            {
                throw new StateTransitionException(AuditStatus, AuditStatus.Done);
            }
            AuditStatus = AuditStatus.Done;
        }

        /// <summary>
        ///     Move state to rejected.
        /// </summary>
        /// <remarks>
        ///     Can move to this state from:
        ///     <list type="bullet">
        ///         <item>PendingReview</item>
        ///     </list>
        /// </remarks>
        public void TransitionToRejected()
        {
            if (AuditStatus != AuditStatus.PendingReview)
            {
                throw new StateTransitionException(AuditStatus, AuditStatus.Rejected);
            }
            AuditStatus = AuditStatus.Rejected;
        }

        /// <summary>
        ///     Move state to discarded.
        /// </summary>
        /// <remarks>
        ///     Can move to this state from:
        ///     <list type="bullet">
        ///         <item>Todo</item>
        ///         <item>Pending</item>
        ///         <item>PendingReview</item>
        ///         <item>Done</item>
        ///         <item>Rejected</item>
        ///     </list>
        /// </remarks>
        public void TransitionToDiscarded()
        {
            if (AuditStatus == AuditStatus.Done)
            {
                throw new StateTransitionException(AuditStatus, AuditStatus.Discarded);
            }
            AuditStatus = AuditStatus.Discarded;
        }
    }
}
