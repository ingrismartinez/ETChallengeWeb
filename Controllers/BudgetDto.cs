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
    }
}
