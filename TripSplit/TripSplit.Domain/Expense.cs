namespace TripSplit.Domain
{
    public class Expense
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<ExpenseSplit> Splits { get; set; } = new List<ExpenseSplit>();
    }
}
