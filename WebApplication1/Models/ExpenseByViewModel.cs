namespace RupeeRoute.Web.Models
{
    public class ExpenseByViewModel
    {
        public int ExpenseById { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
