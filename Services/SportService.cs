using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class SportService : ISportService
    {
        private readonly ISportRepository _sportRepository;

        public SportService(ISportRepository sportRepository)
        {
            _sportRepository = sportRepository;
        }

        public List<Sport> GetAllSports()
        {
            return _sportRepository.GetAllSports();
        }

        public Sport? GetSportById(int id)
        {
            return _sportRepository.GetSportById(id);
        }

        public void AddSport(Sport sport)
        {
            _sportRepository.AddSport(sport);
        }

        public void UpdateSport(Sport sport)
        {
            _sportRepository.UpdateSport(sport);
        }

        public void DeleteSport(int id)
        {
            _sportRepository.DeleteSport(id);
        }
    }
}
