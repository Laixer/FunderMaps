using System.Net.Http.Headers;
using System.Text.Json;
using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Core.ExternalServices;

internal class MailgunService(IOptions<MailgunOptions> options, ILogger<MailgunService> logger) : IEmailService
{
    private const string DefaultBaseUrl = @"https://api.eu.mailgun.net/v3/";

    private readonly MailgunOptions _options = options.Value;
    private readonly HttpClient client = new() { BaseAddress = new(DefaultBaseUrl) };

    /// <summary>
    ///     Send email message.
    /// </summary>
    /// <param name="emailMessage">Message to send.</param>
    /// <param name="token">Cancellation token.</param>
    public async Task SendAsync(EmailMessage emailMessage, CancellationToken token)
    {
        if (!emailMessage.ToAddresses.Any())
        {
            emailMessage.ToAddresses = [new(_options.DefaultRecipientAddress ?? throw new InvalidOperationException("DefaultRecipientAddress not configured"), _options.DefaultRecipientName)];
        }

        foreach (var recipient in emailMessage.ToAddresses)
        {
            await SendMailAsync(recipient, emailMessage, token);
        }
    }

    public async Task SendMailAsync(EmailAddress recipient, EmailMessage emailMessage, CancellationToken token)
    {
        List<KeyValuePair<string, string>> formContent =
        [
            new("from", $"{_options.DefaultSenderName} <{_options.DefaultSenderAddress}>"),
            new("to", $"{recipient.Name} <{recipient.Address}>"),
            new("subject", emailMessage.Subject ?? throw new ArgumentException("Email must have subject", nameof(emailMessage))),
        ];

        if (emailMessage.Template is not null)
        {
            formContent.Add(new("template", emailMessage.Template));
        }
        else if (emailMessage.Content is not null)
        {
            formContent.Add(new("text", emailMessage.Content));
        }
        else
        {
            throw new ArgumentException("No content specified, provide either Template or Content", nameof(emailMessage));
        }

        if (emailMessage.Variables.Any())
        {
            formContent.Add(new("h:X-Mailgun-Variables", JsonSerializer.Serialize(emailMessage.Variables)));
        }

        var authenticationString = $"api:{_options.ApiKey}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_options.Domain}/messages");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
        requestMessage.Content = new FormUrlEncodedContent(formContent);

        logger.LogDebug("Sending message to {Name} <{Address}>", recipient.Name, recipient.Address);

        var response = await client.SendAsync(requestMessage, token);
        response.EnsureSuccessStatusCode();

        logger.LogDebug("Message sent with success");
    }

    /// <summary>
    ///     Test the email service backend.
    /// </summary>
    public Task HealthCheck() => Task.CompletedTask;
}
