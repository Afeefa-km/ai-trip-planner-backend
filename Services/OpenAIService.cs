using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TripPlanner.Api.Configuration;
using TripPlanner.Api.Services.Interfaces;

namespace TripPlanner.Api.Services;

public class OpenAIService : IAIProviderService
{
    private readonly HttpClient _httpClient;
    private readonly AISettings _aiSettings;
    public OpenAIService(HttpClient httpClient, IOptions<AISettings> aiSettings)
    {
        _httpClient = httpClient;
        _aiSettings = aiSettings.Value;
    }

    public async Task<string> GenerateTripPlanAsync(string prompt)
    {
        var requestBody = new
        {
            model = _aiSettings.Model,
            messages = new[]
            {
                new { role = "system", content = "You are a professional travel planner." },
                new { role = "user", content = prompt }
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, _aiSettings.BaseUrl);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", _aiSettings.ApiKey);

        // OpenRouter required headers
        request.Headers.Add("HTTP-Referer", "http://localhost:5187");
        request.Headers.Add("X-Title", "AI Trip Planner");

        request.Content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        using var response = await _httpClient.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(
                $"OpenRouter error ({response.StatusCode}): {content}");

        using var doc = JsonDocument.Parse(content);

        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;
    }
}


