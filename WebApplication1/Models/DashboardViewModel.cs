using RupeeRoute.Web.ViewModels;

namespace RupeeRoute.Web.Models
{
    public class DashboardViewModel
    {
        public decimal MonthlyExpenses { get; set; }
        public decimal MonthlySavings { get; set; }

        public decimal TotalExpenses { get; set; }
        public decimal TotalSavings { get; set; }
        public decimal AvailableBalance { get; set; }
        public List<ExpenseByViewModel> ExpensesByCategory { get; set; } = new();

        // Monthly expense summary for current year
        public List<TransactionViewModel> RecentTransactions { get; set; } = new();



    }
}
