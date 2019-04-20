using System;
using System.Threading.Tasks;
using FunderMaps.Interfaces;

namespace FunderMaps.Services
{
    public class MailService : IMailService
    {
        public Task SendMailAsync(string template, string name, string email, string subject, string msg)
        {
            return Task.CompletedTask;
        }
    }
}
