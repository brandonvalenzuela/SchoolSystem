using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class DashboardService
    {
        private readonly ApiService _apiService;

        public DashboardService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<DashboardDto>> GetStatsAsync()
        {
            return await _apiService.GetAsync<DashboardDto>("api/Dashboard");
        }
    }
}
