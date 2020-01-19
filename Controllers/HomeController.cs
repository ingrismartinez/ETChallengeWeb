using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ETChallengeWeb.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace ETChallengeWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {

            UserCurrentBudgetModel budget; ;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://expensestrackerservices3.azurewebsites.net/api/");
                //HTTP GET
                var responseTask = client.GetAsync("UserBudgets?userId=ingris");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();

                    budget = JsonConvert.DeserializeObject<UserCurrentBudgetModel>(readTask);
                }
                else //web api sent error response 
                {
                    //log response status here..

                    budget = new UserCurrentBudgetModel();
                    budget.ValidationMessage = "Server error. Please contact administrator.";
                    ModelState.AddModelError(string.Empty, budget.ValidationMessage);
                }
            }
            return View(budget);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
