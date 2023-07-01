using System.Net.Http.Headers;
using System.Text.Json;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Core.Email;

internal class MailgunService : IEmailService
{
    /// <summary>
    ///     Default base URL for the remote service.
    /// </summary>
    private const string DefaultBaseUrl = @"https://api.eu.mailgun.net/v3/";

    private readonly MailgunOptions _options;
    private readonly ILogger _logger;

    private HttpClient client = new();

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public MailgunService(IOptions<MailgunOptions> options, ILogger<MailgunService> logger)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        client.BaseAddress = new(DefaultBaseUrl);
    }

    /// <summary>
    ///     Send email message.
    /// </summary>
    /// <param name="emailMessage">Message to send.</param>
    /// <param name="token">Cancellation token.</param>
    public async Task SendAsync(EmailMessage emailMessage, CancellationToken token)
    {
        if (emailMessage is null)
        {
            throw new ArgumentNullException(nameof(emailMessage));
        }

        if (!emailMessage.ToAddresses.Any())
        {
            throw new ArgumentException("No recipients specified.", nameof(emailMessage));
        }

        foreach (var recipient in emailMessage.ToAddresses)
        {
            await SendMailAsync(recipient, emailMessage, token);
        }
    }

    public async Task SendMailAsync(EmailAddress recipient, EmailMessage emailMessage, CancellationToken token)
    {
        var formContent = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("from", $"{_options.DefaultSenderName} <{_options.DefaultSenderAddress}>"),
            new KeyValuePair<string, string>("to", $"{recipient.Name} <{recipient.Address}>"),
            new KeyValuePair<string, string>("subject", emailMessage.Subject ?? throw new ArgumentNullException(nameof(emailMessage.Subject))),
        };

        if (emailMessage.Template is not null)
        {
            formContent.Add(new KeyValuePair<string, string>("template", emailMessage.Template));
        }
        else if (emailMessage.Content is not null)
        {
            formContent.Add(new KeyValuePair<string, string>("text", emailMessage.Content));
        }
        else
        {
            throw new ArgumentNullException();
        }

        if (emailMessage.Varaibles.Any())
        {
            formContent.Add(new KeyValuePair<string, string>("h:X-Mailgun-Variables", JsonSerializer.Serialize(emailMessage.Varaibles)));
        }

        var authenticationString = $"api:{_options.ApiKey}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_options.Domain}/messages");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
        requestMessage.Content = new FormUrlEncodedContent(formContent);

        _logger.LogDebug($"Sending message to {recipient.Name} <{recipient.Address}>");

        var response = await client.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        _logger.LogDebug($"Message sent with success");
    }

    /// <summary>
    ///     Test the email service backend.
    /// </summary>
    public async Task HealthCheck() => await Task.CompletedTask;
}
