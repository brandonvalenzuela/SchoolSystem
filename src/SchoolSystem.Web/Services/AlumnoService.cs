using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class AlumnoService
    {
        private readonly ApiService _apiService;

        public AlumnoService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<PagedResult<AlumnoDto>>> GetAlumnosAsync(int page, int size)
        {
            // Llama al endpoint: GET api/Alumnos?page=1&size=10
            return await _apiService.GetPagedAsync<AlumnoDto>($"api/Alumnos?page={page}&size={size}");
        }

        public async Task<ApiResponse<AlumnoDto>> GetByIdAsync(int id)
        {
            return await _apiService.GetAsync<AlumnoDto>($"api/Alumnos/{id}");
        }

        public async Task<ApiResponse<int>> CreateAsync(CreateAlumnoDto model)
        {
            return await _apiService.PostAsync<CreateAlumnoDto, int>("api/Alumnos", model);
        }

        public async Task<ApiResponse<int>> UpdateAsync(int id, UpdateAlumnoDto model)
        {
            return await _apiService.PutAsync<UpdateAlumnoDto, int>($"api/Alumnos/{id}", model);
        }

        // Opcional: Método Delete
        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            return await _apiService.DeleteAsync<object>($"api/Alumnos/{id}");
        }
    }
}
