using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;



namespace Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly SportsBookingDbContext _context;
        public BookingRepository(SportsBookingDbContext context)
        {
            _context = context;
        }

        public bool ApproveBooking(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == id);
            if (booking == null)
                return false;
            booking.Status = "Confirmed";
            return _context.SaveChanges() > 0;
        }

        public bool DeleteBooking(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == id);
            if (booking == null)
                return false;
            _context.Bookings.Remove(booking);
            return _context.SaveChanges() > 0;
        }

       

        public Booking? GetBookingById(int id)
        {
            return _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Court)
                .FirstOrDefault(b => b.BookingId == id);

        }

        public List<Booking> GetBookingsByUserOrCourt(string keyword)
        {
            return _context.Bookings
                .Where(b => b.User.FullName.Contains(keyword) || b.Court.CourtName.Contains(keyword))
                .ToList();
        }

        public bool UpdateBooking(Booking booking)
        {
            _context.Update(booking);
            return _context.SaveChanges() > 0;
        }

        public bool AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            return _context.SaveChanges() > 0;
        }

        

        public List<Booking> GetAllBookings(bool includeUser)
        {
            if(includeUser)
            {
                return _context.Bookings.Include(b => b.User).Include(b => b.Court).ToList();
            }
            return _context.Bookings.ToList();
        }

        public List<CourtBookingStatistic> GetBookingStatisticsByCourt()
        {
            using var context = new SportsBookingDbContext();
            var data = context.Bookings
                .Include(b => b.Court)
                .GroupBy(b => b.Court.CourtName)
                .Select(g => new CourtBookingStatistic
                {
                    CourtName = g.Key,
                    BookingCount = g.Count()
                })
                .ToList();
            return data;
        }
    }
}
