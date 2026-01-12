using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class PadreService
    {
        private readonly ApiService _apiService;

        public PadreService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<PagedResult<PadreDto>>> GetPadresAsync(int page, int size, string termino = "")
        {
            var url = $"api/Padres?page={page}&size={size}";
            if (!string.IsNullOrEmpty(termino))
            {
                // Asumiendo que tu backend ya soporta filtros como hicimos con Alumnos
                // Si no, tendrás que agregarlo al Backend primero (PadresController).
                url += $"&TerminoBusqueda={Uri.EscapeDataString(termino)}";
            }
            return await _apiService.GetPagedAsync<PadreDto>(url);
        }

        public async Task<ApiResponse<PadreDto>> GetByIdAsync(int id)
        {
            return await _apiService.GetAsync<PadreDto>($"api/Padres/{id}");
        }

        public async Task<ApiResponse<int?>> CreateAsync(CreatePadreDto model)
        {
            return await _apiService.PostAsync<CreatePadreDto, int?>("api/Padres", model);
        }

        public async Task<ApiResponse<int>> UpdateAsync(int id, UpdatePadreDto model)
        {
            return await _apiService.PutAsync<UpdatePadreDto, int>($"api/Padres/{id}", model);
        }
    }
}
