using System.ComponentModel.DataAnnotations;

namespace RupeeRoute.API.DTOs
{
    public class DtoCreateUser
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }

    }

}
