namespace RupeeRoute.Web.Models
{
    public class AddExpenseViewModel
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
    }
}
