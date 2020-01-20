using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETChallengeWeb.Controllers
{
    public class ReportExpenseDto:ExpenseDto
    {
        public bool Editing { get; set; }
        public string CategoryName { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal CategoryBudgetAmount { get; set; }
        public decimal CategoryExpendedAmount { get; set; }
        public decimal CategoryPercentage { get; set; }
        public decimal CategoryExpendedPercentage { get; set; }
        public string BudgetName { get; set; }
        public DateTime BudgetStartDate { get; set; }
        public DateTime BudgetEndDate { get; set; }
    }
}
