using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class GradoService
    {
        private readonly ApiService _apiService;

        public GradoService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<PagedResult<GradoDto>>> GetGradosAsync(int page, int size)
        {
            return await _apiService.GetPagedAsync<GradoDto>($"api/Grados?page={page}&size={size}");
        }

        public async Task<ApiResponse<GradoDto>> GetByIdAsync(int id)
        {
            return await _apiService.GetAsync<GradoDto>($"api/Grados/{id}");
        }

        public async Task<ApiResponse<int>> CreateAsync(CreateGradoDto model)
        {
            return await _apiService.PostAsync<CreateGradoDto, int>("api/Grados", model);
        }

        public async Task<ApiResponse<int>> UpdateAsync(int id, UpdateGradoDto model)
        {
            return await _apiService.PutAsync<UpdateGradoDto, int>($"api/Grados/{id}", model);
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            return await _apiService.DeleteAsync<object>($"api/Grados/{id}");
        }
    }
}
