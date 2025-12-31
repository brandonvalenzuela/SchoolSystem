using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class CalificacionService
    {
        private readonly ApiService _apiService;

        public CalificacionService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<int>> CreateMasivoAsync(CreateCalificacionMasivaDto model)
        {
            return await _apiService.PostAsync<CreateCalificacionMasivaDto, int>("api/Calificaciones/masivo", model);
        }
    }
}
