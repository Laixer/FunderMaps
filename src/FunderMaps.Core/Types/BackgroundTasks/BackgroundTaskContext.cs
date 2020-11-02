using System;
using System.Threading;

namespace FunderMaps.Core.Types.BackgroundTasks
{
    /// <summary>
    ///     Context for executing a background task.
    /// </summary>
    public class BackgroundTaskContext
    {
        /// <summary>
        ///     The cancellation token.
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        ///     The task id.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        ///     The object to process, if any.
        /// </summary>
        public object Value { get; set; }
    }
}
