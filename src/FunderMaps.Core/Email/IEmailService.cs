using FunderMaps.Core.Interfaces;

namespace FunderMaps.Core.Email;

/// <summary>
///     Email service.
/// </summary>
public interface IEmailService : IServiceHealthCheck
{
    /// <summary>
    ///     Send email message.
    /// </summary>
    /// <param name="emailMessage">Message to send.</param>
    /// <param name="token">Cancellation token.</param>
    Task SendAsync(EmailMessage emailMessage, CancellationToken token = default);
}
