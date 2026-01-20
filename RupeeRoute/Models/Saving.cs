using System;
using System.Collections.Generic;

namespace RupeeRoute.API.Models;

public partial class Saving
{
    public int SavingId { get; set; }

    public int UserId { get; set; }

    public long Amount { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual User User { get; set; } = null!;
}
