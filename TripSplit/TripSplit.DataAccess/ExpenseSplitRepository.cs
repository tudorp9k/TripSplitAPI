using TripSplit.Domain;
using TripSplit.Domain.Interfaces;

namespace TripSplit.DataAccess
{
    public class ExpenseSplitRepository : IExpenseSplitRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseSplitRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddExpenseSplit(ExpenseSplit expenseSplit)
        {
            await _context.ExpenseSplits.AddAsync(expenseSplit);
            await _context.SaveChangesAsync();
        }

        public async Task AddExpenseSplits(List<ExpenseSplit> expenseSplits)
        {
            foreach (ExpenseSplit expenseSplit in expenseSplits)
            {
                await _context.ExpenseSplits.AddAsync(expenseSplit);
            }
        }
    }
}
