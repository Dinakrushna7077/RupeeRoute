namespace RupeeRoute.API.DTOs.Expense
{
    public class DtoRecentTransaction
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = "";   // Expense / Saving
        public string Category { get; set; } = "";
        public string Note { get; set; } = "";
        public decimal Amount { get; set; }
    }

}
