using System.ComponentModel.DataAnnotations;

namespace RupeeRoute.API.DTOs
{
    public class DtoCreateExpense
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        public string Description { get; set; }
    }

}
