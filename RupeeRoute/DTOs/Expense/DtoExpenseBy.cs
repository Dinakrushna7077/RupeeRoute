namespace RupeeRoute.API.DTOs.Expense
{
    public class DtoExpenseBy
    {
        public int ExpenseById { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
