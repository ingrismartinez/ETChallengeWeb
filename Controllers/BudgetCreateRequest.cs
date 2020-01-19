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
}
