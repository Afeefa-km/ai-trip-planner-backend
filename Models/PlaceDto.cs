namespace TripPlanner.Api.Models;

public class PlaceDto
{
    public TimeDto Time { get; set; } = new();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public LocationDto Location { get; set; } = new();
}


public class TimeDto
{
    public string Start { get; set; } = string.Empty;
    public string End { get; set; } = string.Empty;
}


public class LocationDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
