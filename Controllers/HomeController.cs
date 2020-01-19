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
using System.Text;

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
            UserCurrentBudgetModel budget;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://expensestrackerservices3.azurewebsites.net/api/");
                //HTTP GET
                var responseTask = client.GetAsync("UserBudgets?userId=ingri");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();

                    budget = JsonConvert.DeserializeObject<UserCurrentBudgetModel>(readTask);
                    
                    if(!budget.IsProposedBudget)
                    {
                        return RedirectToAction("Index", "Expenses", budget.AsBudgetExpenseModel());
                    }
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
        [HttpPost]
        public async Task<IActionResult> CreateBudget(
            UserCurrentBudgetModel Model)
        {
            Model = await AddingEveryNewCategory(Model);
            if(!ModelState.IsValid)
            {
                return View("Index", Model);
            }
            using (var client = new HttpClient())
            {
                var request = Model.Budget.AsMonthBudgetRequest();
                client.BaseAddress = new Uri("https://expensestrackerservices3.azurewebsites.net");
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var responseTask = client.PostAsync("/api/UserBudgets/add-budget",content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();

                    var newModel= JsonConvert.DeserializeObject<UserCurrentBudgetModel>(readTask);
                    if(!string.IsNullOrWhiteSpace(newModel.ValidationMessage))
                    {
                        ModelState.AddModelError("ErrorModel", newModel.ValidationMessage);
                        
                        return View("Index", Model);
                    }
                    else{
                        Model = newModel;
                    }
                }
                else //web api sent error response 
                {
                    //log response status here..

                    Model.ValidationMessage = "Server error. Please contact administrator.";
                    ModelState.AddModelError("ErrorModel", Model.ValidationMessage);
                    return View("Index", Model);
                }
            }
            return RedirectToAction("Index",Model);
        }
        
        [HttpPost]
        public IActionResult AddCategory(
            UserCurrentBudgetModel Model)
        {
            var newCategory = Model.Budget.CreateNewCustomCategory();
            Model.Budget.BudgetCategory.Add(newCategory);
            Model.Budget.RedistributePercentages();
            ModelState.Clear();
            return View("Index",Model);
        }
        [HttpPost]
        public IActionResult Refresh(
            UserCurrentBudgetModel Model)
        {
            Model.Budget.RedistributePercentages();
            ModelState.Clear();
            return View("Index", Model);
        }
        private async Task< UserCurrentBudgetModel> AddingEveryNewCategory(UserCurrentBudgetModel model)
        {
            foreach (var item in model.Budget.BudgetCategory.Where(c=>c.IsNew).ToList())
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://expensestrackerservices3.azurewebsites.net");
                    var request = new CategoryRequest { CategoryName = item.Name, userId = model.Budget.UserId };
                    var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                    var responseTask = client.PostAsync("/api/ExpenseCategories/add-custom", content);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = await result.Content.ReadAsStringAsync();

                        var newModel = JsonConvert.DeserializeObject<CategoryResponse>(readTask);
                        if (string.IsNullOrWhiteSpace(newModel.ValidationMessage))
                        {
                            item.CategoryId = newModel.CategoryId;
                            item.IsNew = false;
                        }else
                        {
                            ModelState.AddModelError("ErrorMessage", newModel.ValidationMessage);
                            break;
                        }
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError("ErrorModel", "Server error. Please contact administrator.");
   
                    }
                }
            }
            return model;
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
