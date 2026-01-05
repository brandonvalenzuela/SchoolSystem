using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class PagoService
    {
        private readonly ApiService _apiService;

        public PagoService(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Obtener historial de pagos
        public async Task<ApiResponse<PagedResult<PagoDto>>> GetPagosAsync(int page, int size)
        {
            return await _apiService.GetPagedAsync<PagoDto>($"api/Pagos?page={page}&size={size}");
        }

        // Obtener deudas de un alumno específico
        public async Task<ApiResponse<List<CargoDto>>> GetCargosPendientesAsync(int alumnoId)
        {
            // Ojo: El endpoint devuelve List, no PagedResult
            return await _apiService.GetAsync<List<CargoDto>>($"api/Pagos/pendientes/alumno/{alumnoId}");
        }

        // Registrar un cobro
        public async Task<ApiResponse<int>> RegistrarPagoAsync(CreatePagoDto model)
        {
            return await _apiService.PostAsync<CreatePagoDto, int>("api/Pagos", model);
        }

        public async Task<ApiResponse<bool>> CancelarPagoAsync(int id, string motivo)
        {
            var dto = new CancelarPagoDto { Motivo = motivo };
            return await _apiService.PostAsync<CancelarPagoDto, bool>($"api/Pagos/{id}/cancelar", dto);
        }
    }
}
