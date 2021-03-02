using FunderMaps.Core.Types;
using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Indicading state transition failed/.
    /// </summary>
    public class StateTransitionException : FunderMapsCoreException
    {
        /// <summary>
        ///     Exception title
        /// </summary>
        public override string Title => "Requested entity cannot change state.";

        /// <summary>
        ///     State in which entity currently operates.
        /// </summary>
        public AuditStatus CurrentState { get; }

        /// <summary>
        ///     State in which entity failed to transition to.
        /// </summary>
        public AuditStatus TransitionState { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public StateTransitionException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public StateTransitionException(AuditStatus currentState, AuditStatus transitionState)
        {
            CurrentState = currentState;
            TransitionState = transitionState;
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public StateTransitionException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public StateTransitionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
