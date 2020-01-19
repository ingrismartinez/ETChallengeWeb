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

    }
}
