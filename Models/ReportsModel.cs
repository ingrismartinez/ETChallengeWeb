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
    }
}
