namespace TripSplit.Domain
{
    public class ExpenseSplit
    {
        public int ExpenseId { get; set; }
        public Expense Expense { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
    }
}
