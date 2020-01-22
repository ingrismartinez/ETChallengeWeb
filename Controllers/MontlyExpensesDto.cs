using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETChallengeWeb.Controllers
{
    public class MontlyExpensesDto
    {
        public string BudgetName { get; set; }
        public string Currency { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal BudgetAmount { get; set; }
        public bool OutOfBudget { get; set; }
    }
    public class TotalExpenseDto
    {
        public string CategoryName { get; set; }
        public string Currency { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal BudgetAmount { get; set; }
        public bool OutOfBudget { get; set; }
    }
}
