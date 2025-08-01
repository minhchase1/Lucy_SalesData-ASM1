using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ITournamentRepository
    {
        public List<Tournament> GetAllTournaments();
        public Tournament GetTournamentById(int id);
        public bool AddTournament(Tournament tournament);
        public bool UpdateTournament(Tournament tournament);
        public bool DeleteTournament(int id);
        bool UpdateTournamentStatuses();
        List<Tournament> SearchTournaments(string keyword);
    }
}
