using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class CourtRepository : ICourtRepository
    {
        private readonly SportsBookingDbContext _context;

        public CourtRepository(SportsBookingDbContext context)
        {
            _context = context;
        }

        public List<Court> GetAllCourts()
        {
            return _context.Courts
                .Include(c => c.Sport)
                .ToList();
        }

        public Court? GetCourtById(int courtId)
        {
        
            return _context.Courts.FirstOrDefault(
                c => c.CourtId == courtId);
        }

        public List<Court> GetCourtsBySportId(int sportId)
        {
            return _context.Courts.Where(c => c.SportId
        == sportId).ToList();
        }

        public void Add(Court court)
        {
            _context.Courts.Add(court);
            _context.SaveChanges();
        }

        public void Update(Court court)
        {
            _context.Courts.Update(court);
            _context.SaveChanges();
        }

        public void Delete(int courtId)
        {
            var court = _context.Courts.Find(courtId);
            if (court != null)
            {
                _context.Courts.Remove(court);
                _context.SaveChanges();
            }
        }
    }
}
