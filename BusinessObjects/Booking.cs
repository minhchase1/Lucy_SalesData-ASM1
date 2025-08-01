using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BusinessObjects;

public partial class Booking
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int CourtId { get; set; }

    public DateOnly BookingDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual Court Court { get; set; } = null!;
    public virtual User User { get; set; } = null!;


}
