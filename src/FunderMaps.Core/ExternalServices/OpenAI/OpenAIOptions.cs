namespace FunderMaps.Core.ExternalServices.OpenAI;

/// <summary>
///     Options for the open AI service.
/// </summary>
public sealed record OpenAIOptions
{
    /// <summary>
    ///     Configuration section key.
    /// </summary>
    public const string Section = "OpenAI";

    /// <summary>
    ///     OpenAI API key.
    /// </summary>
    public string? ApiKey { get; set; }
}
