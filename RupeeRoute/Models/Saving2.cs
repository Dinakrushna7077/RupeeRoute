using System;
using System.Collections.Generic;

namespace RupeeRoute.API.Models;

public partial class Saving2
{
    public int SavingId { get; set; }

    public int UserId { get; set; }

    public byte[] EncryptedAmount { get; set; } = null!;

    public byte[]? EncryptedNote { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual User User { get; set; } = null!;
}
