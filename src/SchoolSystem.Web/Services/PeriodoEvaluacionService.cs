using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class PeriodoEvaluacionService
    {
        private readonly ApiService _apiService;

        public PeriodoEvaluacionService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public Task<ApiResponse<List<PeriodoEvaluacionDto>>> GetPorGrupoAsync(int grupoId, bool soloActivos = true)
        {
            return _apiService.GetAsync<List<PeriodoEvaluacionDto>>(
                $"api/PeriodosEvaluacion/por-grupo/{grupoId}?soloActivos={soloActivos.ToString().ToLowerInvariant()}");
        }
    }
}
