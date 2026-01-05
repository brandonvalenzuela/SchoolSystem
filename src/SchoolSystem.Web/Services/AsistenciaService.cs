using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class AsistenciaService
    {
        private readonly ApiService _apiService;

        public AsistenciaService(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Este es el método que te faltaba
        public async Task<ApiResponse<int>> CreateMasivoAsync(CreateAsistenciaMasivaDto model)
        {
            return await _apiService.PostAsync<CreateAsistenciaMasivaDto, int>("api/Asistencias/masivo", model);
        }

        // Métodos futuros que necesitarás
        public async Task<ApiResponse<PagedResult<AsistenciaDto>>> GetAsistenciasAsync(int page, int size)
        {
            return await _apiService.GetPagedAsync<AsistenciaDto>($"api/Asistencias?page={page}&size={size}");
        }

        public async Task<ApiResponse<List<ReporteMensualDto>>> GetReporteMensualAsync(int grupoId, int mes, int anio)
        {
            return await _apiService.GetAsync<List<ReporteMensualDto>>(
                $"api/Asistencias/reporte/mensual?grupoId={grupoId}&mes={mes}&anio={anio}");
        }
    }
}
