namespace TripPlanner.Api.Models;

public class TripPlanResponse
{
    public TripPlanResponse(string destination, int numberOfDays, string aiResult)
    {
        Destination = destination;
        TotalDays = numberOfDays;
        AiGeneratedPlan = aiResult;
    }

    public string Destination { get; set; } = string.Empty;
    public int TotalDays { get; set; }
    public string AiGeneratedPlan { get; set; } = string.Empty;
}