using System;
using FunderMaps.Core.Threading;

namespace FunderMaps.Console.BackgroundTasks
{
    /// <summary>
    ///     Context for executing a background task.
    /// </summary>
    public class CommandTaskContext : BackgroundTaskContext
    {
        /// <summary>
        ///     Workspace directory
        /// </summary>
        public string Workspace { get; set; }

        /// <summary>
        ///     Whether or not to keep the workspace directory.
        /// </summary>
        public bool KeepWorkspace { get; set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public CommandTaskContext(Guid TaskId)
            : base(TaskId)
        {
        }
    }
}
