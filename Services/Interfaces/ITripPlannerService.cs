using TripPlanner.Api.Models;

namespace TripPlanner.Api.Services.Interfaces;

public interface ITripPlannerService
{
    Task<TripPlanResponse> CreateTripPlanAsync(TripPlanRequest request);
}
