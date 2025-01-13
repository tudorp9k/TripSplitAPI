using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Domain.Dto
{
    public class GetExpensesResponse
    {
        public IEnumerable<ExpenseDto> Expenses { get; set; }
    }
}
