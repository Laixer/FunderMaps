using System.Collections.Generic;
using System.Linq;

namespace FunderMaps.Core.Email
{
    public class EmailMessage
    {
        public IEnumerable<EmailAddress> ToAddresses { get; set; } = new List<EmailAddress>();
        public IEnumerable<EmailAddress> FromAddresses { get; set; } = new List<EmailAddress>();

        public string Subject { get; set; }
        public string Content { get; set; }

        public void AddToAddress(EmailAddress emailAddress)
            => _ = ToAddresses.Append(emailAddress);

        public void AddFromAddress(EmailAddress emailAddress)
            => _ = FromAddresses.Append(emailAddress);
    }
}
