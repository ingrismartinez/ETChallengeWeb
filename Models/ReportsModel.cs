using ETChallengeWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETChallengeWeb.Models
{
    public class ReportsModel
    {
        public List<ReportExpenseDto> Expenses { get; set; }
        public List<ReportExpenseDto> GetCurrentBudget()
        {
            return Expenses.GroupBy(c => c.BudgetStartDate).OrderByDescending(c => c.Key).FirstOrDefault()?.Select(c => c).ToList();
        }
        public List<MontlyExpensesDto> MonthlySpent { get; set; }
        public List<TotalExpenseDto> CategorySpent { get; set; }

        internal void CalculateMonthly()
        {
            MonthlySpent = Expenses.OrderBy(c => c.BudgetStartDate).GroupBy(c => new { c.BudgetName, c.BudgetAmount })
                .Select(c => new MontlyExpensesDto
                {
                    BudgetName = c.Key.BudgetName,
                    SpentAmount = c.Sum(d => d.ExpendedValue),
                    BudgetAmount = c.Key.BudgetAmount,
                    OutOfBudget = c.Sum(d => d.ExpendedValue) > c.Key.BudgetAmount
                }).ToList();
        }
    }
}
