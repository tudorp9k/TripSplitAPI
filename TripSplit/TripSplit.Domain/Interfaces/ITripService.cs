﻿using TripSplit.Domain.Dto;

namespace TripSplit.Domain.Interfaces
{
    public interface ITripService
    {
        Task AddUserToTrip(string userId, int tripId);
        Task<IEnumerable<TripDto>> GetUserTrips(string userId);
        Task RemoveTrip(int tripId);
        Task SetTripOwner(string userId, int tripId);
        Task<TripDetailDto> GetTripDetails(int tripId);
        Task<int> CreateTrip(CreateTripDto trip, string userId);

    }
}