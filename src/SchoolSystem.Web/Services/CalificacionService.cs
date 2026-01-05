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

        public async Task<ApiResponse<PagedResult<CalificacionDto>>> GetCalificacionesAsync(int page, int size)
        {
            return await _apiService.GetPagedAsync<CalificacionDto>($"api/Calificaciones?page={page}&size={size}");
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            return await _apiService.DeleteAsync<object>($"api/Calificaciones/{id}");
        }
    }
}
