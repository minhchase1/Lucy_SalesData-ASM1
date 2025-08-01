using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITournamentRegistrationService
    {
        bool RegisterUser(int userId, int tournamentId, string? teamName, int? numberOfMembers);
        List<TournamentRegistration> GetRegistrationsByTournament(int tournamentId);
        List<TournamentRegistration> GetUserRegistrations(int userId);
        List<TournamentRegistration> GetUserRegistrationsInTournament(int userId, int tournamentId);
        bool UpdateRegistrationStatus(int registrationId, string newStatus);
        int CountApprovedRegistrations(int tournamentId);
        bool CanApproveMore(int tournamentId);
        bool ApproveRegistration(int registrationId);
        bool RejectRegistration(int registrationId);
    }
}
