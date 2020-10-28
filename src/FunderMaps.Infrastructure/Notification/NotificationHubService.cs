using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Infrastructure.Notification
{
    // FUTURE: Move this to the core when rebuilding.
    internal class NotificationHubService : INotificationService
    {
        private readonly IEmailService _emailService;

        public NotificationHubService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // FUTURE: Write more generic helper for this problem
        private async void DispatchTask(EmailMessage message)
        {
            await _emailService.SendAsync(message);

            // TODO: Log on faillure.
        }

        // FUTURE: Wrap params in generic notification message
        // NOTE: For now we just redirect this call to the email service.
        public async Task NotifyByEmailAsync(string[] addresses, string content, string subject)
        {
            var message = new EmailMessage
            {
                Content = content,
                Subject = subject,
            };

            message.ToAddresses = addresses.Select(address => new EmailAddress
            {
                Address = address,
            });

            await Task.CompletedTask;
            DispatchTask(message);
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
