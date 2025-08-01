using BusinessObjects;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class TournamentRequestRepository : ITournamentRequestRepository
    {
        SportsBookingDbContext _context;
        public TournamentRequestRepository(SportsBookingDbContext context)
        {
            _context = context;
        }
        public List<TournamentRequest> GetAllRequests()
        {
            return _context.TournamentRequests
                          .Include(r => r.User)
                          .Include(r => r.Sport)
                          .OrderByDescending(r => r.RequestDate)
                          .ToList();
        }

        // ✅ Lấy đơn theo trạng thái (Pending, Approved, Rejected...)
        public List<TournamentRequest> GetRequestsByStatus(string status)
        {
            return _context.TournamentRequests
                          .Include(r => r.User)
                          .Include(r => r.Sport)
                          .Where(r => r.Status == status)
                          .OrderByDescending(r => r.RequestDate)
                          .ToList();
        }

        // ✅ Lấy đơn theo ID
        public TournamentRequest? GetRequestById(int id)
        {
            return _context.TournamentRequests
                          .Include(r => r.User)
                          .Include(r => r.Sport)
                          .FirstOrDefault(r => r.RequestId == id);
        }

        // ✅ Lấy đơn của user hiện tại
        public List<TournamentRequest> GetRequestsByUserId(int userId)
        {
            return _context.TournamentRequests
                          .Include(r => r.Sport)
                          .Where(r => r.UserId == userId)
                          .OrderByDescending(r => r.RequestDate)
                          .ToList();
        }

        // ✅ Tạo mới đơn đăng ký
        public bool AddRequest(TournamentRequest request)
        {
            _context.TournamentRequests.Add(request);
            return _context.SaveChanges() > 0;
        }

        // ✅ Cập nhật đơn (toàn bộ)
        public bool UpdateRequest(TournamentRequest request)
        {
            _context.TournamentRequests.Update(request);
            return _context.SaveChanges() > 0;
        }

        // ✅ Chỉ cập nhật trạng thái đơn
        public bool UpdateRequestStatus(int requestId, string newStatus)
        {
            var request = _context.TournamentRequests.FirstOrDefault(r => r.RequestId == requestId);
            if (request == null) return false;

            request.Status = newStatus;
            return _context.SaveChanges() > 0;
        }
    }
}
