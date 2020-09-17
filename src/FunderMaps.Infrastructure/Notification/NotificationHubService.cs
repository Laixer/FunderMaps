using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

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

        // NOTE: For now we just redirect this call to the email service.
        public async ValueTask NotifyByEmailAsync(string[] addresses, string content)
        {
            var message = new EmailMessage
            {
                Content = content,
            };

            message.ToAddresses = addresses.Select(address => new EmailAddress
            {
                Address = address,
            });

            await _emailService.SendAsync(message);
        }
    }
}
