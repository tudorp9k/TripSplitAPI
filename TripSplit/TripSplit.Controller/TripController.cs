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
        public async Task<IActionResult> AddUserToTrip(AddUserToTripRequest request)
        {
            var userId = request.UserId;
            var tripId = request.TripId;
            await tripService.AddUserToTrip(userId, tripId);
            return Ok(new { Message = "User added to trip successfully!" });
        }

        [HttpDelete("remove-trip")]
        public async Task<IActionResult> RemoveTrip(int tripId)
        {
            await tripService.RemoveTrip(tripId);
            return Ok(new { Message = "Trip removed successfully!" });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripDto trip)
        {
            var tripId = await tripService.CreateTrip(trip);
            return Ok(new { TripId = tripId });
        }

        [HttpPut("set-trip-owner")]
        public async Task<IActionResult> SetTripOwner(int tripId, string userId)
        {
            await tripService.SetTripOwner(userId, tripId);
            return Ok(new { Message = "Trip owner set successfully!" });
        }
    }
}
