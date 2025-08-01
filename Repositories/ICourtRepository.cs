using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Repositories
{
    public interface ICourtRepository
    {
        List<Court> GetAllCourts();
        List<Court> GetCourtsBySportId(int sportId);
        Court?GetCourtById(int courtId);
        void Add(Court court);
        void Update(Court court);
        void Delete(int courtId);

    }
}
