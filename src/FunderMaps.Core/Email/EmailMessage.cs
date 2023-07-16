namespace FunderMaps.Core.Email;

/// <summary>
///     Email message.
/// </summary>
public record EmailMessage
{
    /// <summary>
    ///     Email recipients.
    /// </summary>
    public IEnumerable<EmailAddress> ToAddresses { get; set; } = new List<EmailAddress>();

    /// <summary>
    ///     Message subject.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    ///     Message content.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    ///     Template variables.
    /// </summary>
    public IDictionary<string, object> Varaibles { get; set; } = new Dictionary<string, object>();

    /// <summary>
    ///     Message template.
    /// </summary>
    public string? Template { get; set; }
}
