using System;
using System.Collections.Generic;

namespace RupeeRoute.API.Models;

public partial class ExpenseCategory
{
    public int CategoryId { get; set; }

    public int? UserId { get; set; }

    public string CategoryName { get; set; } = null!;

    public bool? IsDefault { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual User? User { get; set; }
}
