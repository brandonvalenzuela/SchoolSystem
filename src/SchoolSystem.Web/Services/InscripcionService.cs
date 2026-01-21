using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class InscripcionService
    {
        private readonly ApiService _apiService;
        private readonly AcademicContextService _academicContext;

        public InscripcionService(ApiService apiService, AcademicContextService academicContext)
        {
            _apiService = apiService;
            _academicContext = academicContext;
        }

        public async Task<ApiResponse<PagedResult<InscripcionDto>>> GetInscripcionesAsync(int page, int size)
        {
            return await _apiService.GetPagedAsync<InscripcionDto>($"api/Inscripciones?page={page}&size={size}");
        }

        public async Task<ApiResponse<InscripcionDto>> GetByIdAsync(int id)
        {
            return await _apiService.GetAsync<InscripcionDto>($"api/Inscripciones/{id}");
        }

        public async Task<ApiResponse<int>> CreateAsync(CreateInscripcionDto model)
        {
            return await _apiService.PostAsync<CreateInscripcionDto, int>("api/Inscripciones", model);
        }

        public async Task<ApiResponse<int>> UpdateAsync(int id, UpdateInscripcionDto model)
        {
            return await _apiService.PutAsync<UpdateInscripcionDto, int>($"api/Inscripciones/{id}", model);
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            return await _apiService.DeleteAsync<object>($"api/Inscripciones/{id}");
        }

        public Task<ApiResponse<List<InscripcionDto>>> GetAlumnosPorGrupoAsync(int grupoId, bool soloActivos = true)
            => _apiService.GetAsync<List<InscripcionDto>>($"api/Inscripciones/grupo/{grupoId}?soloActivos={soloActivos.ToString().ToLowerInvariant()}");



        public async Task<ApiResponse<List<InscripcionDto>>> GetHistorialAlumnoAsync(int alumnoId)
        {
            // Llama al nuevo endpoint que creamos en el Backend
            return await _apiService.GetAsync<List<InscripcionDto>>($"api/Inscripciones/historial/alumno/{alumnoId}");
        }
    }
}
