using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    /// <summary>
    /// Mail service.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Send mail.
        /// </summary>
        /// <param name="templateFormat">Email template.</param>
        /// <param name="name">Recipient name.</param>
        /// <param name="email">Recipient mail address.</param>
        /// <param name="subject">Email subject.</param>
        /// <param name="msg">Message body.</param>
        Task SendMailAsync(string templateFormat, string name, string email, string subject, string msg);
    }
}
