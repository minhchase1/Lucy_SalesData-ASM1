using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public List<RevenueReport> GetMonthlyRevenue()
        {
            return _reportRepository.GetMonthlyRevenue();
        }

        public List<RevenueReport> GetDailyRevenue()
        {
            return _reportRepository.GetDailyRevenue();
        }
    }
}
