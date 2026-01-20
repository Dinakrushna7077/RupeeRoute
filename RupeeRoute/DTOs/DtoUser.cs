namespace RupeeRoute.API.DTOs
{
    public class DtoUser
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsVaultEnabled { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public bool IsActive { get; set; }
    }

}
