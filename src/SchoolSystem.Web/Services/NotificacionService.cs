using SchoolSystem.Web.Models;

namespace SchoolSystem.Web.Services
{
    public class NotificacionService
    {
        private readonly ApiService _apiService;

        public NotificacionService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<PagedResult<NotificacionDto>>> GetMisNotificacionesAsync(int page, int size)
        {
            // Asumiendo que crearás este endpoint o filtrarás por usuario en el backend
            return await _apiService.GetPagedAsync<NotificacionDto>($"api/Notificaciones/mis-notificaciones?page={page}&size={size}");
        }

        public async Task<ApiResponse<bool>> MarcarComoLeida(int id)
        {
            return await _apiService.PutAsync<object, bool>($"api/Notificaciones/{id}/leer", null);
        }

        // Método auxiliar para obtener conteo de no leídas (para la campanita)
        public async Task<ApiResponse<int>> GetConteoNoLeidas()
        {
            return await _apiService.GetAsync<int>("api/Notificaciones/conteo-noleidas");
        }
    }
}
