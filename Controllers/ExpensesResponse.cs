using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETChallengeWeb.Controllers
{
    public class ExpensesResponse:ResponseBase
    {
        public List<ExpenseDto> Expenses { get; set; }

        
    }
}
