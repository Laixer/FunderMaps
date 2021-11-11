using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Threading;

namespace FunderMaps.Core.Notification.Jobs
{
    /// <summary>
    ///     Email notification job.
    /// </summary>
    internal class EmailJob : NotificationJob
    {
        private readonly ITemplateParser _templateParser;
        private readonly IEmailService _emailService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public EmailJob(ITemplateParser templateParser, IEmailService emailService)
            => (_templateParser, _emailService) = (templateParser, emailService);

        /// <summary>
        ///     Handle the incoming notification.
        /// </summary>
        /// <remarks>
        ///     If one of the recipient fields happens to contain more than
        ///     a single address split them now. We need to only send valid
        ///     addresses because they are checked against all notification
        ///     jobs.
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

            if (!string.IsNullOrEmpty(envelope.Template))
            {
                _templateParser.AddObject(envelope.Items);
                _templateParser.FromTemplateFile("Email", envelope.Template);

                foreach (var extension in envelope.Extensions)
                {
                    // FUTURE: Fix this cast.
                    (_templateParser as Core.Components.TemplateParser).RegisterExtension(extension as Scriban.Runtime.IScriptObject);
                }

                message.Content = await _templateParser.RenderAsync(default);
            }

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

            await _emailService.SendAsync(message, context.CancellationToken);
        }

        /// <summary>
        ///     Method to check if the envelope can be handeld by this notification handler.
        /// </summary>
        /// <param name="envelope">Envelope containing the notification.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(Envelope envelope)
            => !string.IsNullOrEmpty(envelope.Subject)
                && envelope.Recipients.All(address => address.Contains('@', System.StringComparison.InvariantCulture));
    }
}
