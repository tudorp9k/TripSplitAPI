using Microsoft.AspNetCore.Mvc;
using TripSplit.Domain.Dto;
using TripSplit.Domain.Interfaces;

namespace TripSplit.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            this.expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
        }

        [HttpPost("create")]
        public async Task<CreateExpenseResponse> CreateExpense(CreateExpenseDto createExpenseDto)
        {
            return await expenseService.CreateExpense(createExpenseDto);
        }

        [HttpGet("get-by-id")]
        public async Task<GetExpensesResponse> GetExpensesByTripId(int tripId)
        {
            var expenses = await expenseService.GetExpensesByTripId(tripId);
            return expenses;
        }

        [HttpPost("split")]
        public async Task SplitExpense(SplitExpensesRequest request)
        {
            var isEqualSplit = true;
            await expenseService.SplitExpense(request.ExpenseId, request.UserSplits, isEqualSplit);
        }

        [HttpGet("get-splits-by-expense-id")]
        public async Task<Dictionary<string, decimal>> GetExpensesSplitByExpenseId(int expenseId)
        {
            return await expenseService.GetExpensesSplitByExpenseId(expenseId);
        }
    }
}
