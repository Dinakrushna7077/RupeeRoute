using Microsoft.AspNetCore.Mvc;
using RupeeRoute.Web.Models;
using RupeeRoute.Web.ViewModels;
using System.Threading.Tasks;

namespace RupeeRoute.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApiService api;

        public DashboardController()
        {
            api = new ApiService();
        }
        public IActionResult Index()
        {
            return View(); 
        }
        public async Task<IActionResult> Home()
        
        {
            int userId = 3; // replace later with logged-in user id

            var dashboard = await api.GetDashboard(userId);

            if (dashboard == null)
            {
                // Send EMPTY model instead of null
                dashboard = new DashboardViewModel();
                ViewBag.Error = "Could not load dashboard data";
            }

            return PartialView("_Dashbard", dashboard);
        }

        public async Task<IActionResult> Expenses()
        {
            int userId = 3; // later from login/session
            int limit = 10;
            var expenses = await api.GetExpenses(userId,limit);

            return PartialView("_Expenses",expenses);
        }
        public async Task<IActionResult> NewSaving([FromBody]SavingViewModel sav)
        {
            int userId = 3; // later from login/session
            int limit=10;
            var success = await api.AddSaving(userId,sav);
            var savings = await api.GetSavings(userId,limit);
            return PartialView("_Saving", savings);
        }
        public async Task<IActionResult> Saving()
        {
            int userId = 3; // later from login/session
            int limit = 10;
            var savings = await api.GetSavings(userId,limit);

            return PartialView("_Saving",savings);
        }
        public async Task<IActionResult> Categories()
        {
            int userId = 3; // later from login/session

            var categories = await api.GetCategories(userId);

            return PartialView("_Categories",categories);
        }
        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] AddExpenseViewModel model)
        {
            model.UserId = 3; // later from session/login

            var success = await api.AddExpense(model);

            if (!success)
                return BadRequest();

            // Reload dashboard partial after insert
            var dashboard = await api.GetDashboard(model.UserId);

            return PartialView("_Dashbard", dashboard);
        }
        public async Task<IActionResult> GetCategories()
        {
            int userId = 3; // later from login/session
            var categories = await api.GetCategories(userId);
            return Json(categories);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] ExpenseCategoryViewModel model)
        {
            int userId = 3; // later from session/login

            var success = await api.NewCategory(userId,model);

            if (!success)
                return BadRequest();

            // Reload dashboard partial after insert
            var categories = await api.GetCategories(userId);
            return PartialView("_Categories", categories);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await api.DeleteCategoryAsync(id);

            if (!result)
                return Json(new { success = false, message = "Delete failed" });

            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var result = await api.DeleteExpenseAsync(id);

            if (!result)
                return Json(new { success = false, message = "Delete failed" });

            return Json(new { success = true });
        }



    }
}
