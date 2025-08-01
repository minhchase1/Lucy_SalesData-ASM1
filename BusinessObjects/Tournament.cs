using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Tournament
{
    public int TournamentId { get; set; }

    public string Title { get; set; } = null!;

    public int SportId { get; set; }

    public string? Description { get; set; }

    public int OrganizerId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string? Location { get; set; }

    public string? Status { get; set; }

    public DateOnly? RegistrationDeadline { get; set; }

    public int? MaxParticipants { get; set; }

    public bool? IsTeamBased { get; set; }

    public string? Rules { get; set; }

    public virtual User Organizer { get; set; } = null!;

    public virtual Sport Sport { get; set; } = null!;

    public virtual ICollection<TournamentRegistration> TournamentRegistrations { get; set; } = new List<TournamentRegistration>();
}
