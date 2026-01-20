namespace RupeeRoute.Web.ViewModels
{
    public class TransactionViewModel
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = "";
        public string Category { get; set; } = "";
        public string Note { get; set; } = "";
        public decimal Amount { get; set; }
    }
}
