using Microsoft.AspNetCore.Mvc;
using TripSplit.Application;
using TripSplit.DataAccess;
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
            try
            {
                // Just call the service
                await tripService.AddUserToTrip(userId, tripId);
                return Ok(new { Message = "User added to trip successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("remove-trip")]
        public async Task<IActionResult> RemoveTrip(int tripId)
        {
            await tripService.RemoveTrip(tripId);
            return Ok(new { Message = "Trip removed successfully!" });
        }

        
   

        [HttpPut("set-trip-owner")]
        public async Task<IActionResult> SetTripOwner(int tripId, string userId)
        {
            await tripService.SetTripOwner(userId, tripId);
            return Ok(new { Message = "Trip owner set successfully!" });

        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripDto trip, [FromQuery] string userId)
        {
            // e.g., the userId is passed as a query param: /Trip/create?userId=abc
            // or you can extract from JWT if you prefer
            try
            {
                var tripId = await tripService.CreateTrip(trip, userId);
                return Ok(new { TripId = tripId });
            }
            catch (Exception ex)
            {
                // Return a 400 with the error message
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("get-trip-details")]
        public async Task<IActionResult> GetTripDetails([FromQuery] int tripId)
        {
            // Example usage: GET /api/Trip/get-trip-details?tripId=123
            var detail = await tripService.GetTripDetails(tripId);
            return Ok(detail);
        }


    }
}
