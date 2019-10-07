using FunderMaps.Interfaces;
using System.Threading.Tasks;

namespace FunderMaps.Services
{
    /// <summary>
    /// Mail service.
    /// </summary>
    public class MailService : IMailService
    {
        /// <summary>
        /// Send email async.
        /// </summary>
        /// <param name="template">Markup template.</param>
        /// <param name="name">Name of recipient.</param>
        /// <param name="email">Email of recipient.</param>
        /// <param name="subject">Mail subject.</param>
        /// <param name="msg">Mail message.</param>
        /// <returns></returns>
        public Task SendMailAsync(string template, string name, string email, string subject, string msg)
        {
            return Task.CompletedTask;
        }
    }
}
