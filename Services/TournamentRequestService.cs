using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TournamentRequestService : ITournamentRequestService
    {
        ITournamentRequestRepository repository;
        public TournamentRequestService(ITournamentRequestRepository repo)
        { 
                repository = repo;
        }

        public List<TournamentRequest> GetAllRequests() => repository.GetAllRequests();

        public List<TournamentRequest> GetRequestsByStatus(string status) => repository.GetRequestsByStatus(status);

        public TournamentRequest? GetRequestById(int id) => repository.GetRequestById(id);

        public List<TournamentRequest> GetRequestsByUserId(int userId) => repository.GetRequestsByUserId(userId);

        public bool AddRequest(TournamentRequest request) => repository.AddRequest(request);

        public bool UpdateRequest(TournamentRequest request) => repository.UpdateRequest(request);

        public bool UpdateRequestStatus(int requestId, string newStatus) => repository.UpdateRequestStatus(requestId, newStatus);
    }
}
