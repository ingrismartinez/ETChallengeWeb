using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETChallengeWeb.Models
{
    public class BudgetExpensesModel
    {
        public string BudgetName { get; set; }
        public string UserId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public List<BudgetCategory> Detail { get; set; }
    }
    public class BudgetCategory
    {
        public int Id { get; set; }
        public decimal Percentage { get; set; }
        public decimal ExpendedPercentage { get; set; }
        public decimal ExpendedAmount { get; set; }
        public List<Expense> Expenses { get; set; }
        public string Name { get; set; }
        public bool IsOk()
        {
            return ExpendedPercentage < 25 && ExpendedPercentage > 0;
        }
        public bool IsWarning()
        {
            return ExpendedPercentage < 75 && ExpendedPercentage > 0;
        }
        public bool IsExpendingMore()
        {
            return ExpendedPercentage >= 75 && ExpendedPercentage > 0;
        }
        
    }
    public class Expense
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
    }
}
