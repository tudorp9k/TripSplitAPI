using TripSplit.Domain;

namespace TripSplit.Domain.Interfaces
{
    public interface IExpenseSplitRepository
    {
        Task AddExpenseSplit(ExpenseSplit expenseSplit);
        Task AddExpenseSplits(List<ExpenseSplit> expenseSplits);
    }
}