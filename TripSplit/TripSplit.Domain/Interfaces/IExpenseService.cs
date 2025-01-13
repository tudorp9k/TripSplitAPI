﻿using TripSplit.Domain.Dto;

namespace TripSplit.Domain.Interfaces
{
    public interface IExpenseService
    {
        Task<int> CreateExpense(CreateExpenseDto createExpenseDto);
        Task<GetExpensesResponse> GetExpensesByTripId(int tripId);
        Task SplitExpense(int expenseId, Dictionary<string, decimal> userSplits, bool isEqualSplit);
    }
}