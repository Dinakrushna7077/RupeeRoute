namespace RupeeRoute.API.DTOs
{
    public class DtoExpense
    {
        public int ExpenseId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}
