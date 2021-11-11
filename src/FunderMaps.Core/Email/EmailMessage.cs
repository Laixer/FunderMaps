namespace FunderMaps.Core.Email
{
    /// <summary>
    ///     Email message.
    /// </summary>
    public record EmailMessage
    {
        /// <summary>
        ///     Email recipients.
        /// </summary>
        public IEnumerable<EmailAddress> ToAddresses { get; set; } = new List<EmailAddress>();

        /// <summary>
        ///     Email senders.
        /// </summary>
        public IEnumerable<EmailAddress> FromAddresses { get; set; } = new List<EmailAddress>();

        /// <summary>
        ///     Message subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///     Message content.
        /// </summary>
        public string Content { get; set; }
    }
}
