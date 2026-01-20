using System;
using System.Collections.Generic;

namespace RupeeRoute.API.Models;

public partial class Expense
{
    public int ExpenseId { get; set; }

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly ExpenseDate { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual ExpenseCategory Category { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
