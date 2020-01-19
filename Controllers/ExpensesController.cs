using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETChallengeWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ETChallengeWeb.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ILogger<ExpensesController> _logger;

        public ExpensesController(ILogger<ExpensesController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            BudgetExpensesModel model= new BudgetExpensesModel();
            if (TempData["BudgetExpensesModel"] is string s)
            {
                model = JsonConvert.DeserializeObject<BudgetExpensesModel>(s);
                // use newUser object now as needed
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult AddExpense( BudgetCategory category,BudgetExpensesModel model)
        {
            if (TempData["BudgetExpensesModel"] is string s)
            {
                model = JsonConvert.DeserializeObject<BudgetExpensesModel>(s);
                // use newUser object now as needed
            }
            return View(model);
        }
    }
}