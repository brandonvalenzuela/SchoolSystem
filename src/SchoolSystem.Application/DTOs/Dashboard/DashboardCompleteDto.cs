using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Dashboard
{
    public class DashboardCompleteDto : DashboardDto
    {
        public ChartDto StudentsChart { get; set; }
        public ChartDto FinanceChart { get; set; }
    }
}
