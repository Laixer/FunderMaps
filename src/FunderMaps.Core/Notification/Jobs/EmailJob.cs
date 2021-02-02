using System.Collections.Generic;
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
    ///     Email notification job.
    /// </summary>
    internal class EmailJob : NotificationJob
    {
        private readonly IEmailService _emailService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public EmailJob(IEmailService emailService)
            => _emailService = emailService;

        /// <summary>
        ///     Handle the incoming notification.
        /// </summary>
        /// <remarks>
        ///     If one of the recipient fields happens to contain more than
        ///     a single address split them now.
        /// </remarks>
        /// <param name="context">Notification context.</param>
        /// <param name="envelope">Envelope containing the notification.</param>
        public override async Task NotifyAsync(BackgroundTaskContext context, Envelope envelope)
        {
            EmailMessage message = new()
            {
                Content = envelope.Content,
                Subject = envelope.Subject,
            };

            List<EmailAddress> toAddresses = new();
            foreach (var recipients in envelope.Recipients.Where(s => s.Contains(';')))
            {
                foreach (var recipient in recipients.Split(';'))
                {
                    toAddresses.Add(new EmailAddress()
                    {
                        Address = recipient.Trim(),
                    });
                }
            }

            toAddresses.AddRange(envelope.Recipients.Where(s => !s.Contains(';')).Select(address => new EmailAddress
            {
                Address = address.Trim(),
            }));

            message.ToAddresses = toAddresses;

            await _emailService.SendAsync(message);
        }

        /// <summary>
        ///     Method to check if the envelope can be handeld by this notification handler.
        /// </summary>
        /// <param name="envelope">Envelope containing the notification.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(Envelope envelope)
            => !string.IsNullOrEmpty(envelope.Subject)
                && envelope.Recipients[0].Contains('@', System.StringComparison.InvariantCulture);
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
