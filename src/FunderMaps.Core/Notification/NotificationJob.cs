using System;
using System.Threading.Tasks;
using FunderMaps.Core.Threading;

namespace FunderMaps.Core.Notification;

/// <summary>
///     Base class to notification tasks.
/// </summary>
public abstract class NotificationJob : BackgroundTask
{
    private const string TaskName = "NOTIFICATION";

    /// <summary>
    ///     Prepare the notification for the handler.
    /// </summary>
    /// <param name="context">Background task execution context.</param>
    public override async Task ExecuteAsync(BackgroundTaskContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        await NotifyAsync(context, context.Value as Envelope);
    }

    /// <summary>
    ///     Method to check if a task can be handled by this job.
    /// </summary>
    /// <param name="name">The task name.</param>
    /// <param name="value">The task payload.</param>
    /// <returns><c>True</c> if method handles task, false otherwise.</returns>
    public override bool CanHandle(string name, object value)
        => name is not null && name.ToUpperInvariant() == TaskName
            && value is Envelope envelope
            && envelope.Recipients.Count > 0
            && CanHandle(envelope);

    /// <summary>
    ///     Method to check if the envelope can be handeld by this notification handler.
    /// </summary>
    /// <param name="envelope">Envelope containing the notification.</param>
    /// <returns><c>True</c> if method handles task, false otherwise.</returns>
    public abstract bool CanHandle(Envelope envelope);

    /// <summary>
    ///     Handle the incoming notification.
    /// </summary>
    /// <param name="context">Notification context.</param>
    /// <param name="envelope">Envelope containing the notification.</param>
    public abstract Task NotifyAsync(BackgroundTaskContext context, Envelope envelope);
}
