using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Court
{
    public int CourtId { get; set; }

    public string CourtName { get; set; } = null!;

    public int SportId { get; set; }

    public string? Location { get; set; }

    public decimal? PricePerHour { get; set; }

    public string? Status { get; set; }

    public int? StatusId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<CourtReview> CourtReviews { get; set; } = new List<CourtReview>();

    public virtual Sport Sport { get; set; } = null!;

    public virtual CourtStatus? StatusNavigation { get; set; }
}
