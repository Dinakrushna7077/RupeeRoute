using System;
using System.Collections.Generic;

namespace RupeeRoute.API.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? VaultPasswordHash { get; set; }

    public bool? IsVaultEnabled { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? LastLoginOn { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<ExpenseCategory> ExpenseCategories { get; set; } = new List<ExpenseCategory>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Saving2> Saving2s { get; set; } = new List<Saving2>();

    public virtual ICollection<Saving> Savings { get; set; } = new List<Saving>();
}
