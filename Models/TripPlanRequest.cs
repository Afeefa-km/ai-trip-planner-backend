namespace TripPlanner.Api.Models;

public class TripPlanRequest
{
    public string Destination { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public int NumberOfDays { get; set; }
    //public string? Preferences { get; set; } 
}