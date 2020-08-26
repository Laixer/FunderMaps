using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Infrastructure.Email
{
    /// <summary>
    ///     Options for the email service.
    /// </summary>
    public sealed class EmailOptions
    {
        /// <summary>
        ///     Configuration section key.
        /// </summary>
        public const string Section = "Email";

        /// <summary>
        ///     SMTP server host.
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        ///     SMTP server port.
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        ///     SMTP server secure connection.
        /// </summary>
        public bool SmtpTls { get; set; }

        /// <summary>
        ///     SMTP authentication username.
        /// </summary>
        public string SmtpUsername { get; set; }

        /// <summary>
        ///     SMTP authentication password.
        /// </summary>
        public string SmtpPassword { get; set; }

        /// <summary>
        ///     Default sender name if none is provided.
        /// </summary>
        public string DefaultSenderName { get; set; }

        /// <summary>
        ///     Default sender address if none is provided.
        /// </summary>
        [Required, EmailAddress]
        public string DefaultSenderAddress { get; set; }
    }
}
