using System.Collections.Generic;

namespace FunderMaps.Core.Notification;

/// <summary>
///     Notification envelope.
/// </summary>
public record Envelope
{
    /// <summary>
    ///     Email recipients.
    /// </summary>
    public IList<string> Recipients { get; set; } = new List<string>();

    /// <summary>
    ///     Email senders.
    /// </summary>
    public IList<string> Senders { get; set; } = new List<string>();

    /// <summary>
    ///     Message subject.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    ///     Message content.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    ///     Message key values.
    /// </summary>
    public IDictionary<string, object> Items { get; set; } = new Dictionary<string, object>();

    /// <summary>
    ///     Message template.
    /// </summary>
    public string Template { get; set; }

    /// <summary>
    ///     Rendering extensions.
    /// </summary>
    public IList<object> Extensions { get; set; } = new List<object>();
}
