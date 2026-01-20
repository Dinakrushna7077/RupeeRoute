using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RupeeRoute.API.DTOs;
using RupeeRoute.API.DTOs.Login;
using RupeeRoute.API.DTOs.Expense;
using RupeeRoute.API.Models;
using System.Security.Cryptography;
using System.Text;


namespace RupeeRoute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly RupeeRouteDbContext _context;
        public UsersController(RupeeRouteDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // 1️⃣ Validate input
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Email and password are required." });

            // 2️⃣ Check email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.IsActive == true);

            if (user == null)
                return Unauthorized(new { message = "Email does not exist." });

            // 3️⃣ Hash incoming password
            var incomingPasswordHash = ComputeSha256Hash(dto.Password);

            // 4️⃣ Compare password hash
            if (user.PasswordHash != incomingPasswordHash)
                return Unauthorized(new { message = "Incorrect password." });

            // 5️⃣ RupeeRoute response
            return Ok(new
            {
                userId = user.UserId,
                email = user.Email,
                //isVaultEnabled = user.IsVaultEnabled ?? false,
                message = "Login successful"
            });
        }
        private string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToBase64String(bytes);
        }
        [HttpPost("vault-login")]
        public async Task<IActionResult> VaultLogin(int userId, string vaultPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.IsVaultEnabled != true)
                return Unauthorized("Vault not enabled.");

            var vaultHash = ComputeSha256Hash(vaultPassword);

            if (user.VaultPasswordHash != vaultHash)
                return Unauthorized("Incorrect vault password.");

            return Ok("Vault access granted.");
        }
        //new user registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DtoCreateUser dto)
        {
            // 1️⃣ Validate input
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Email and password are required." });

            // 2️⃣ Check if email already exists
            var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailExists)
                return Conflict(new { message = "Email already registered." });

            // 3️⃣ Hash password
            if (dto.Password != dto.ConfirmPassword)
                return BadRequest(new { message = "Password and Confirm Password do not match." });
            var passwordHash = ComputeSha256Hash(dto.Password);

            // 4️⃣ Create new user
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                VaultPasswordHash = null,
                IsVaultEnabled = false,
                IsActive = true,
                CreatedOn = DateTime.Now
            };

            // 5️⃣ Save to DB
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                userId = user.UserId,
                message = "User registered successfully"
            });
        }
        [HttpPost("expense-categories")]
        public async Task<IActionResult> NewExpenseCategory(DtoCreateExpenseCategory dto)
        {
            // Check if user exists
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return NotFound(new { message = "User not found." });
            // Create new expense category
            var isExistingCategory = await _context.ExpenseCategories
                .AnyAsync(c => c.UserId == dto.UserId && c.CategoryName == dto.CategoryName);
            if (isExistingCategory)
                return Conflict(new { message = dto.CategoryName + " Expense category already exists." });

            var category = new ExpenseCategory
            {
                CategoryName = dto.CategoryName,
                UserId = dto.UserId,
                CreatedOn = DateTime.Now,
                IsDefault = false
            };
            // Save to DB
            _context.ExpenseCategories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                categoryId = category.CategoryId,
                message = "Expense category created successfully"
            });
        }
    }
 }
