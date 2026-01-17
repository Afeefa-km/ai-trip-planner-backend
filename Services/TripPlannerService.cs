
using System.Text.Json;
using Newtonsoft.Json;
using TripPlanner.Api.Models;
using TripPlanner.Api.Services.Interfaces;

namespace TripPlanner.Api.Services;

public class TripPlannerService : ITripPlannerService
{
    private readonly IAIProviderService _aIProviderService;
    public TripPlannerService(IAIProviderService aIProviderService)
    {
        _aIProviderService = aIProviderService;
    }

    public async Task<TripPlanResponse> CreateTripPlanAsync(TripPlanRequest request)
    {
        var aiResult = await GenerateAiTripPlan(request);
        var trimmed = aiResult.TrimStart();
        if (trimmed.StartsWith("\""))
        {
            aiResult = JsonConvert.DeserializeObject<string>(aiResult)!;
        }
        var tripPlanDto = JsonConvert.DeserializeObject<List<TripPlanDto>>(aiResult);

        return new TripPlanResponse(
            request.Destination,
            request.NumberOfDays,
            tripPlanDto ?? new List<TripPlanDto>()
        );
    }


    public async Task<string> GenerateAiTripPlan(TripPlanRequest request)
    {
        var prompt = $@"
            Generate a {request.NumberOfDays}-day trip plan for {request.Destination}
            starting {request.StartDate:yyyy-MM-dd}.

            STRICT OUTPUT RULES:
            - Output ONLY the JSON array.
            - Do NOT include any text before or after the JSON.
            - Do NOT include markdown, code fences, or explanations.
            - Do NOT wrap the JSON in quotes.

            JSON FORMAT:
            [
            {{
                ""day"": number,
                ""date"": ""yyyy-MM-dd"",
                ""places"": [
                {{
                    ""time"": {{ ""start"": ""HH:mm"", ""end"": ""HH:mm"" }},
                    ""name"": string,
                    ""description"": string,
                    ""location"": {{ ""latitude"": number, ""longitude"": number }}
                }}
                ]
            }}
            ]

            RULES:
            - 24-hour time (HH:mm)
            - No overlapping times
            - Include travel and food breaks
            - Visit duration: 60–120 minutes
            - Travel duration: 15–60 minutes
            - Day runs roughly 09:00–20:00
            ";

        return await _aIProviderService.GenerateTripPlanAsync(prompt);
    }
}