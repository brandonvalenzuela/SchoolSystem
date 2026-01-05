using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class ConductaService
    {
        private readonly ApiService _apiService;

        public ConductaService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<PagedResult<RegistroConductaDto>>> GetRegistrosAsync(int page, int size)
        {
            return await _apiService.GetPagedAsync<RegistroConductaDto>($"api/RegistrosConducta?page={page}&size={size}");
        }

        public async Task<ApiResponse<int>> CreateAsync(CreateRegistroConductaDto model)
        {
            return await _apiService.PostAsync<CreateRegistroConductaDto, int>("api/RegistrosConducta", model);
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            return await _apiService.DeleteAsync<object>($"api/RegistrosConducta/{id}");
        }
    }
}
