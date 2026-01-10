
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
        var prompt = $@"
            Create a {request.NumberOfDays}-day travel itinerary for {request.Destination}.
            Start date: {request.StartDate:yyyy-MM-dd}.
            Include daily activities and tips.";

        var aiResult = await _aIProviderService.GenerateTripPlanAsync(prompt);

        return new TripPlanResponse(
            request.Destination, request.NumberOfDays, aiResult
        );
    }
}