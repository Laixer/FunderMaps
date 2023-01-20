using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Infrastructure.Email;

/// <summary>
///     Options for the smtp service.
/// </summary>
public sealed record MailgunOptions
{
    /// <summary>
    ///     Configuration section key.
    /// </summary>
    public const string Section = "Mailgun";

    /// <summary>
    ///     Mailgun API key.
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    ///     Sender domain.
    /// </summary>
    public string Domain { get; set; }

    /// <summary>
    ///     Default sender name if none is provided.
    /// </summary>
    public string DefaultSenderName { get; set; }

    /// <summary>
    ///     Default sender address if none is provided.
    /// </summary>
    [Required, EmailAddress]
    public string DefaultSenderAddress { get; set; }
}
