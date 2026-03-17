using System.Text.Json;
using FunderMaps.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

[Route("api/pdf")]
public sealed class PdfController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : FunderMapsController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        var apiKey = configuration["PdfCo:ApiKey"];
        var reportBaseUrl = configuration["PdfCo:ReportUrl"] ?? "https://whale-app-nm9uv.ondigitalocean.app";

        if (string.IsNullOrEmpty(apiKey))
        {
            return StatusCode(503, new { message = "PDF service not configured" });
        }

        var client = httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromMinutes(2);
        client.DefaultRequestHeaders.Add("x-api-key", apiKey);

        var payload = JsonSerializer.Serialize(new
        {
            url = $"{reportBaseUrl}/{id}",
            name = $"{id}.pdf",
            paperSize = "A4",
            async = false,
        });

        using var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://api.pdf.co/v1/pdf/convert/from/url", content);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode(502, new { message = "PDF generation failed" });
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.TryGetProperty("error", out var error) && error.GetBoolean())
        {
            return StatusCode(502, new { message = "PDF generation failed" });
        }

        var url = doc.RootElement.GetProperty("url").GetString();

        return Ok(new { accessLink = url });
    }
}
