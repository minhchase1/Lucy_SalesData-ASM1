using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class SportRepository : ISportRepository
    {
        private readonly SportsBookingDbContext _context;

        public SportRepository(SportsBookingDbContext context)
        {
            _context = context;
        }

        public List<Sport> GetAllSports()
        {
            return _context.Sports.ToList();
        }

        public Sport? GetSportById(int id)
        {
            return _context.Sports.FirstOrDefault(s => s.SportId == id);
        }

        public void AddSport(Sport sport)
        {
            _context.Sports.Add(sport);
            _context.SaveChanges();
        }

        public void UpdateSport(Sport sport)
        {
            _context.Sports.Update(sport);
            _context.SaveChanges();
        }

        public void DeleteSport(int id)
        {
            var sport = _context.Sports.FirstOrDefault(s => s.SportId == id);
            if (sport != null)
            {
                _context.Sports.Remove(sport);
                _context.SaveChanges();
            }
        }
    }
}

