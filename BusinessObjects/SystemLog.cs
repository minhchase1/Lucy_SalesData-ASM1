using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class SystemLog
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? TableAffected { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User User { get; set; } = null!;
}
