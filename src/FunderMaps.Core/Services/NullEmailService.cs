using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    internal class NullEmailService : IEmailService
    {
        public Task SendAsync(EmailMessage emailMessage)
        {
            return Task.CompletedTask;
        }
    }
}
