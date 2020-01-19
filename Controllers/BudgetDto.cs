using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETChallengeWeb.Controllers
{
    public class BudgetDto
    {
        
        public string Name { get; set; }
        public int Year { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string UserId { get; set; }
        public List<BudgetCategoryDto> BudgetCategory { get; set; }

        internal BudgetCategoryDto CreateNewCustomCategory()
        {

            var newAmount = BudgetCategory.Sum(c => c.Amount);

            var newCat = new BudgetCategoryDto {IsNew=true, Name = $"Custom {BudgetCategory.Count(c => c.IsNew) + 1}" };

            if (newAmount > Amount)
            {
                Amount = newAmount;
            }else
            {
                var newpercentaje = BudgetCategory.Sum(c => c.Percentage);
                newCat.Amount = Amount - newAmount;
                decimal newCatPercentage = (100 - newpercentaje);
                newCat.Percentage = newCatPercentage < 0 ? 0 : newCatPercentage;
            }
            return newCat;

        }

        public void RedistributePercentages()
        {
            foreach (var item in BudgetCategory)
            {
                
                item.Amount = item.Amount == 0 && item.Percentage > 0 ? Math.Round(Amount * (item.Percentage/100),0) : item.Amount;
                item.Percentage = item.Amount != 0 ? Math.Round(Math.Round(item.Amount / Amount,2 )*100,0) : 0;
            }
        }

        internal CreateNewBudgetRequest AsMonthBudgetRequest()
        {
            return new CreateNewBudgetRequest
            {
                Amount = Amount,
                BudgetCategories = BudgetCategory,
                Currency = Currency,
                StartDate = StartDate,
                EndDate = EndDate,
                UserId = UserId,
            };
        }
    }
    public class BudgetCategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public bool IsNew { get; set; }
    }
    public class CategoryResponse
    {
        public string ValidationMessage { get; set; }
        public int CategoryId { get; set; }
    }
    public class CreateNewBudgetRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserId { get; set; }
        public IEnumerable<BudgetCategoryDto> BudgetCategories { get; set; }
    }
}
