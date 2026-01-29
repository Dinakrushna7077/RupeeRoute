using System.ComponentModel.DataAnnotations;

namespace RupeeRoute.API.DTOs
{
    public class DtoCreateExpenseCategory
    {
        public int? UserId { get; set; } // optional, null for default categories
        [Required]
        public string? CategoryName { get; set; }

        //public bool IsDefault { get; set; } = false;
    }

}
