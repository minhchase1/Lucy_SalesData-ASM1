using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITournamentRequestService
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
