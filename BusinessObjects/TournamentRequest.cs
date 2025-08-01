using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class TournamentRequest
{
    public int RequestId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public int SportId { get; set; }

    public string? Description { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public DateOnly? RegistrationDeadline { get; set; }

    public string? Location { get; set; }

    public bool? IsTeamBased { get; set; }

    public string? Rules { get; set; }

    public int? MaxParticipants { get; set; }

    public bool IsApproved { get; set; }

    public DateTime RequestDate { get; set; }

    public string? Status { get; set; } = null!;

    public virtual Sport Sport { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
