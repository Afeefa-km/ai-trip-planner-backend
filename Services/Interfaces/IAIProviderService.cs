namespace TripPlanner.Api.Services.Interfaces;

public interface IAIProviderService
{
    Task<string> GenerateTripPlanAsync(string prompt);
}