namespace RupeeRoute.API.DTOs.Expense
{
    public class DtoAddExpense
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
    }
}
