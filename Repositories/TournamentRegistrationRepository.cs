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
    public class TournamentRegistrationRepository : ITournamentRegistrationRepository
    {
        SportsBookingDbContext _context;
        public TournamentRegistrationRepository(SportsBookingDbContext context)
        {
            _context = context;
        }

        public bool RegisterUser(int userId, int tournamentId, string? teamName, int? numberOfMembers)
        {

            // Kiểm tra đã đăng ký chưa
            if (_context.TournamentRegistrations.Any(r => r.UserId == userId
                       && r.TournamentId == tournamentId
                       && r.Status != "Đã hủy"))
                return false;

            var tournament = _context.Tournaments.FirstOrDefault(t => t.TournamentId == tournamentId);
            if (tournament == null)
                return false;

            // Chuyển DateTime.Today sang DateOnly
            var today = DateOnly.FromDateTime(DateTime.Today);

            // Kiểm tra quá hạn đăng ký
            if (tournament.RegistrationDeadline != null && today > tournament.RegistrationDeadline)
                return false;

            // Kiểm tra giải đấu chưa bắt đầu
            if (tournament.StartDate != null && today >= tournament.StartDate)
                return false;

            // Tính số lượt đăng ký
            int currentRegistrations = _context.TournamentRegistrations
                .Count(r => r.TournamentId == tournamentId && r.Status != "Từ chối");

            if (currentRegistrations >= tournament.MaxParticipants)
                return false;

            // Tạo đơn đăng ký
            var registration = new TournamentRegistration
            {
                UserId = userId,
                TournamentId = tournamentId,
                RegisterDate = DateTime.Now,
                Status = "Đang chờ xác nhận",
                TeamName = teamName,
                NumberOfMembers = numberOfMembers
            };

            _context.TournamentRegistrations.Add(registration);
            return _context.SaveChanges() > 0;
        }


        public List<TournamentRegistration> GetRegistrationsByTournament(int tournamentId)
        {
            return _context.TournamentRegistrations
                .Include(r => r.User)
                .Include(r => r.Tournament)
                .Where(r => r.TournamentId == tournamentId)
                .ToList();
        }


        public List<TournamentRegistration> GetUserRegistrations(int userId)
        {
            return _context.TournamentRegistrations
                .Include(r => r.Tournament)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToList();
        }


        public bool UpdateRegistrationStatus(int registrationId, string newStatus)
        {
            var reg = _context.TournamentRegistrations
                .Include(r => r.Tournament)
                .FirstOrDefault(r => r.RegistrationId == registrationId);

            if (reg == null) return false;

            if (newStatus == "Đã được chấp nhận")
            {
                int approvedCount = _context.TournamentRegistrations
                    .Count(r => r.TournamentId == reg.TournamentId && r.Status == "Đã được chấp nhận");

                if (approvedCount >= reg.Tournament.MaxParticipants)
                    return false; // Không thể duyệt vì đã đầy
            }

            reg.Status = newStatus;
            return _context.SaveChanges() > 0;
        }

        public List<TournamentRegistration> GetUserRegistrationsInTournament(int userId, int tournamentId)
        {
            return _context.TournamentRegistrations
                .Include(r => r.Tournament)
                .Include(r => r.User)
                .Where(r => r.UserId == userId && r.TournamentId == tournamentId)
                .ToList();
        }

        public int CountApprovedRegistrations(int tournamentId)
        {
            return _context.TournamentRegistrations
                .Count(r => r.TournamentId == tournamentId && r.Status == "Đã được chấp nhận");
        }
        public bool CanApproveMore(int tournamentId)
        {
            var tournament = _context.Tournaments.FirstOrDefault(t => t.TournamentId == tournamentId);
            if (tournament == null) return false;

            int approved = CountApprovedRegistrations(tournamentId);
            return approved < tournament.MaxParticipants;
        }

    }
}

