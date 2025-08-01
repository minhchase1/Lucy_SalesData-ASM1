using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Sport
{
    public int SportId { get; set; }

    public string SportName { get; set; } = null!;

    public virtual ICollection<Court> Courts { get; set; } = new List<Court>();
    public virtual ICollection<TournamentRequest> TournamentRequests { get; set; }

    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}
