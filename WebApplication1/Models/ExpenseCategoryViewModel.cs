namespace RupeeRoute.Web.Models
{
    public class ExpenseCategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public DateTime? CreatedOn { get; set; }
    }
}
