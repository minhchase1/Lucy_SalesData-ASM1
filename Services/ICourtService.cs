using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services
{
    public interface ICourtService
    {
        List<Court> GetAllCourts();
        List<Court> GetCourtsBySportId(int sportId);
        Court GetCourtById(int courtId);

        void UpdateCourt(Court court);
        void AddCourt(Court court);
        void DeleteCourt(int courtId);

    }
}
