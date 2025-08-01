using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ITournamentRegistrationRepository
    {
        bool RegisterUser(int userId, int tournamentId, string? teamName, int? numberOfMembers);
        List<TournamentRegistration> GetRegistrationsByTournament(int tournamentId);
        List<TournamentRegistration> GetUserRegistrations(int userId);
        bool UpdateRegistrationStatus(int registrationId, string newStatus);
        List<TournamentRegistration> GetUserRegistrationsInTournament(int userId, int tournamentId);
        int CountApprovedRegistrations(int tournamentId);
        bool CanApproveMore(int tournamentId);
    }
}
