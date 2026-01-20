namespace RupeeRoute.API.DTOs.Expense
{
    public class DtoDashboard
    {
        public decimal TotalExpenses { get; set; }
        public decimal TotalSavings { get; set; }

        public decimal MonthlyExpenses { get; set; }
        public decimal MonthlySavings { get; set; }

        public decimal AvailableBalance { get; set; }

        public List<DtoExpenseBy> ExpensesByCategory { get; set; } = new();
        public List<DtoRecentTransaction> RecentTransactions { get; set; } = new();
    }

}
