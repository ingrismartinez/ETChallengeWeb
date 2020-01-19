using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETChallengeWeb.Controllers
{
    public class BudgetCreateRequest
    {
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
    public class CategoryRequest
    {
        public string CategoryName { get; set; }
        public string userId { get; set; }
    }
}
