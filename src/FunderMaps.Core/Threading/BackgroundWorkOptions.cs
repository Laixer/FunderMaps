using System;

namespace FunderMaps.Core.Threading;

/// <summary>
///     Wrapper for all our console options.
/// </summary>
public sealed record BackgroundWorkOptions
{
    /// <summary>
    ///     Configuration section key.
    /// </summary>
    public const string Section = "BackgroundWorkOptions";

    /// <summary>
    ///     Defines the maximum items in our queue.
    /// </summary>
    public int MaxQueueSize { get; set; } = 8192;

    /// <summary>
    ///     The amount of simultaneous running background workers
    ///     for synchronous work.
    /// </summary>
    public int Workers { get; set; } = 2;

    /// <summary>
    ///     The time interval to wait before canceling a task.
    /// </summary>
    public TimeSpan TimeoutDelay { get; set; } = TimeSpan.FromMinutes(30);
}
