using System.Net.Http.Headers;
using FunderMaps.Core.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Infrastructure.Email;

internal class MailgunService : IEmailService
{
    const string DefaultBaseUrl = @"https://api.eu.mailgun.net/v3/";

    private readonly MailgunOptions _options;

    private readonly ILogger _logger;

    private HttpClient client = new();

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public MailgunService(IOptions<MailgunOptions> options, ILogger<SmtpService> logger)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        client.BaseAddress = new(DefaultBaseUrl);
    }

    //     message.To.AddRange(emailMessage.ToAddresses.Select(m => new MailboxAddress(m.Name, m.Address)));

    /// <summary>
    ///     Send email message.
    /// </summary>
    /// <param name="emailMessage">Message to send.</param>
    /// <param name="token">Cancellation token.</param>
    public async Task SendAsync(EmailMessage emailMessage, CancellationToken token)
    {
        var yy = string.Join("; ", emailMessage.ToAddresses.Select(m => $"{m.Name} <{m.Address}>"));

        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("from", $"{_options.DefaultSenderName} <{_options.DefaultSenderAddress}>"),
            new KeyValuePair<string, string>("to", "Yorick de Wid <ydewid@gmail.com>"),
            new KeyValuePair<string, string>("subject", emailMessage.Subject),
            // new KeyValuePair<string, string>("text", emailMessage.Content),
            new KeyValuePair<string, string>("template", "test-temp"),
            new KeyValuePair<string, string>("h:X-Mailgun-Variables", "{\"name\": \"Adolf\"}"),
        });

        var authenticationString = $"api:{_options.ApiKey}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_options.Domain}/messages");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
        requestMessage.Content = formContent;

        _logger.LogDebug($"Sending message to {string.Join(", ", emailMessage.ToAddresses)}");

        var response = await client.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation($"Message sent with success");
    }

    /// <summary>
    ///     Test the email service backend.
    /// </summary>
    public async Task HealthCheck()
    {
        await Task.CompletedTask;
    }
}
