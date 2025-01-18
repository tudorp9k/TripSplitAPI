using TripSplit.Domain;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Application
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository expenseRepository;
        private readonly IExpenseSplitRepository expenseSplitRepository;

        public ExpenseService(IExpenseRepository expenseRepository, IExpenseSplitRepository expenseSplitRepository)
        {
            this.expenseRepository = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
            this.expenseSplitRepository = expenseSplitRepository ?? throw new ArgumentNullException(nameof(expenseSplitRepository));
        }

        public async Task<CreateExpenseResponse> CreateExpense(CreateExpenseDto createExpenseDto)
        {
            var expense = MappingProfile.CreateExpenseDtoToExpense(createExpenseDto);
            var expenseId = await expenseRepository.AddExpense(expense);
            var response = new CreateExpenseResponse
            {
                ExpenseId = expenseId
            };
            return response;
        }

        public async Task<GetExpensesResponse> GetExpensesByTripId(int tripId)
        {
            var expenses = await expenseRepository.GetExpensesByTripId(tripId);
            var expensesDto = expenses.Select(e => MappingProfile.ExpenseToExpenseDto(e));
            var expensesResponse = new GetExpensesResponse
            {
                Expenses = expensesDto
            };
            return expensesResponse;
        }

        public async Task SplitExpense(int expenseId, Dictionary<string, decimal> userSplits, bool isEqualSplit)
        {
            var expense = await expenseRepository.GetExpenseById(expenseId);
            if (expense == null)
            {
                throw new Exception("Expense not found");
            }

            var splits = new List<ExpenseSplit>();
            if (isEqualSplit)
            {
                var selectedUsers = userSplits.Keys.ToList();
                var equalAmount = Math.Round(expense.Amount / selectedUsers.Count, 2);
                foreach (var userId in selectedUsers)
                {
                    splits.Add(new ExpenseSplit
                    {
                        ExpenseId = expenseId,
                        UserId = userId,
                        Amount = equalAmount,
                        IsPaid = false
                    });
                }

                var remainingAmount = expense.Amount - splits.Sum(s => s.Amount);
                if (remainingAmount > 0)
                {
                    splits.First().Amount += remainingAmount;
                }
            }
            else
            {
                var totalSplitAmount = userSplits.Values.Sum();
                if (Math.Abs(totalSplitAmount - expense.Amount) > 0.01m)
                {
                    throw new Exception("Split amounts do not match the total expense amount.");
                }

                foreach (var userSplit in userSplits)
                {
                    splits.Add(new ExpenseSplit
                    {
                        ExpenseId = expenseId,
                        UserId = userSplit.Key,
                        Amount = userSplit.Value,
                        IsPaid = false
                    });
                }
            }

            await expenseSplitRepository.AddExpenseSplits(splits);
        }

        public async Task<Dictionary<string, decimal>> GetExpensesSplitByExpenseId(int expenseId)
        {
            var splits = await expenseSplitRepository.GetExpenseSplitsByExpenseId(expenseId);
            var userSplits = new Dictionary<string, decimal>();
            foreach (var split in splits)
            {
                userSplits.Add(split.UserId, split.Amount);
            }
            return userSplits;
        }
    }
}
