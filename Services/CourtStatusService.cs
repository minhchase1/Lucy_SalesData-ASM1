using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class CourtStatusService : ICourtStatusService
    {
        private readonly ICourtStatusRepository _courtStatusRepository;

        public CourtStatusService(ICourtStatusRepository courtStatusRepository)
        {
            _courtStatusRepository = courtStatusRepository;
        }

        public List<CourtStatus> GetAllStatuses()
        {
            return _courtStatusRepository.GetAll();
        }
    }
}
