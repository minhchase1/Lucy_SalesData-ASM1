using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class CourtStatus
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<Court> Courts { get; set; } = new List<Court>();
}
