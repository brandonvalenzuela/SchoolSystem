using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class UsuarioService
    {
        private readonly ApiService _apiService;

        public UsuarioService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<PagedResult<UsuarioDto>>> GetUsuariosAsync(int page, int size)
        {
            return await _apiService.GetPagedAsync<UsuarioDto>($"api/Usuarios?page={page}&size={size}");
        }

        public async Task<ApiResponse<UsuarioDto>> GetByIdAsync(int id)
        {
            return await _apiService.GetAsync<UsuarioDto>($"api/Usuarios/{id}");
        }

        public async Task<ApiResponse<int>> CreateAsync(CreateUsuarioDto model)
        {
            return await _apiService.PostAsync<CreateUsuarioDto, int>("api/Usuarios", model);
        }

        public async Task<ApiResponse<int>> UpdateAsync(int id, UpdateUsuarioDto model)
        {
            return await _apiService.PutAsync<UpdateUsuarioDto, int>($"api/Usuarios/{id}", model);
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            return await _apiService.DeleteAsync<object>($"api/Usuarios/{id}");
        }
    }
}
