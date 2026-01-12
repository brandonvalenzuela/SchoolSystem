using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class RelacionService
    {
        private readonly ApiService _apiService;

        public RelacionService(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Obtener hijos de un padre
        public async Task<ApiResponse<List<AlumnoPadreDto>>> GetHijosPorPadre(int padreId)
        {
            return await _apiService.GetAsync<List<AlumnoPadreDto>>($"api/Relaciones/padre/{padreId}");
        }

        // Obtener padres de un alumno
        public async Task<ApiResponse<List<AlumnoPadreDto>>> GetPadresPorAlumno(int alumnoId)
        {
            return await _apiService.GetAsync<List<AlumnoPadreDto>>($"api/Relaciones/alumno/{alumnoId}");
        }

        // Crear vínculo
        public async Task<ApiResponse<int>> Vincular(CreateAlumnoPadreDto dto)
        {
            return await _apiService.PostAsync<CreateAlumnoPadreDto, int>("api/Relaciones", dto);
        }

        // Eliminar vínculo
        public async Task<ApiResponse<bool>> Desvincular(int id)
        {
            return await _apiService.DeleteAsync<bool>($"api/Relaciones/{id}");
        }
    }
}
