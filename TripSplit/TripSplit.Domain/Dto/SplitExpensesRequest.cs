namespace TripSplit.Domain.Dto
{
    public class SplitExpensesRequest
    {
        public int ExpenseId { get; set; }
        public Dictionary<string, decimal> UserSplits { get; set; }
    }
}
