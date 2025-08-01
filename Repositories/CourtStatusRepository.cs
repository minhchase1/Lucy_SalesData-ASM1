using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class CourtStatusRepository : ICourtStatusRepository
    {
        private readonly SportsBookingDbContext _context;

        public CourtStatusRepository(SportsBookingDbContext context)
        {
            _context = context;
        }

        public List<CourtStatus> GetAll()
        {
            return _context.CourtStatuses.ToList();
        }
    }
}
