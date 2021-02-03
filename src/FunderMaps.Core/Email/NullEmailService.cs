using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Core.Email
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
        /// <param name="token">Cancellation token.</param>
        public Task SendAsync(EmailMessage emailMessage, CancellationToken token) => Task.CompletedTask;

        /// <summary>
        ///     Test the batch service backend.
        /// </summary>
        public Task HealthCheck() => Task.CompletedTask;
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
