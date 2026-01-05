using SchoolSystem.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetStatsAsync();
    }
}
