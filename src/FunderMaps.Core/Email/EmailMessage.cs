using System.Collections.Generic;

namespace FunderMaps.Core.Email
{
    public class EmailMessage
    {
        public IEnumerable<EmailAddress> ToAddresses { get; set; } = new List<EmailAddress>();
        public IEnumerable<EmailAddress> FromAddresses { get; set; } = new List<EmailAddress>();

        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
