using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class UserProfile
{
    public int UserId { get; set; }

    public string? Avatar { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? Note { get; set; }

    public virtual User User { get; set; } = null!;
}
