namespace RupeeRoute.API.DTOs.Login
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
