using System.Text;
using System.Text.Json;
using FunderMaps.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

// TODO: This is a makeshift controller to demonstrate the PDF generation.
public class PdfController(IConfiguration configuration) : FunderMapsController
{
    [HttpGet("api/pdf/{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        var httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5),
            DefaultRequestHeaders =
            {
                { "x-api-key", configuration.GetSection("PdfCo:ApiKey").Value}
            }
        };

        try
        {
            string jsonBody = JsonSerializer.Serialize(new
            {
                url = $"https://whale-app-nm9uv.ondigitalocean.app/{id}",
                name = $"{id}.pdf",
                paperSize = "A4",
                async = false,
            });
            using var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("https://api.pdf.co/v1/pdf/convert/from/url", content);
            response.EnsureSuccessStatusCode(); // Throw an exception if the request failed

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var url = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse)["url"];

            return Ok(url);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        httpClient.Dispose();

        return Ok();
    }
}
