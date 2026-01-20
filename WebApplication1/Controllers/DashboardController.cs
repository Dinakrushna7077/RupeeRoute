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

            var expenses = await api.GetExpenses(userId);

            return PartialView("_Expenses",expenses);
        }
        public async Task<IActionResult> Saving()
        {
            int userId = 3; // later from login/session

            var savings = await api.GetSavings(userId);

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



    }
}
