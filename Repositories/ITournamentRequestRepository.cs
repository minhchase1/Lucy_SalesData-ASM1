using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ITournamentRequestRepository
    {
        List<TournamentRequest> GetAllRequests();
        List<TournamentRequest> GetRequestsByStatus(string status);
        TournamentRequest? GetRequestById(int id);
        List<TournamentRequest> GetRequestsByUserId(int userId);
        bool AddRequest(TournamentRequest request);
        bool UpdateRequest(TournamentRequest request);
        bool UpdateRequestStatus(int requestId, string newStatus);
    }
}
