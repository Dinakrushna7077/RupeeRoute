using System.Text.Json.Serialization;

namespace RupeeRoute.Web.Models
{
    public class ExpenseCategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        [JsonPropertyName("createdOn")]
        public DateTime? CreatedOn { get; set; }
    }
}
