using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class MaestroService
    {
        private readonly ApiService _apiService;

        public MaestroService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<PagedResult<MaestroDto>>> GetMaestrosAsync(int page, int size)
        {
            return await _apiService.GetPagedAsync<MaestroDto>($"api/Maestros?page={page}&size={size}");
        }

        public async Task<ApiResponse<MaestroDto>> GetByIdAsync(int id)
        {
            return await _apiService.GetAsync<MaestroDto>($"api/Maestros/{id}");
        }

        public async Task<ApiResponse<int>> CreateAsync(CreateMaestroDto model)
        {
            return await _apiService.PostAsync<CreateMaestroDto, int>("api/Maestros", model);
        }

        public async Task<ApiResponse<int>> UpdateAsync(int id, UpdateMaestroDto model)
        {
            return await _apiService.PutAsync<UpdateMaestroDto, int>($"api/Maestros/{id}", model);
        }

        // Recuerda que en el Backend cambiamos para que retorne bool
        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            return await _apiService.DeleteAsync<object>($"api/Maestros/{id}");
        }
    }
}
