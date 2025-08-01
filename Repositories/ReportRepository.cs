using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class ReportRepository : IReportRepository
    {
        public List<RevenueReport> GetDailyRevenue()
        {
            using (var context = new SportsBookingDbContext())
            {
                var rawData = (from b in context.Bookings
                               join c in context.Courts on b.CourtId equals c.CourtId
                               where c.PricePerHour != null // Filter out courts without price
                               select new
                               {
                                   b.BookingDate,
                                   b.StartTime,
                                   b.EndTime,
                                   Price = c.PricePerHour ?? 0m // Use null-coalescing to handle nullable price
                               }).ToList();

                var result = rawData
                    .GroupBy(x => x.BookingDate)
                    .Select(g => new RevenueReport
                    {
                        DateOrMonth = g.Key.ToString("yyyy-MM-dd"),
                        TotalRevenue = g.Sum(x =>
                        {
                            var duration = x.EndTime - x.StartTime;
                            var hours = Math.Max(0, duration.TotalHours); // Ensure non-negative duration
                            return (decimal)hours * x.Price;
                        })
                    })
                    .OrderBy(r => r.DateOrMonth)
                    .ToList();

                return result;
            }
        }


        public List<RevenueReport> GetMonthlyRevenue()
        {
            using (var context = new SportsBookingDbContext())
            {
                var rawData = (from b in context.Bookings
                               join c in context.Courts on b.CourtId equals c.CourtId
                               where c.PricePerHour != null
                               select new
                               {
                                   b.BookingDate,
                                   b.StartTime,
                                   b.EndTime,
                                   Price = c.PricePerHour ?? 0m
                               }).ToList();

                var result = rawData
                    .GroupBy(x => new { x.BookingDate.Year, x.BookingDate.Month })
                    .Select(g => new RevenueReport
                    {
                        DateOrMonth = $"{g.Key.Year}-{g.Key.Month:D2}",
                        TotalRevenue = g.Sum(x =>
                        {
                            var duration = x.EndTime - x.StartTime;
                            var hours = Math.Max(0, duration.TotalHours);
                            return (decimal)hours * x.Price;
                        })
                    })
                    .OrderBy(r => r.DateOrMonth)
                    .ToList();

                return result;
            }
        }

    }
}
