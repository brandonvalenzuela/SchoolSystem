using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class MateriaService
    {
        private readonly ApiService _apiService;
        public MateriaService(ApiService apiService) => _apiService = apiService;

        public async Task<ApiResponse<PagedResult<MateriaDto>>> GetAsync(int page, int size)
            => await _apiService.GetPagedAsync<MateriaDto>($"api/Materias?page={page}&size={size}");

        public async Task<ApiResponse<MateriaDto>> GetByIdAsync(int id)
            => await _apiService.GetAsync<MateriaDto>($"api/Materias/{id}");

        public async Task<ApiResponse<int>> CreateAsync(CreateMateriaDto model)
            => await _apiService.PostAsync<CreateMateriaDto, int>("api/Materias", model);

        public async Task<ApiResponse<int>> UpdateAsync(int id, UpdateMateriaDto model)
            => await _apiService.PutAsync<UpdateMateriaDto, int>($"api/Materias/{id}", model);

        public async Task<ApiResponse<object>> DeleteAsync(int id)
            => await _apiService.DeleteAsync<object>($"api/Materias/{id}");
    }
}
