using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using RupeeRoute.API.DTOs;
using RupeeRoute.API.DTOs.Expense;
using RupeeRoute.API.Models;

namespace RupeeRoute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly RupeeRouteDbContext _context;
        public ExpensesController(RupeeRouteDbContext context)
        {
            _context = context;
        }
        //Dashboard controller method
        [HttpGet("home/{userId}")]
        public async Task<IActionResult> GetDashboard(int userId)
        {
            var now = DateTime.Now;

            // 🔹 TOTAL EXPENSES
            var totalExpenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .Select(e => (decimal?)e.Amount)
                .SumAsync() ?? 0;

            // 🔹 TOTAL SAVINGS
            var totalSavings = await _context.Savings
                .Where(s => s.UserId == userId)
                .Select(s => (decimal?)s.Amount)
                .SumAsync() ?? 0;

            // 🔹 MONTHLY EXPENSES (CURRENT MONTH)
            var monthlyExpenses = await _context.Expenses
                .Where(e =>
                    e.UserId == userId &&
                    e.CreatedOn.HasValue &&
                    e.CreatedOn.Value.Month == now.Month &&
                    e.CreatedOn.Value.Year == now.Year)
                .Select(e => (decimal?)e.Amount)
                .SumAsync() ?? 0;

            // 🔹 MONTHLY SAVINGS (CURRENT MONTH)
            var monthlySavings = await _context.Savings
                .Where(s =>
                    s.UserId == userId &&
                    s.CreatedOn.HasValue &&
                    s.CreatedOn.Value.Month == now.Month &&
                    s.CreatedOn.Value.Year == now.Year)
                .Select(s => (decimal?)s.Amount)
                .SumAsync() ?? 0;

            // 🔹 AVAILABLE BALANCE
            var availableBalance = totalSavings - totalExpenses;

            // 🔹 EXPENSES BY CATEGORY
            var expensesByCategory = await _context.Expenses
                .Where(e => e.UserId == userId)
                .GroupBy(e => e.Category.CategoryName)
                .Select(g => new DtoExpenseBy
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            // 🔹 RECENT EXPENSES
            var recentExpenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.CreatedOn)
                .Take(5)
                .Select(e => new DtoRecentTransaction
                {
                    Date = e.CreatedOn ?? DateTime.Now,
                    Type = "Expense",
                    Category = e.Category.CategoryName,
                    Note = e.Description ?? "",
                    Amount = -e.Amount
                })
                .ToListAsync();

            // 🔹 RECENT SAVINGS
            var recentSavings = await _context.Savings
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedOn)
                .Take(5)
                .Select(s => new DtoRecentTransaction
                {
                    Date = s.CreatedOn ?? DateTime.Now,
                    Type = "Saving",
                    Category = "Saving",
                    Note = s.Note ?? "",
                    Amount = s.Amount
                })
                .ToListAsync();

            // 🔹 MERGE & SORT
            var RecentTransactions = recentExpenses
                .Concat(recentSavings)
                .OrderByDescending(x => x.Date)
                .Take(8)
                .ToList();

            return Ok(new DtoDashboard
            {
                TotalExpenses = totalExpenses,
                TotalSavings = totalSavings,
                MonthlyExpenses = monthlyExpenses,
                MonthlySavings = monthlySavings,
                AvailableBalance = availableBalance,
                ExpensesByCategory = expensesByCategory,
                RecentTransactions = RecentTransactions
            });
        }



        [HttpPost("create")]
        public async Task<IActionResult> AddExpense([FromBody] DtoExpenseCategory expense)
        {
            if (expense == null) 
                return BadRequest("Expense data is required.");
            ExpenseCategory category = new ExpenseCategory()
            {
                //CategoryId = expense.CategoryId,
                CategoryName = expense.CategoryName,
                CreatedOn = DateTime.Now,
                IsDefault = false,
                //UserId = expense.UserId
            };
            _context.ExpenseCategories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(expense);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] Expense expense)
        {
            var existing = await _context.Expenses.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Amount = expense.Amount;
            existing.CategoryId = expense.CategoryId;
            existing.Description = expense.Description;
            existing.CreatedOn = expense.CreatedOn;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        
        [HttpGet("expense/{userId}")]
        public async Task<IActionResult> GetExpenses(int userId,int limit)
        {
            var data = await _context.Expenses
                .Where(e => e.UserId == userId)
                .Take(limit)
                .Select(e => new
                {
                    e.ExpenseId,
                    ExpenseDate = e.CreatedOn,
                    CategoryName = e.Category.CategoryName,
                    e.Description,
                    e.Amount
                })
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();

            return Ok(data);
        }
        [HttpPost("addsaving/{userId}")]
        public async Task<IActionResult> AddSaving([FromBody] DtoCreateSaving dto,int userId)
        {
            if (dto.Amount <=0)
                return BadRequest("Invalid amount");
            var saving = new Saving
            {
                UserId = userId,
                Amount = dto.Amount,
                Note = dto.Note,
                CreatedOn = DateTime.Now
            };
            _context.Savings.Add(saving);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = dto.Amount+"|"+dto.Note });
        }
        [HttpGet("saving/{userId}")]
        public async Task<IActionResult> GetSavings(int userId,int limit)
        {
            var data = await _context.Savings
                .Where(e => e.UserId == userId).Take(limit)
                .Select(e => new
                {
                    SavingDate = e.CreatedOn,
                    Amount = e.Amount,
                    SavingNote = e.Note,
                })
                .OrderByDescending(e => e.SavingDate)
                .ToListAsync();

            return Ok(data);
        }
        [HttpGet("categories/{userId}")]
        public async Task<IActionResult> GetCategories(int userId)
        {
            var data = await _context.ExpenseCategories
                .Where(e => e.UserId == userId || e.IsDefault==true)
                .Select(e => new
                {
                    e.CategoryId,
                    CreatedOn = e.CreatedOn,
                    CategoryName = e.CategoryName,
                })
                .Take(10)
                .OrderBy(e => e.CreatedOn)
                .ToListAsync();

            return Ok(data);
        }
        [HttpPost("addexpense")]
        public async Task<IActionResult> AddExpense([FromBody] DtoAddExpense dto)
        {
            if (dto.Amount <= 0)
                return BadRequest("Invalid amount");

            var expense = new Expense
            {
                UserId = dto.UserId,
                Amount = dto.Amount,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                CreatedOn = DateTime.Now
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Expense added successfully" });
        }
        
        [HttpPost("addexpensecategory/{userId}")]
        public async Task<IActionResult> NewCategory(int userId,[FromBody] DtoCreateExpenseCategory dto)
        {
            var category = new ExpenseCategory
            {
                UserId= userId,
                CategoryName=dto.CategoryName,
                CreatedOn= DateTime.Now
            };

            _context.ExpenseCategories.Add(category);
            int x=await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Category Created Successfully" });
        }
        //Delete Category
        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existing = await _context.ExpenseCategories.FindAsync(id);
            if (existing == null) return NotFound();

            _context.ExpenseCategories.Remove(existing);
            await _context.SaveChangesAsync();
            return Ok();
        }
        //Delete Category
        [HttpDelete("delete-expense/{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var existing = await _context.Expenses.FindAsync(id);
            if (existing == null) return NotFound();

            _context.Expenses.Remove(existing);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
