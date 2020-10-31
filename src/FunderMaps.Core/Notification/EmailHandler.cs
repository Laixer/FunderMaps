using System.Linq;
using System.Threading.Tasks;
using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Core.Notification
{
    /// <summary>
    ///     Email notification handler.
    /// </summary>
    internal class EmailHandler : INotifyHandler
    {
        private readonly IEmailService _emailService;
        private Envelope envelope;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public EmailHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        ///     Test if this handler can handle the notification.
        /// </summary>
        /// <returns><c>True</c> if notification will be handled, false otherwise.</returns>
        private bool CanHandle()
            => !string.IsNullOrEmpty(envelope.Subject) && envelope.Recipients[0].Contains('@');

        /// <summary>
        ///     Handle the incoming notification.
        /// </summary>
        /// <param name="context">Notification context.</param>
        public async ValueTask Handle(NotifyContext context)
        {
            envelope = context.Envelope;

            if (!CanHandle())
            {
                context.SetStatus(NotifyContextStatus.NotHandled);
                return;
            }

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

            context.SetStatus(NotifyContextStatus.Success);
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
