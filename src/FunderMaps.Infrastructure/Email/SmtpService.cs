using FunderMaps.Core.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Infrastructure.Email
{
    // FUTURE: Catch ex.
    // FUTURE: We can configure much more and we will in a next release.
    // TODO: Check input.
    /// <summary>
    ///     Send email to the MTA.
    /// </summary>
    /// <remarks>
    ///     Keep the connection to the remote host succinct. MTA's are
    ///     typically not prepared to deal with long living connections.
    ///     Also its unclear if <seealso cref="SmtpClient"/> is designed
    ///     to reuse transport connections.
    /// </remarks>
    internal class SmtpService : IEmailService
    {
        private readonly SmtpOptions _options;

        /// <summary>
        ///     Logger.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public SmtpService(IOptions<SmtpOptions> options, ILogger<SmtpService> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private InternetAddress GetDefaultSender => new MailboxAddress(_options.DefaultSenderName, _options.DefaultSenderAddress);

        /// <summary>
        ///     Set the headers in the email message.
        /// </summary>
        /// <param name="message">Mail message to send.</param>
        /// <param name="emailMessage">Message source.</param>
        protected virtual void BuildHeader(MimeMessage message, EmailMessage emailMessage)
        {
            message.To.AddRange(emailMessage.ToAddresses.Select(m => new MailboxAddress(m.Name, m.Address)));

            if (emailMessage.FromAddresses.Any())
            {
                message.From.AddRange(emailMessage.FromAddresses.Select(m => new MailboxAddress(m.Name, m.Address)));
            }
            else
            {
                message.From.Add(GetDefaultSender);
            }

            message.Subject = emailMessage.Subject;

            Logger.LogDebug($"Sending message to {string.Join(", ", emailMessage.ToAddresses)}");
        }

        /// <summary>
        ///     Set the body in the email message.
        /// </summary>
        /// <param name="message">Mail message to send.</param>
        /// <param name="emailMessage">Message source.</param>
        protected virtual void BuildBody(MimeMessage message, EmailMessage emailMessage)
        {
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content,
            };
        }

        /// <summary>
        ///     Send email message.
        /// </summary>
        /// <param name="emailMessage">Message to send.</param>
        public async Task SendAsync(EmailMessage emailMessage)
        {
            MimeMessage message = new();

            BuildHeader(message, emailMessage);
            BuildBody(message, emailMessage);

            Logger.LogDebug($"Message prepared, try sending message to MTA");

            using SmtpClient client = new();

            await client.ConnectAsync(_options.SmtpServer, _options.SmtpPort, _options.SmtpTls);
            await client.AuthenticateAsync(_options.SmtpUsername, _options.SmtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(quit: true);

            Logger.LogInformation($"Message sent with success");
        }

        /// <summary>
        ///     Test the email service backend.
        /// </summary>
        public async Task HealthCheck()
        {
            using SmtpClient client = new();

            await client.ConnectAsync(_options.SmtpServer, _options.SmtpPort, _options.SmtpTls);
            await client.AuthenticateAsync(_options.SmtpUsername, _options.SmtpPassword);
            await client.NoOpAsync();
            await client.DisconnectAsync(quit: true);
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
