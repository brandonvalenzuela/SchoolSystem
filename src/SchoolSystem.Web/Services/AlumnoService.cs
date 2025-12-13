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

        // Aquí agregaremos luego Create, Update, Delete
    }
}
