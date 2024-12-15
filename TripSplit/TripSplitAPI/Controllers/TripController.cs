using Microsoft.AspNetCore.Mvc;
using TripSplit.Application;
using TripSplit.Domain;

[ApiController]
[Route("api/[controller]")]
public class TripController : ControllerBase
{
    private readonly TripService _tripService;

    public TripController(TripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetTrips()
    {
        // Sa se modifice cu contextu de auth
        int userId = 1; 
        var trips = await _tripService.GetUserTrips(userId);
        return Ok(trips);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTrip([FromBody] Trip trip)
    {
        // Sa se modifice cu contextu de auth
        trip.UserId = 1; //.
        await _tripService.CreateTrip(trip);
        return Ok(new { Message = "Trip created successfully!" });
    }


}
