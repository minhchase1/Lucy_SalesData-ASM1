using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class RevenueReport
    {
        public string DateOrMonth { get; set; } // VD: "2025-07-28" hoặc "2025-07"
        public decimal TotalRevenue { get; set; }
    }
}
