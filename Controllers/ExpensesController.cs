using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ETChallengeWeb.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ILogger<ExpensesController> _logger;

        public ExpensesController(ILogger<ExpensesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var context =HttpContext;
            return View();
        }
    }
}