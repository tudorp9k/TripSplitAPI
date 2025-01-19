using Microsoft.AspNetCore.Identity;
using TripSplit.Domain;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Application
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository expenseRepository;
        private readonly IExpenseSplitRepository expenseSplitRepository;
        private readonly UserManager<User> userManager;

        public ExpenseService(IExpenseRepository expenseRepository, IExpenseSplitRepository expenseSplitRepository, UserManager<User> userManager)
        {
            this.expenseRepository = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
            this.expenseSplitRepository = expenseSplitRepository ?? throw new ArgumentNullException(nameof(expenseSplitRepository));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
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
            var expensesDto = new List<ExpenseDto>();
            foreach (var expense in expenses)
            {
                string paidByName;
                if (expense.UserId == null)
                {
                    paidByName = "Unknown";
                }
                else
                {
                    var paidByUser = await userManager.FindByIdAsync(expense.UserId);
                    paidByName = $"{paidByUser.FirstName} {paidByUser.LastName}";
                }
                var expenseDto = MappingProfile.ExpenseToExpenseDto(expense);
                expenseDto.PaidBy = paidByName;
                expensesDto.Add(expenseDto);

                var splits = await expenseSplitRepository.GetExpenseSplitsByExpenseId(expense.Id);

                if (splits == null)
                {
                    continue;
                }

                foreach (var split in splits)
                {
                    var contributorUser = await userManager.FindByIdAsync(split.UserId);
                    var contributorName = $"{contributorUser.FirstName} {contributorUser.LastName}";

                    if (expenseDto.Contributors == null)
                    {
                        expenseDto.Contributors = new List<ContributorDto>();
                    }

                    expenseDto.Contributors.Add(new ContributorDto
                    {
                        Name = contributorName,
                        Amount = split.Amount,
                    });
                }
            }

            var expensesResponse = new GetExpensesResponse
            {
                Expenses = expensesDto
            };

            return expensesResponse;
        }

        public async Task SplitExpense(int expenseId, Dictionary<string, decimal> userSplits)
        {
            var expense = await expenseRepository.GetExpenseById(expenseId);
            if (expense == null)
            {
                throw new Exception("Expense not found");
            }

            var splits = new List<ExpenseSplit>();

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

            await expenseSplitRepository.AddExpenseSplits(splits);
        }

    }
}
