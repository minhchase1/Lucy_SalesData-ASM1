using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class TournamentRegistration
{
    public int RegistrationId { get; set; }

    public int TournamentId { get; set; }

    public int UserId { get; set; }

    public DateTime? RegisterDate { get; set; }

    public string? Status { get; set; }

    public string? TeamName { get; set; }

    public int? NumberOfMembers { get; set; }

    public virtual Tournament Tournament { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
