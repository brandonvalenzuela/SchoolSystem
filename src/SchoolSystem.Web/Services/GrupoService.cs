using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class GrupoService
    {
        private readonly ApiService _apiService;

        public GrupoService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<PagedResult<GrupoDto>>> GetGruposAsync(int page, int size)
        {
            return await _apiService.GetPagedAsync<GrupoDto>($"api/Grupos?page={page}&size={size}");
        }

        public async Task<ApiResponse<GrupoDto>> GetByIdAsync(int id)
        {
            return await _apiService.GetAsync<GrupoDto>($"api/Grupos/{id}");
        }

        public async Task<ApiResponse<int>> CreateAsync(CreateGrupoDto model)
        {
            return await _apiService.PostAsync<CreateGrupoDto, int>("api/Grupos", model);
        }

        public async Task<ApiResponse<int>> UpdateAsync(int id, UpdateGrupoDto model)
        {
            return await _apiService.PutAsync<UpdateGrupoDto, int>($"api/Grupos/{id}", model);
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            return await _apiService.DeleteAsync<object>($"api/Grupos/{id}");
        }

        public async Task<ApiResponse<List<GrupoDto>>> GetMisGruposAsync()
        {
            // Endpoint que devuelve List<GrupoDto>
            return await _apiService.GetAsync<List<GrupoDto>>("api/Grupos/mis-grupos");
        }
    }
}