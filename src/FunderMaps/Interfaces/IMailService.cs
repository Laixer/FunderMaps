using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    public interface IMailService
    {
        Task SendMailAsync(string template, string name, string email, string subject, string msg);
    }
}
