using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TournamentRegistrationService : ITournamentRegistrationService
    {
        ITournamentRegistrationRepository repository;

        public TournamentRegistrationService(ITournamentRegistrationRepository repo)
        {
            repository = repo;
        }

        public bool RegisterUser(int userId, int tournamentId, string? teamName, int? numberOfMembers)
            => repository.RegisterUser(userId, tournamentId, teamName, numberOfMembers);

        public List<TournamentRegistration> GetRegistrationsByTournament(int tournamentId)
            => repository.GetRegistrationsByTournament(tournamentId);

        public List<TournamentRegistration> GetUserRegistrations(int userId)
            => repository.GetUserRegistrations(userId);

        public List<TournamentRegistration> GetUserRegistrationsInTournament(int userId, int tournamentId)
            => repository.GetUserRegistrationsInTournament(userId, tournamentId);

        public int CountApprovedRegistrations(int tournamentId)
            => repository.CountApprovedRegistrations(tournamentId);

        public bool CanApproveMore(int tournamentId)
            => repository.CanApproveMore(tournamentId);

        public bool ApproveRegistration(int registrationId)
            => repository.UpdateRegistrationStatus(registrationId, "Đã được chấp nhận");

        public bool RejectRegistration(int registrationId)
            => repository.UpdateRegistrationStatus(registrationId, "Từ chối");

        public bool UpdateRegistrationStatus(int registrationId, string newStatus)
            => repository.UpdateRegistrationStatus(registrationId, newStatus);
    }
}
