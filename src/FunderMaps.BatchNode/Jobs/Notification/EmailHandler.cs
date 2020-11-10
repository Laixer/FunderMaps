using System.Linq;
using System.Threading.Tasks;
using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Notification;
using FunderMaps.Core.Threading;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.BatchNode.Jobs.Notification
{
    /// <summary>
    ///     Email notification handler.
    /// </summary>
    internal class EmailHandler : NotificationTask
    {
        private readonly IEmailService _emailService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public EmailHandler(IEmailService emailService)
            => _emailService = emailService;

        /// <summary>
        ///     Handle the incoming notification.
        /// </summary>
        /// <param name="context">Notification context.</param>
        /// <param name="envelope">Envelope containing the notification.</param>
        public override async Task NotifyAsync(BackgroundTaskContext context, Envelope envelope)
        {
            var message = new EmailMessage
            {
                Content = envelope.Content,
                Subject = envelope.Subject,
            };

            message.ToAddresses = envelope.Recipients.Select(address => new EmailAddress
            {
                Address = address,
            });

            await _emailService.SendAsync(message);
        }

        /// <summary>
        ///     Method to check if the envelope can be handeld by this notification handler.
        /// </summary>
        /// <param name="envelope">Envelope containing the notification.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(Envelope envelope)
            => !string.IsNullOrEmpty(envelope.Subject)
                && envelope.Recipients.Count > 0
                && envelope.Recipients[0].Contains('@', System.StringComparison.InvariantCulture);
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
