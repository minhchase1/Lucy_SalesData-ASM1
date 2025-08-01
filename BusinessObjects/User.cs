using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public int RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Address { get; set; }
    public string Status { get; set; } = "Active";

    public DateOnly? Dob { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<CourtReview> CourtReviews { get; set; } = new List<CourtReview>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<SystemLog> SystemLogs { get; set; } = new List<SystemLog>();

    public virtual ICollection<TournamentRegistration> TournamentRegistrations { get; set; } = new List<TournamentRegistration>();

    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    public virtual ICollection<TournamentRequest> TournamentRequests { get; set; }
    public virtual UserProfile? UserProfile { get; set; }
}
