using FunderMaps.Core.Email;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Email service.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        ///     Send email message.
        /// </summary>
        /// <param name="emailMessage">Message to send.</param>
        Task SendAsync(EmailMessage emailMessage);

        /// <summary>
        ///     Test the email service backend.
        /// </summary>
        Task TestService();
    }
}
