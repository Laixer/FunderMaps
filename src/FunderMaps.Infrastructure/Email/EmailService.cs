using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
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
    // FUTURE: Turn this into a factory
    // FUTURE: We can configure much more and we will in a next release.
    // TODO: Check input.
    internal class EmailService : IEmailService, IDisposable
    {
        private readonly EmailOptions _options;
        private readonly ISmtpClient emailClient = new SmtpClient();
        private bool disposedValue;

        /// <summary>
        ///     Logger.
        /// </summary>
        public ILogger Logger { get; }

        public EmailService(IOptions<EmailOptions> options, ILogger<EmailService> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            Logger = logger;
        }

        private InternetAddress GetDefaultSender
            => new MailboxAddress(_options.DefaultSenderName, _options.DefaultSenderAddress);

        /// <summary>
        ///     Set the headers in the email message.
        /// </summary>
        /// <param name="message">Mail message to send.</param>
        /// <param name="emailMessage">Message source.</param>
        protected void BuildHeader(ref MimeMessage message, EmailMessage emailMessage)
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
        protected static void BuildBody(ref MimeMessage message, EmailMessage emailMessage)
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

            BuildHeader(ref message, emailMessage);
            BuildBody(ref message, emailMessage);

            Logger.LogDebug($"Message prepared, try sending message to MTA");

            await emailClient.ConnectAsync(_options.SmtpServer, _options.SmtpPort, _options.SmtpTls);
            await emailClient.AuthenticateAsync(_options.SmtpUsername, _options.SmtpPassword);
            await emailClient.SendAsync(message);
            await emailClient.DisconnectAsync(quit: true);

            Logger.LogInformation($"Message sent with success");
        }

        // FUTURE: From interface
        /// <summary>
        ///     Test the email service backend.
        /// </summary>
        public async Task TestService()
        {
            await emailClient.ConnectAsync(_options.SmtpServer, _options.SmtpPort, _options.SmtpTls);
            await emailClient.AuthenticateAsync(_options.SmtpUsername, _options.SmtpPassword);
            await emailClient.DisconnectAsync(quit: true);
        }

        #region Disposable Pattern

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    emailClient.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Disposable Pattern
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
