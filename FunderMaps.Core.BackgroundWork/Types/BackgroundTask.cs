using System;

namespace FunderMaps.Core.BackgroundWork.Types
{
    /// <summary>
    ///     Base class for a background task.
    /// </summary>
    public abstract class BackgroundTask
    {
        /// <summary>
        ///     The task id.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        ///     Indicates if this background task should be run
        ///     on a thread (synchronously) or as a task (async).
        /// </summary>
        public abstract bool RunSynchronously { get; }
    }
}
