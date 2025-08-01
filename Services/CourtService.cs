using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;

namespace Services
{
    public class CourtService : ICourtService
    {
        private readonly ICourtRepository _courtRepository;

        public CourtService(ICourtRepository courtRepository)
        {
            _courtRepository = courtRepository;
        }

        public Court GetCourtById(int courtId)
        {
            return _courtRepository.GetCourtById(courtId);
        }

        public List<Court> GetCourtsBySportId(int sportId)
        {
            return _courtRepository.GetCourtsBySportId(sportId);
        }

        public List<Court> GetAllCourts()
        {
            return _courtRepository.GetAllCourts();
        }

        public void UpdateCourt(Court court)
        {
             _courtRepository.Update(court);
        }

        public void AddCourt(Court court)
        {
            _courtRepository.Add(court);
        }

        public void DeleteCourt(int courtId)
        {
            _courtRepository.Delete(courtId);
        }
    }
}
