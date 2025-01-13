using TripSplit.Domain;

namespace TripSplit.Domain.Interfaces
{
    public interface IExpenseRepository
    {
        Task<int> AddExpense(Expense expense);
        Task<Expense> GetExpenseById(int expense);
        Task<IEnumerable<Expense>> GetExpensesByTripId(int tripId);
        Task RemoveExpense(Expense expense);
        Task UpdateExpense(Expense expense);
    }
}