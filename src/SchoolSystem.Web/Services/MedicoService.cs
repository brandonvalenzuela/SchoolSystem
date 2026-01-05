using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class MedicoService
    {
        private readonly ApiService _apiService;

        public MedicoService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<ExpedienteMedicoDto>> GetByAlumnoIdAsync(int alumnoId)
        {
            // El endpoint devuelve 404 si no existe, el ApiService lo maneja
            return await _apiService.GetAsync<ExpedienteMedicoDto>($"api/Expedientes/alumno/{alumnoId}");
        }

        public async Task<ApiResponse<int>> CreateAsync(CreateExpedienteDto model)
        {
            return await _apiService.PostAsync<CreateExpedienteDto, int>("api/Expedientes", model);
        }

        // Puedes agregar UpdateAsync luego reutilizando el DTO de creación o uno específico
    }
}
