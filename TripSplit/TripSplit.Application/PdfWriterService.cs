using TripSplit.Domain.Interfaces;

namespace TripSplit.Application
{
    public class PdfWriterService : IPdfWriterService
    {
        private readonly IExpenseService expenseService;
        private readonly ITripService tripService;
        private readonly IUserService userService;

        public PdfWriterService(IExpenseService expenseService, ITripService tripService, IUserService userService)
        {
            this.expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
            this.tripService = tripService ?? throw new ArgumentNullException(nameof(tripService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<string> WritePdf(int tripId)
        {
            var trip = await tripService.GetTripDetails(tripId);
            var expenses = await expenseService.GetExpensesByTripId(tripId);
            var expensesSum = expenses.Expenses.Sum(e => e.Amount).ToString();
            var args = new NativeMethods.Args(trip.Destination, expensesSum);
            
            var fileName = $"Trip_{trip.Destination}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf";
            var filePath = Path.Combine("Reports", fileName);

            Directory.CreateDirectory("Reports");

            var groupedExpenses = expenses.Expenses
                .Where(e => e.UserId != null)
                .GroupBy(e => e.UserId);

            foreach (var group in groupedExpenses)
            {
                var userId = group.Key;
                if (userId != null)
                {
                    var user = await userService.GetUserById(userId);
                    var userExpenseSum = group.Sum(e => e.Amount).ToString();
                    var pdfUserExpense = args.CreateUserExpense(user.FirstName, userExpenseSum);

                    foreach (var expense in group)
                    {
                        pdfUserExpense.AddExpense(expense.Name, expense.Amount.ToString());
                    }
                }
            }

            args.Run(filePath);
            return filePath;
        }
    }
}
