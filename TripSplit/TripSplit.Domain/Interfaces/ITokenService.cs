using TripSplit.Domain;

namespace TripSplit.Domain.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user);
    }
}