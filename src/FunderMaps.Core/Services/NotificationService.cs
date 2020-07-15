using FunderMaps.Core.Interfaces;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    public class NotificationService : INotificationService
    {
        public ValueTask NotifyByEmailAsync(string[] address)
        {
            return new ValueTask();
        }
    }
}
