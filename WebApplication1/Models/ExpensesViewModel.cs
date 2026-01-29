namespace RupeeRoute.Web.Models
{
    public class ExpensesViewModel
    {
        public int ExpenseId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string CategoryName { get; set; } = "";
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public List<ExpenseByViewModel> ExpensesByCategory { get; set; } = new();

    }

}
