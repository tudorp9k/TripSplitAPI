using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain;
using TripSplit.Domain.Interfaces;

namespace TripSplit.DataAccess
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Expense>> GetExpensesByTripId(int tripId)
        {
            return await _context.Expenses
                .Where(e => e.TripId == tripId)
                .ToListAsync();
        }

        public async Task<Expense> GetExpenseById(int expense)
        {
            return await _context.Expenses.FindAsync(expense);
        }

        public async Task<int> AddExpense(Expense expense)
        {
            var result = await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task RemoveExpense(Expense expense)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExpense(Expense expense)
        {
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }
    }
}
