
using Microsoft.AspNetCore.Mvc;
using TripPlanner.Api.Models;
using TripPlanner.Api.Services.Interfaces;

namespace TripPlanner.Api.Controllers;

[ApiController]
[Route("api/trip-planner")]
public class TripPlannerController : ControllerBase
{
    private readonly ITripPlannerService _tripPlannerService;

    public TripPlannerController(ITripPlannerService tripPlannerService)
    {
        _tripPlannerService = tripPlannerService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateTripPlan([FromBody] TripPlanRequest request)
    {
        try
        {
            var result = await _tripPlannerService.CreateTripPlanAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                error = "Internal Server Error",
                message = "An unexpected error occurred while generating the trip plan.",
                details = ex.Message
            });
        }
    }
}