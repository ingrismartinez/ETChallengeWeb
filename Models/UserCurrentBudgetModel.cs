using ETChallengeWeb.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETChallengeWeb.Models
{
    public class UserCurrentBudgetModel
    {
        public bool IsProposedBudget { get; set; }
        public string ValidationMessage { get; set; }
        public BudgetDto Budget { get; set; }

        public BudgetExpensesModel AsBudgetExpenseModel()
        {
            var budget= new BudgetExpensesModel
            {
                Amount = Budget.Amount,
                BudgetName = Budget.Name,
                Currency = Budget.Currency,
                UserId = Budget.UserId,
                Detail = Budget.BudgetCategory?.Select(c => new BudgetCategory
                {
                    Id = c.Id,
                    Percentage = c.Percentage,
                    Expenses = c.Expenses,
                    ExpendedAmount = (c.Expenses?.Sum(d => d.Value))??0,
                    Name= c.Name,
                    ExpendedPercentage = (((c.Expenses?.Sum(d => d.Value))??0) / c.Amount) * 100
                }).ToList(),
            };
            return budget;
        }
    }
}
