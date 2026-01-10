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

        var request = new HttpRequestMessage(HttpMethod.Post, _aiSettings.BaseUrl);
        
        // Authorization header
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _aiSettings.ApiKey);
        
        // REQUIRED by OpenRouter
        request.Headers.Add("HTTP-Referer", "http://localhost:5187");
        request.Headers.Add("X-Title", "AI Trip Planner");

        request.Content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"OpenRouter API error ({response.StatusCode}): {error}");
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        return doc
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;
    }
}