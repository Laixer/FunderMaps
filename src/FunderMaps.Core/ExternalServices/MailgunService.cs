using System.Net.Http.Headers;
using System.Text.Json;
using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Core.ExternalServices;

internal class MailgunService : IEmailService
{
    /// <summary>
    ///     Default base URL for the remote service.
    /// </summary>
    private const string DefaultBaseUrl = @"https://api.eu.mailgun.net/v3/";

    private readonly MailgunOptions _options;
    private readonly ILogger _logger;

    private readonly HttpClient client = new();

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
        if (!emailMessage.ToAddresses.Any())
        {
            emailMessage.ToAddresses = new List<EmailAddress>
            {
                new(_options.DefaultRecipientAddress ?? throw new ArgumentNullException(nameof(_options.DefaultRecipientAddress)), _options.DefaultRecipientName)
            };
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
            new("from", $"{_options.DefaultSenderName} <{_options.DefaultSenderAddress}>"),
            new("to", $"{recipient.Name} <{recipient.Address}>"),
            new("subject", emailMessage.Subject ?? throw new ArgumentException("Email must have subject", nameof(emailMessage))),
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
            throw new ArgumentException("No content specified, provide either Template or Content", nameof(emailMessage));
        }

        if (emailMessage.Varaibles.Any())
        {
            formContent.Add(new KeyValuePair<string, string>("h:X-Mailgun-Variables", JsonSerializer.Serialize(emailMessage.Varaibles)));
        }

        var authenticationString = $"api:{_options.ApiKey}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_options.Domain}/messages");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
        requestMessage.Content = new FormUrlEncodedContent(formContent);

        _logger.LogDebug("Sending message to {Name} <{Address}>", recipient.Name, recipient.Address);

        var response = await client.SendAsync(requestMessage, token);
        response.EnsureSuccessStatusCode();

        _logger.LogDebug("Message sent with success");
    }

    /// <summary>
    ///     Test the email service backend.
    /// </summary>
    public Task HealthCheck() => Task.CompletedTask;
}
