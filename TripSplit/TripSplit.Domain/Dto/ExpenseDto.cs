namespace TripSplit.Domain.Dto
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string UserId { get; set; }
        public string PaidBy { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public List<ContributorDto> Contributors { get; set; }
    }
}
