using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ETChallengeWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ETChallengeWeb.Controllers
{
    public class ReportsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            ReportsModel budget = new ReportsModel();
            if (TempData["ReportsModel"] is string s)
            {
               var model = JsonConvert.DeserializeObject<ReportsModel>(s);
                return View(model);
                // use newUser object now as needed
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://expensestrackerservices3.azurewebsites.net/api/");
                //HTTP GET
                var responseTask = client.GetAsync("Reports?userId=ingri");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();

                    var records = JsonConvert.DeserializeObject<List<ReportExpenseDto>>(readTask);
                    budget.Expenses = records;
                }
                else //web api sent error response 
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(budget);
        }
        [HttpGet]
        public async Task< IActionResult> DetailExpenses(string userId, string categoryId)
        {
            ReportsModel budget = new ReportsModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://expensestrackerservices3.azurewebsites.net/api/");
                //HTTP GET
                var responseTask = client.GetAsync($"Expenses?userId={userId}&categoryId={Convert.ToInt32(categoryId)}");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();

                    var records = JsonConvert.DeserializeObject<ExpensesResponse>(readTask);
                    var serializeReport = JsonConvert.SerializeObject(records.Expenses);
                    budget.Expenses = JsonConvert.DeserializeObject<List<ReportExpenseDto>>(serializeReport);
                    TempData["ReportsModel"] = Newtonsoft.Json.JsonConvert.SerializeObject(budget);
                    
                }
                else //web api sent error response 
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string expenseId,string userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://expensestrackerservices3.azurewebsites.net");
                var request = new ExpenseRequest { UserId = userId, ExpenseId = expenseId };
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var responseTask = client.DeleteAsync($"/api/Expenses/delete?userBudget.userId={userId}&userBudget.ExpenseId={expenseId}");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();

                    var response = JsonConvert.DeserializeObject<ExpensesResponse>(readTask);

                    if (response.IsValid())
                    {
                        return RedirectToAction("Index", "Reports");
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
            return RedirectToAction("Index", "Reports");
        }

        [HttpGet]
        public IActionResult Edit(string expenseId, string userId,string descripcion,decimal value,DateTime transactionDate)
        {
            return View("EditExpense",
                new ExpenseDto
                {
                    ExpenseId = expenseId,
                    UserId = userId,
                    Description = descripcion,
                    ExpendedValue = value,
                    TransactionDate = transactionDate
                });
            
        }
        [HttpPost]
        public async Task< IActionResult> Save(ExpenseDto expense)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://expensestrackerservices3.azurewebsites.net");
                var request = new ExpenseRequest
                {
                    UserId = expense.UserId,
                    ExpenseId = expense.ExpenseId,
                    Description = expense.Description,
                    ExpendedValue = expense.ExpendedValue,
                    TransactionDate = expense.TransactionDate
                };
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var responseTask = client.PutAsync($"/api/Expenses/edit",content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();

                    var response = JsonConvert.DeserializeObject<ExpensesResponse>(readTask);

                    if (!response.IsValid())
                    {
                        ModelState.AddModelError("ErrorMessage", response.ValidationMessage);
                    }
                }
                else //web api sent error response 
                {
                    ModelState.AddModelError("ErrorMessage", "Server error. Please contact administrator.");
                }
            }
            return RedirectToAction("Index", "Reports");
        }
    }
}