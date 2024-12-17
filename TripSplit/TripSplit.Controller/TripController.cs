using Microsoft.AspNetCore.Mvc;
using TripSplit.Application;
using TripSplit.Domain;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ITripService tripService;

        public TripController(ITripService tripService)
        {
            this.tripService = tripService ?? throw new ArgumentNullException(nameof(tripService));
        }

        [HttpGet("get-trip-history")]
        public async Task<IActionResult> GetTrips(string userId)
        {
            var trips = await tripService.GetUserTrips(userId);
            return Ok(trips);
        }

        [HttpPost("add-user-to-trip")]
        public async Task<IActionResult> AddUserToTrip(string userId, int tripId)
        {
            await tripService.AddUserToTrip(userId, tripId);
            return Ok(new { Message = "User added to trip successfully!" });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripDto trip)
        {
            await tripService.CreateTrip(trip);
            return Ok(new { Message = "Trip created successfully!" });
        }
    }
}
