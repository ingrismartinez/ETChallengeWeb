using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        [HttpPost]
        public async Task<IActionResult> AddExpense(BudgetExpensesModel Model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://expensestrackerservices3.azurewebsites.net");
                var request = Model.AsNewExpenseRequest();
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var responseTask = client.PostAsync("/api/Expenses/add", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();

                    var response = JsonConvert.DeserializeObject<ExpensesResponse>(readTask);

                    if (response.IsValid())
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("ErrorMessage", response.ValidationMessage);
                    }
                }
                else //web api sent error response 
                {
                    ModelState.AddModelError("ErrorMessage", "Server error. Please contact administrator.");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        
        
    }
}