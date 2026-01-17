namespace TripPlanner.Api.Models;

public class TripPlanDto
{
    public int Day { get; set; }
    public string Date { get; set; } = string.Empty;
    public List<PlaceDto> Places { get; set; } = new();
}
