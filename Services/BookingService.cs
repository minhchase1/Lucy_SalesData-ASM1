using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public bool AddBooking(Booking booking)
        {
            return _bookingRepository.AddBooking(booking);
        }

        public bool ApproveBooking(int id)
        {
            var booking = _bookingRepository.GetBookingById(id);
            if (booking == null)
                throw new ArgumentException("Booking not found");

            var duration = booking.EndTime - booking.StartTime;
            if (duration.TotalHours <= 0)
                throw new ArgumentException("Thời lượng đặt sân không hợp lệ");

            return _bookingRepository.ApproveBooking(id);
        }

        public List<Booking> GetAllBookings(bool includeUser = false)
        {
            return _bookingRepository.GetAllBookings(includeUser);
        }

        public Booking? GetBookingById(int id)
        {
            return _bookingRepository.GetBookingById(id);
        }

        public List<Booking> GetBookingsByUserOrCourt(string keyword)
        {
            var bookings = _bookingRepository.GetAllBookings(true);

            return bookings.Where(b =>
                (!string.IsNullOrEmpty(b.User?.FullName) && b.User.FullName.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(b.Court?.CourtName) && b.Court.CourtName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        public bool UpdateBooking(Booking booking)
        {
            return _bookingRepository.UpdateBooking(booking);
        }

        public bool DeleteBooking(int id)
        {
            return _bookingRepository.DeleteBooking(id);
        }

        public List<Booking> GetBookingsByCourtAndDate(int courtId, DateOnly date)
        {
            var bookings = _bookingRepository.GetAllBookings()
                .Where(b => b.CourtId == courtId
                         && b.BookingDate == date
                         && b.Status != "Cancelled")
                .ToList();

            return bookings;
        }

        public List<CourtBookingStatistic> GetBookingStatisticsByCourt()
        {
            return _bookingRepository.GetBookingStatisticsByCourt();
        }
    }
}
