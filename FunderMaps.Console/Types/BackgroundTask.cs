using System;

namespace FunderMaps.Console.Types
{
    /// <summary>
    ///     Base class for a background task.
    /// </summary>
    public abstract class BackgroundTask
    {
        /// <summary>
        ///     The task id.
        /// </summary>
        public Guid Id { get; set; }
    }
}
