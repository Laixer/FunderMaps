using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Dummy eamail service.
    /// </summary>
    internal class NullEmailService : IEmailService
    {
        /// <summary>
        ///     Send email message.
        /// </summary>
        /// <param name="emailMessage">Message to send.</param>
        public Task SendAsync(EmailMessage emailMessage)
        {
            return Task.CompletedTask;
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
