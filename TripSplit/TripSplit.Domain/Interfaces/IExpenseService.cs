using TripSplit.Domain.Dto;

namespace TripSplit.Domain.Interfaces
{
    public interface IExpenseService
    {
        Task<CreateExpenseResponse> CreateExpense(CreateExpenseDto createExpenseDto);
        Task<GetExpensesResponse> GetExpensesByTripId(int tripId);
        Task<Dictionary<string, decimal>> GetExpensesSplitByExpenseId(int expenseId);
        Task SplitExpense(int expenseId, Dictionary<string, decimal> userSplits, bool isEqualSplit);
    }
}