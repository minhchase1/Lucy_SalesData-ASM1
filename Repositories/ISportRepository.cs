using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories
{
    public interface ISportRepository
    {
        List<Sport> GetAllSports();
        Sport? GetSportById(int id);
        void AddSport(Sport sport);
        void UpdateSport(Sport sport);
        void DeleteSport(int id);
    }
}
