namespace TripPlanner.Api.Models;

public class TripPlanResponse
{
    public TripPlanResponse(string destination, int numberOfDays, List<TripPlanDto> tripPlan)
    {
        Destination = destination;
        TotalDays = numberOfDays;
        TripPlan = tripPlan;
    }

    public string Destination { get; set; } = string.Empty;
    public int TotalDays { get; set; }
    public List<TripPlanDto> TripPlan { get; set; } = new();
}