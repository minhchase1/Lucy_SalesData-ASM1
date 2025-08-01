﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories
{
    public interface IReportRepository
    {
        List<RevenueReport> GetDailyRevenue();
        List<RevenueReport> GetMonthlyRevenue();
    }
}
