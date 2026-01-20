using RupeeRoute.Web.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace RupeeRoute.Web.Models
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5196/api/")
            };
        }

        // LOGIN
        public async Task<(bool success, string response)> LoginUser(string url, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            return (response.IsSuccessStatusCode, responseBody);
        }

        // REGISTER
        public async Task<string?> RegisterUser(object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("users/register", content);

            if (response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<DashboardViewModel?> GetDashboard(int userId)
        {
            var response = await _client.GetAsync($"Expenses/home/{userId}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<DashboardViewModel>();
        }

        public async Task<List<ExpensesViewModel>> GetExpenses(int userId)
        {
            var response = await _client.GetAsync($"Expenses/expense/{userId}");

            if (!response.IsSuccessStatusCode)
                return new List<ExpensesViewModel>();

            return await response.Content.ReadFromJsonAsync<List<ExpensesViewModel>>()
                   ?? new List<ExpensesViewModel>();
        }
        public async Task<List<SavingViewModel>> GetSavings(int userId)
        {
            var response = await _client.GetAsync($"Expenses/saving/{userId}");

            if (!response.IsSuccessStatusCode)
                return new List<SavingViewModel>();

            return await response.Content.ReadFromJsonAsync<List<SavingViewModel>>()
                   ?? new List<SavingViewModel>();
        }
        public async Task<List<ExpenseCategoryViewModel>> GetCategories(int userId)
        {
            var response = await _client.GetAsync($"Expenses/categories/{userId}");

            if (!response.IsSuccessStatusCode)
                return new List<ExpenseCategoryViewModel>();

            return await response.Content.ReadFromJsonAsync<List<ExpenseCategoryViewModel>>()
                   ?? new List<ExpenseCategoryViewModel>();
        }
        public async Task<bool> AddExpense(AddExpenseViewModel model)
        {
            var response = await _client.PostAsJsonAsync("Expenses/addexpense", model);

            return response.IsSuccessStatusCode;
        }






    }
}
